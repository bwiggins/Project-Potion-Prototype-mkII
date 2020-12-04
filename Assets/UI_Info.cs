using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Info : MonoBehaviour
{
    public Text targetRecipeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Recipe targetR = CraftingModeManager.Instance.targetRecipe;
        if (targetR == null)
            targetRecipeText.text = "No New Recipes to craft";
        else
            targetRecipeText.text = "Press R to craft " + targetR.ingredientOutput;
    }
}
