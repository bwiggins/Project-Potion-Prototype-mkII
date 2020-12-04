using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingModeManager : MonoBehaviour
{
    //singleton
    private static CraftingModeManager _instance;
    public static CraftingModeManager Instance { get { return _instance; } }

    private bool isChoosing = true;
    private bool doneStartup = false;
    [SerializeField] private StationSelectUI choosingUI;
    [SerializeField] private GameObject enterUI;
    [SerializeField] private GameObject leaveUI;

    [SerializeField]
    private float maxJoySnapDelay = 1;
    private float joySnapDelay = 0;

    private List<Recipe> recipeList;
    public Recipe targetRecipe;//the current selected recipe for crafting... can be null

    public List<string> inventory;//TODO: object class
    //TODO: make private since I have perfectly reasonable checking functions now

    [SerializeField]
    private List<CraftingOperation> operations;
    private int currOpID = 0;

    public UI_Lab uiRef;//TODO: ue this
    public PopUp startingPop;

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


                    PlayerCamera.Instance.transform.LookAt(operations[currOpID].transform);

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
                            operations[currOpID].setPreviewState(false);
                            currOpID++;
                            if (currOpID >= operations.Count)
                            {
                                currOpID = 0;
                            }
                            operations[currOpID].setPreviewState(true);
                        }
                        else if (scroll < 0)
                        {
                            operations[currOpID].setPreviewState(false);
                            currOpID--;
                            if (currOpID < 0)
                            {
                                currOpID = operations.Count - 1;
                            }
                            operations[currOpID].setPreviewState(true);
                        }

                        choosingUI.setName(operations[currOpID].displayName);

                        if (doneStartup && operations[currOpID].isEnterable)
                            enterUI.SetActive(true);
                        else
                            enterUI.SetActive(false);
                    }

                    CraftingOperation curOp = operations[currOpID];
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
                        CraftingOperation curOp = operations[currOpID];
                        curOp.setPreviewState(true);
                        isChoosing = true;
                        enterUI.SetActive(true);
                        leaveUI.SetActive(false);
                    }

                    //crafting
                    if(doneStartup && Input.GetKeyDown("r"))
                    {
                        CraftingOperation curOp = operations[currOpID];
                        addToInventory(targetRecipe.ingredientOutput);
                        getTargetRecipe(curOp.toolType);
                    }
                }
            }
        }
    }

    [SerializeField]
    private List<Clutter> clutterList;

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
            Debug.Log("Inventory Added: " + item);
        }
        else
            Debug.Log("Inventory item not added: " + item);
    }

    private void getTargetRecipe(string station)
    {
        List<Recipe> l = Recipe.getPossibleRecipes(recipeList, inventory, station);
        targetRecipe = null;
        foreach (Recipe r in l)
        {
            Debug.Log(r.write());
            if (!isInInventory(r.ingredientOutput) &&
                r.ingredientOutput != "" &&
                targetRecipe == null)
            {
                targetRecipe = r;
                Debug.Log("Target Recipe for: " + r.ingredientOutput);
            }
        }

        if (targetRecipe == null)
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