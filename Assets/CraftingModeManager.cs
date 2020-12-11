using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingModeManager : MonoBehaviour
{
    //singleton
    private static CraftingModeManager _instance;
    public static CraftingModeManager Instance { get { return _instance; } }

    private bool isChoosing = true;//TODO: remove
    [SerializeField] private StationSelectUI choosingUI;//TODO: remove
    [SerializeField] private GameObject enterUI;//TODO: remove
    [SerializeField] private GameObject leaveUI;//TODO: remove

    private float tempCraftingTimer = 0;
    [SerializeField] private float maxCraftingTimer = 2;
    [SerializeField] UnityEngine.UI.Text tempCraftingTimerText;

    [SerializeField]
    private float maxJoySnapDelay = 1;
    private float joySnapDelay = 0;

    private List<Recipe> recipeList;
    public Recipe targetRecipe;//the current selected recipe for crafting... can be null

    public List<string> inventory;//TODO: object class
    //TODO: make private since I have perfectly reasonable checking functions now
    public UI_Lab uiRef;//TODO: ue this

    [Header("Stations")]
    public PopUp startingPop;
    [SerializeField] private CraftingStation stationPrep;
    [SerializeField] private CraftingStation stationCrafting;
    [SerializeField] private CraftingStation stationInventory;
    [SerializeField] private CraftingStation stationNotes;
    [SerializeField] private List<CraftingStation> stations;//TODO: remove
    private int currOpID = 0;//TODO: remove

    [Header("States")]
    private bool doneStartup = false;
    private bool doneHarvest = false;

    private void Awake()
    {
        //singleton init
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;


        //load in recipes
        recipeList = new List<Recipe>();
        recipeList = Recipe.loadAllRecipes();
    }

    // Start is called before the first frame update
    void Start()
    {
        enterUI.SetActive(false);
        choosingUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startingPop)
        {
            //TODO: there is an issue where the starting popup check doesnt work unless it is enabled from the start. I dont get it. Says completed internally, but not compkleted, externally
            if (startingPop.isCompleted)
            {
                if (isChoosing)
                {
                    PlayerCamera.Instance.transform.LookAt(stations[currOpID].transform);

                    if (isChoosing && !choosingUI.gameObject.activeInHierarchy)
                        choosingUI.gameObject.SetActive(true);

                    //joystick input delay
                    joySnapDelay -= Time.deltaTime;
                    if (joySnapDelay <= 0)
                    {
                        joySnapDelay = maxJoySnapDelay;

                        float scroll = Input.GetAxis("Horizontal");
                        if (scroll > 0)
                        {
                            stations[currOpID].setPreviewState(false);
                            currOpID++;
                            if (currOpID >= stations.Count)
                            {
                                currOpID = 0;
                            }
                            stations[currOpID].setPreviewState(true);
                        }
                        else if (scroll < 0)
                        {
                            stations[currOpID].setPreviewState(false);
                            currOpID--;
                            if (currOpID < 0)
                            {
                                currOpID = stations.Count - 1;
                            }
                            stations[currOpID].setPreviewState(true);
                        }

                        choosingUI.setName(stations[currOpID].displayName);

                        if (doneStartup && stations[currOpID].isEnterable)
                            enterUI.SetActive(true);
                        else
                            enterUI.SetActive(false);
                    }

                    CraftingStation curOp = stations[currOpID];
                    //selecting
                    if (doneStartup && Input.GetKeyDown("q") && curOp.isEnterable)
                    {
                        curOp.setState(true);
                        enterUI.SetActive(false);
                        leaveUI.SetActive(true);
                        isChoosing = false;

                        getTargetRecipe(curOp.toolType);
                    }

                    doneStartup = true;
                }
                else//not choosing
                {
                    //leaving
                    if (doneStartup && Input.GetKeyDown("e"))
                    {
                        CraftingStation curOp = stations[currOpID];
                        curOp.setPreviewState(true);
                        isChoosing = true;
                        enterUI.SetActive(true);
                        leaveUI.SetActive(false);
                    }

                    //crafting
                    if(doneStartup && Input.GetKeyDown("r") && tempCraftingTimer <= 0)
                    {
                        CraftingStation curOp = stations[currOpID];
                        curOp.activate();//tempversion of this call
                        if (targetRecipe != null)
                        {
                            addToInventory(targetRecipe.ingredientOutput);
                            tempCraftingTimer = maxCraftingTimer;
                        }
                        getTargetRecipe(curOp.toolType);
                    }
                }
            }
        }

        //guaranteed execution things
        //below here
        if (tempCraftingTimer > 0)
            tempCraftingTimer -= Time.deltaTime;
        else if (tempCraftingTimer < 0)
            tempCraftingTimer = 0;
        tempCraftingTimerText.text = tempCraftingTimer.ToString("#.00");
    }

    //TODO: duplicate
    public List<Recipe> getRecipesForTool(string toolType)
    {
        List<Recipe> result = new List<Recipe>();    
        foreach(Recipe r in recipeList)
        {
            if (r.toolType == toolType)
                result.Add(r);
        }
        return result;
    }

    /// <summary>
    /// checks for the presence of a particular item, in player inventory
    /// </summary>
    /// <param name="item">the item to look for</param>
    /// <returns>true if present</returns>
    public bool isInInventory(string item)
    {
        bool result = false;

        foreach(string s in inventory)
        {
            if (s == item)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// item to add to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void addToInventory(string item)
    {
        //only add if it isnt already present
        if (!isInInventory(item))
        {
            inventory.Add(item);
            Debug.Log("Inventory Added: " + item + "\nExpand for full Inv.\n" + getInventoryString());
        }
        else
            Debug.Log("Inventory item not added: " + item);
    }

    public string getInventoryString()
    {
        string result = "";
        foreach (string s in inventory)
            result += s + "\n";
        return result;
    }

    private void getTargetRecipe(string station)
    {
        List<Recipe> l = Recipe.getPossibleRecipes(recipeList, inventory, station);
        targetRecipe = null;
        foreach (Recipe r in l)
        {
            if (!isInInventory(r.ingredientOutput) &&
                r.ingredientOutput != "" &&
                targetRecipe == null)
            {
                targetRecipe = r;
            }
        }

        if (targetRecipe != null)
            Debug.Log("Target Recipe for: " + targetRecipe.ingredientOutput);
        else
            Debug.Log("No new recipes to craft");
    }
}

public class Recipe
{
    //TODO: make these read only
    //TODO: have a ctor that loads these all in from a line in the csv reader
    public string ingredientOutput;
    public string toolType;
    public string ingredientA;
    public string ingredientB;
    public string craftingActionType;

    public string write(string id = "")
    {
        string append = "";
        if (id != "")
            append = id + ") ";

        return append + ingredientOutput + " | " + toolType + " | " + ingredientA + " | " + ingredientB + " | " + craftingActionType;
    }

    public static string writeHeading()
    {
        return "ingredientOutput | toolType | ingredientA | ingredientB | craftingActionType";
    }

    /// <summary>
    /// turns all provided recipes into a string with header
    /// </summary>
    /// <param name="recipeList">list of recipe objects</param>
    /// <param name="showHeader">true to show header line</param>
    /// <returns></returns>
    public static string writeRecipeList(List<Recipe> recipeList, bool showHeader = true)
    {
        int i = 0;
        string result = Recipe.writeHeading();
        foreach (Recipe r in recipeList)
        {
            result += "\n" + r.write(i.ToString());
            i++;
        }
        return result;
    }

    /// <summary>
    /// static function to read in all recpe data from csv into a List of Recipe
    /// </summary>
    /// <param name="filename">name of the csv file</param>
    /// <returns>a list of all recipes in the data</returns>
    public static List<Recipe> loadAllRecipes(string filename = "recipes")
    {
        List<Recipe> result = new List<Recipe>();
        List<Dictionary<string, object>> recipeData;
        recipeData = CSVReader.Read(filename);
        //string recipeDebug = "LOADED - Recipe Data\n (click for details)\n\n";

        for (int i = 0; i < recipeData.Count; i++)
        {
            //recipeDebug += i + ": " + recipeData[i]["ingredientOutput"] + "\n";
            Recipe r = new Recipe();
            r.ingredientOutput = recipeData[i]["ingredientOutput"].ToString();
            r.toolType = recipeData[i]["toolType"].ToString();
            r.ingredientA = recipeData[i]["ingredientA"].ToString();
            r.ingredientB = recipeData[i]["ingredientB"].ToString();
            r.craftingActionType = recipeData[i]["craftingActionType"].ToString();

            result.Add(r);
        }
        //Debug.Log(recipeDebug);

        Debug.Log("LOADED - Recipe Data\n(click for details)\n\n" + Recipe.writeRecipeList(result));
        return result;
    }

    //TODO: duplicate
    public static List<Recipe> getStationRecipes(List<Recipe> recipes, string station)
    {
        List<Recipe> result = new List<Recipe>();

        foreach (Recipe r in recipes)
        {
            if (r.toolType == station)
                result.Add(r);
        }

        return result;
    }

    public static List<Recipe> getPossibleRecipes(List<Recipe> recipes, List<string> inv, string station)
    {
        List<Recipe> result = new List<Recipe>();

        List<Recipe> stationRecipes = getStationRecipes(recipes, station);

        foreach(Recipe r in stationRecipes)
        {
            bool isViable = false;
            bool a = false;
            bool b = false;
            foreach(string s in inv)
            {
                if (s == r.ingredientA || r.ingredientA=="")
                    a = true;
                if (s == r.ingredientB || r.ingredientB=="")
                    b = true;
                if (a && b)
                {
                    isViable = true;
                    break;
                }
            }

            if (isViable)
                result.Add(r);
        }

        return result;
    }
}