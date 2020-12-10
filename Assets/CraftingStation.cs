using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public string displayName;
    public string toolType;
    [SerializeField] protected GameObject OperationPreview;
    [SerializeField] protected GameObject OperationUI;
    public bool isEnterable;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPreviewState(bool isActive)
    {
        if (OperationPreview) OperationPreview.SetActive(isActive);

        if (OperationUI && isActive) OperationUI.SetActive(false);
    }

    public void setState(bool isActive)
    {
        if (OperationUI) OperationUI.SetActive(isActive);

        if (OperationPreview && isActive) OperationUI.SetActive(false);
    }

    public virtual void activate()
    {

    }
}


