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
        menu.SetActive(showCrafting);
    }

    /*
    public void displayPrompt()
    {
        //is prereq available

        if (!isFinished || endDialog != "")
        {
            //display id thing
            UI_Lab.Instance.prompt(this);
        }
    }*/

    /// <summary>
    /// interact
    /// </summary>
    /// <returns>returns a reward if given, otherwise empty string</returns>
    public virtual string activate()
    {
        isActive = true;


        PlayerCamera.Instance.setTarget(this.gameObject);
        //PlayerCamera.Instance.setTarget(null);
        showCrafting = true;

        //checks if we've made it through all dialog
        if (currentDialogID >= dialog.Count)
            isFinished = true;

        UnityEngine.Debug.Log(currentDialogID + " " + dialog.Count);

        if (isFinished)
        {
            deactivate();

            return itemReward;           
        }
        else
        {
            //display ID dialog
            UI_Lab.Instance.showScreen(dialog[currentDialogID]);

            //set up the next screen to show
            currentDialogID++;
            return "";
        }
    }

    public virtual void deactivate()
    {
        isActive = false;


        //testing, but remove these later
        currentDialogID = 0;
        isFinished = false;


        UI_Lab.Instance.hideScreen();
        PlayerCamera.Instance.setTarget(null);
    } 

    public virtual void setReady(bool readyState)
    {
        isReady = readyState;

        if (isReady)
        {
            UI_Lab.Instance.setActionDialog(this);
        }
    }
}
