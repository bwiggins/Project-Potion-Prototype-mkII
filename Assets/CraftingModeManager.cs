using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingModeManager : MonoBehaviour
{
    //singleton
    private static CraftingModeManager _instance;
    public static CraftingModeManager Instance { get { return _instance; } }


    private List<Recipe> recipeList;


    private static float desiredTimeDialation = 1;
    private static float currentTimeDialation = 1;
    //private actual time value (needs a get value for other func to access

    public UI_Lab uiRef;//TODO: ue this

    private static bool isPaused = false;
    public static void pauseToggle()
    {
        if (isPaused)
            isPaused = false;
        else
            isPaused = true;
    }
    public static void pauseSet(bool willPause) { isPaused = willPause; }
    private static void pause() { isPaused = true; } //backup time value, and et actual to 0
    private static void unpause() { isPaused = false; } //restore time value
    public static float getTimeDialation() { return currentTimeDialation; }
    public static void setTimeDialation(float desiredTime) { desiredTimeDialation = desiredTime; }

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
        if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 9"))
        {
            if (!isPaused)
            {
                pause();
                uiRef.menuPause.SetActive(true);
            }
            else
            {
                unpause();
                uiRef.menuPause.SetActive(false);

            }
        }


        if (isPaused)
        {
            currentTimeDialation = 0;
            if (Input.GetKeyDown("joystick button 0"))
                clearNextClutter();
            if (Input.GetKeyDown("joystick button 3"))
                quit();
        }
        else
            currentTimeDialation = desiredTimeDialation;
    }

    public void quit()
    {
        Application.Quit();
    }

    [SerializeField]
    private List<Clutter> clutterList;

    public void clearNextClutter()
    {
        clutterList[0].clear();
        clutterList.RemoveAt(0);
    }

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