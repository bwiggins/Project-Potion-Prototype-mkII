using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CraftingTool : Interaction
{
    public string toolType;
    private List<Recipe> recipes;

    private void Awake()
    {
       
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
        
    }

    protected override void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
            UI_Lab.Instance.setActionTool(this);
    }

    protected override void OnTriggerExit(Collider other)
    {
        UI_Lab.Instance.setActionTool(null);
    }

    public override string activate()
    {
        UnityEngine.Debug.Log("getting: " + itemReward);
        return base.activate();
    }

}
