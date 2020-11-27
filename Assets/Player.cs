using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //singleton
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    private Interaction actionChosen = null;
    private Interaction actionDialog;
    private CraftingTool actionTool;

    //private bool isInsideTrigger = false;
    [Header("Action buttons")]
    [SerializeField] private string keyTool = "e";
    [SerializeField] private string keyDialog = "space";

    public Animator anim_ref;

    [SerializeField]
    private GameObject body;

    public CharacterController controller;

    public float speed = 10f;

    public List<string> inventory;

    private void Awake()
    {
        //singleton init
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //turn off context when inside crafting
        //turn off movement when inside crafting
        //start caemra move

        /*if (isCrafting)
            UI_ref.showContextAlert = false;
        else
            UI_ref.showContextAlert = isInsideTrigger;
        UI_ref.showCrafting = isCrafting;*/

        //cant move if locked into an action
        if (actionChosen == null)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 direction = new Vector3(move.x, 0, move.z) + body.transform.position;
            body.transform.LookAt(direction);
            controller.Move(move * speed * CraftingModeManager.getTimeDialation() * Time.deltaTime);

            anim_ref.SetFloat("walking", move.magnitude);
        }

        //toggle crafting
        /*if (isInsideTrigger && Input.GetKeyDown("space"))
        {
            if (currentInteraction != null)
            {
                currentInteraction = null;
                CraftingModeManager.pauseSet(false);
            }
            else
            {
                //isCrafting = true;
                CraftingModeManager.pauseSet(true);
            }
        }*/

        //turn off if this is empty
        //TODO: also control turning on if not
        if (actionDialog == null)
            UI_Lab.Instance.setActionDialog(null);
        else if (Input.GetKeyDown(keyDialog))
            actionDialog.activate();

        //turn off if this is empty
        //TODO: also control turning on if not
        if (actionTool == null)
            UI_Lab.Instance.setActionTool(null);
        else if (Input.GetKeyDown(keyTool))
            actionTool.activate();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        Interaction interaction = other.GetComponent<Interaction>();
        if (interaction)//has any sort of interaction component
        {
            interaction.setReady(true);

            CraftingTool craftInteraction = other.GetComponent<CraftingTool>();
            if (craftInteraction)//if it specifically is a craftingTool Component
            {
                //UI_Lab.Instance.setActionDialog(element);
                actionTool = craftInteraction;
            }
            else//regular old interactive
                actionDialog = interaction;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        //UI_Lab.Instance.setActionDialog(null);
        //currentInteraction = null;

        //checks if the trigger being left, belonged to any of the interactions that the player was expecting to communicate with.

        //if it has an interactive component
        Interaction interaction = other.GetComponent<Interaction>();
        if (interaction)
        {
            interaction.setReady(false);

            if (interaction == actionDialog)
                actionDialog = null;

            CraftingTool craftInteraction = other.GetComponent<CraftingTool>();
            if (craftInteraction == actionTool)
                actionTool = null;
        }
    }
}
