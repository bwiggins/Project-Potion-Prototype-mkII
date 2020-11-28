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

      
    }
}
