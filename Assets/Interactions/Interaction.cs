using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    /// <summary>
    /// how the prompt button appears
    /// </summary>
    public string promptTitle;

    protected bool _isReady = false;
    public bool isReady { get { return _isReady; } protected set { _isReady = value; } }

    protected bool _isActive = false;
    public bool isActive { get { return _isActive; } protected set { _isActive = value; } }

    public List<string> preRequisite;

    //for tracking where in the dialog it is currently
    private int currentDialogID = 0;
    private bool isFinished = false;

    public List<string> dialog;
    public string endDialog;
    public string itemReward;

    public GameObject menu;
    public bool showCrafting = false;

    protected void Awake()
    {
        menu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
