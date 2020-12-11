using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCauldron : CraftingStation
{
    public UI_PromptStack promptStack;


    [SerializeField] protected List<GameObject> candles;
    [SerializeField] protected GameObject candleReplacement;
    [SerializeField] protected int candlesLit = 0;
    [SerializeField] private GameObject subStation;

    // Start is called before the first frame update
    void Start()
    {
        promptStack.setPrompt(UI_PromptStack.Button.BUTTON_1, "Add to Cauldron");
        promptStack.setPrompt(UI_PromptStack.Button.BUTTON_2, "Add to side-pot");
        promptStack.setPrompt(UI_PromptStack.Button.BUTTON_3, "Finish Mixing");
        promptStack.setPrompt(UI_PromptStack.Button.BUTTON_4, "Go to Pantry");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void activate()
    {
        if (candlesLit < candles.Count)
        {
            Debug.Log("light candle");
            candles[candlesLit].SetActive(false);//temp
            candlesLit++;
        }
        else
            Debug.Log("no more uses");
    }
}
