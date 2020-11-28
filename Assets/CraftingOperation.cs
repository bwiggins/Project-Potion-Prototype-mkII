using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingOperation : MonoBehaviour
{
    [SerializeField]
    private string displayName;

    [SerializeField]
    private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setState(bool isActive)
    {
        if (UI)
            UI.SetActive(isActive);
    }
}
