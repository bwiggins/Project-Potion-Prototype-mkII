using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingModeManager : MonoBehaviour
{
    //singleton
    private static CraftingModeManager _instance;
    public static CraftingModeManager Instance { get { return _instance; } }

    private bool isChoosing = false;
    [SerializeField] private StationSelectUI choosingUI;

    [SerializeField]
    private float maxJoySnapDelay = 1;
    private float joySnapDelay = 0;

    private List<Recipe> recipeList;

    public List<string> inventory;//TODO: object class

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

    }

    // Update is called once per frame
    void Update()
    {
        if (startingPop)
        {
            //TODO: there is an issue where the starting popup check doesnt work unless it is enabled from the start. I dont get it. Says completed internally, but not compkleted, externally
            if (startingPop.isCompleted)
            {
                isChoosing = true;

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
                        currOpID++;
                        if (currOpID >= operations.Count)
                        {
                            currOpID = 0;
                        }
                        operations[currOpID].setPreviewState(true);
                    }

                    choosingUI.setName(operations[currOpID].displayName);
                }

                //measure joystick delay duration
                /*float scroll = Input.GetAxis("Horizontal");
                if (scroll > 0)
                {
                    if (joysnap <= 0)
                        joysnap = maxJoySnap;//change in direction resets
                    else
                        joysnap += Time.deltaTime;//add up time until the max 
                }
                else if (scroll < 0)
                {
                    if (joysnap >= 0)
                        joysnap = -maxJoySnap;//change in direction resets
                    else
                        joysnap -= Time.deltaTime;//add up time until the max 
                }
                else
                    joysnap = 0;// if released, then we need to reset

                //if joystick delay has crossed the threshold, we do someything and reset
                if(joysnap >= maxJoySnap)
                {
                    joysnap = 0;
                    operations[currOpID].setState(false);
                    currOpID++;
                    if (currOpID >= operations.Count)
                    {
                        currOpID = 0;
                    }
                    operations[currOpID].setState(true);
                }
                else if (joysnap <= -maxJoySnap)
                {
                    joysnap = 0;
                    operations[currOpID].setState(false);
                    currOpID++;
                    if (currOpID >= operations.Count)
                    {
                        currOpID = 0;
                    }
                    operations[currOpID].setState(true);
                }*/
            }
        }
    }

    [SerializeField]
    private List<Clutter> clutterList;


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

    public string write()
    {
        return ingredientOutput + " | " + toolType + " | " + ingredientA + " | " + ingredientB + " | " + craftingActionType;
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
        string result = Recipe.writeHeading();
        foreach (Recipe r in recipeList)
            result += "\n" + r.write();
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
}