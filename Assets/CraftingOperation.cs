using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingOperation : MonoBehaviour
{
    public string displayName;
    [SerializeField] private GameObject OperationPreview;
    [SerializeField] private GameObject OperationUI;
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
}
