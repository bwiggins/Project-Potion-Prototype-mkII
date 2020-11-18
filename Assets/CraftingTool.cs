using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CraftingTool : Interaction
{
    private bool hasCheckedRecipes;

    // Start is called before the first frame update
    void Start()
    {
        
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


}
