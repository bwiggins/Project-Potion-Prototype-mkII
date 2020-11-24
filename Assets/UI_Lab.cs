using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TOOD: merge this into crafting mode manager... since a lot of it is controlling game behaviors, rather than just UI elements...

public class UI_Lab : MonoBehaviour
{
    //singleton pattern
    private static UI_Lab _instance;
    public static UI_Lab Instance { get { return _instance;  } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    [Header("Action buttons")]
    [SerializeField] private string keyTool = "e";
    [SerializeField] private string keyDialog = "space";

    [Header("UI Element Refs")]
    public GameObject promptTool;
    public GameObject promptDialog;

    [Space(10)]
    private Interaction actionDialog;
    private CraftingTool actionTool;

    public string promptName = "Alert";

    //a ref to the object that is prompting
    //so that I can call activate on it, from a button press func in this
    

    private Text textref;



    public GameObject menuPause;

    // Start is called before the first frame update
    void Start()
    {
        textref = promptDialog.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textref.text = promptName;

        //activate
        /*if (promptDialog.activeSelf && Input.GetKeyDown(keyDialog))
        {
            promptPress(actionDialog);
        }
        //activate
        else if (promptTool.activeSelf && Input.GetKeyDown(keyTool))
        {
            promptPress(actionTool);
        }*/

        if (actionDialog)
            promptDialog.SetActive(true);
        else
            promptDialog.SetActive(false);
           
        if (actionTool)
            promptTool.SetActive(true);
        else
            promptTool.SetActive(false);

        //if there is a valid possible tool action AND the relevant button is pressed
        if(Input.GetKeyDown(keyTool) && actionTool)
        {
            //placeholder testing csv data


        }
    }

    /*
    public void prompt(Interaction i)
    {
        inter = i;
        showPromptDialog = true;
        promptName = inter.promptText;
        
    }*/

    //TODO: what a mess that the ui controller has to talk between the action and the player to get them inventory options. THE UI SHOULD ONLY BE REACTING TO THESE THINGS, NOT CONTROLLING THEM
    /*public void promptPress(Interaction inter)
    {
        Inventory invRef = Player.Instance.gameObject.GetComponent<Inventory>();

        actionChosen = inter;
        string result = actionChosen.activate();
        if(result != null)
            invRef.Items.Add(result);
    }*/

    public void setActionTool(CraftingTool tool)
    {
        actionTool = tool;
        if(tool)//tool is often set to null
            promptTool.GetComponentInChildren<Text>().text = tool.promptTitle;
    }

    public void setActionDialog(Interaction dialog)
    {
        actionDialog = dialog;
        if(dialog)
            promptDialog.GetComponentInChildren<Text>().text = dialog.promptTitle;
    }
}
