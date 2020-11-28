using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("UI Ref")]
    [SerializeField] private GameObject menuPause;

    [Header("Buttons")]
    [SerializeField] private string keyPause = "escape";

    [Header("Pause")]
    public bool isPaused = false;
    public bool togglePause()
    {
        /*
         * if (Input.GetKeyDown("joystick button 3"))
                quit();
        */

        if (isPaused)//unpause
        {
            Time.timeScale = 1;
            isPaused = false;
            menuPause.SetActive(false);
        }
        else//pause
        {
            Time.timeScale = 0;
            isPaused = true;
            menuPause.SetActive(true);
        }

        return isPaused;
    }

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
        menuPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPause))
            togglePause();
    }
}
