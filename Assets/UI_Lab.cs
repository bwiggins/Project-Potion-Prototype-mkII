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

    [Header("Buttons")]
    [SerializeField] private string accept;
    [SerializeField] private string decline;
    [SerializeField] private string pause;
    [SerializeField] private string notes;
    [SerializeField] private string engage_dialog;
    [SerializeField] private string engage_tool;

    [Header("UI Element Refs")]
    public GameObject startingPopUp;

    public GameObject screenPopUp;

    //a ref to the object that is prompting
    //so that I can call activate on it, from a button press func in this
    



    public GameObject menuPause;

    // Start is called before the first frame update
    void Start()
    {
        if (startingPopUp != null)
            showScreen(startingPopUp);

    }

    // Update is called once per frame
    void Update()
    {

  
    }

    public void showScreen(string t)
    {
        Text text = screenPopUp.GetComponentInChildren<Text>();
        if (text)
            text.text = t;

        screenPopUp.SetActive(true);
    }

    public void showScreen(GameObject go)
    {
        screenPopUp = go;
        screenPopUp.SetActive(true);
    }

    public void hideScreen()
    {
        screenPopUp.SetActive(false);
    }
}
