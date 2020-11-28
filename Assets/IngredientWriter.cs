using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientWriter : MonoBehaviour
{
    public Text textbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textbox.text = "";
        foreach (string s in CraftingModeManager.Instance.inventory)
            textbox.text += "- " + s + "\n";
    }
}
