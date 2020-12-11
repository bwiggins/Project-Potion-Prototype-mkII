using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCauldron : CraftingStation
{

    [SerializeField] protected List<GameObject> candles;
    [SerializeField] protected GameObject candleReplacement;
    [SerializeField] protected int candlesLit = 0;
    [SerializeField] private GameObject subStation;

    // Start is called before the first frame update
    void Start()
    {
        
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
