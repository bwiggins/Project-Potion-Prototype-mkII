using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CraftingTool : Interaction
{
    public string toolType;
    private List<Recipe> recipes;

    [SerializeField]
    private List<string> steps;//list of steps in the crafting
    private int currentStep = 0;//current step

    protected void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        //looks for all the recipes possible
        recipes = CraftingModeManager.Instance.getRecipesForTool(toolType);

        string debugString = "";
        foreach (Recipe r in recipes)
            debugString += r.write() + "\n";

        if (debugString == "") debugString = "NO RECIPES";

        UnityEngine.Debug.Log(toolType + " recipes:\n(click for details)\n\n" + debugString);

        //TESTING LINE
        //only give rewards for items had
        if (recipes.Count > 0)
            itemReward = recipes[0].ingredientOutput;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (steps.Count > currentStep)
            {
                if (Input.GetKeyDown(steps[currentStep]))
                {
                    UnityEngine.Debug.Log("completed step " + currentStep + " (" + steps[currentStep] + ")");
                    currentStep++;
                }
            }
            else
            {
                UnityEngine.Debug.Log("no more steps");
                deactivate();
            }
        }
    }

    public override string activate()
    {
        UnityEngine.Debug.Log("getting: " + itemReward);
        return base.activate();
    }

    public override void setReady(bool readyState)
    {
        isReady = readyState;

        if (isReady)
        {
            UI_Lab.Instance.setActionTool(this);
        }
    }

}
