using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingModeManager : MonoBehaviour
{
    //singleton
    private static CraftingModeManager _instance;
    public static CraftingModeManager Instance { get { return _instance; } }


    private List<Dictionary<string, object>> recipeData;


    private static float desiredTimeDialation = 1;
    private static float currentTimeDialation = 1;
    //private actual time value (needs a get value for other func to access

    public UI_Lab uiRef;//TODO: ue this

    private static bool isPaused = false;
    public static void pauseToggle()
    {
        if (isPaused)
            isPaused = false;
        else
            isPaused = true;
    }
    public static void pauseSet(bool willPause) { isPaused = willPause; }
    private static void pause() { isPaused = true; } //backup time value, and et actual to 0
    private static void unpause() { isPaused = false; } //restore time value
    public static float getTimeDialation() { return currentTimeDialation; }
    public static void setTimeDialation(float desiredTime) { desiredTimeDialation = desiredTime; }

    private void Awake()
    {
        //singleton init
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        //load all recipeData
        recipeData = CSVReader.Read("recipes");
        Debug.Log(recipeData[1]["toolType"]);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 9"))
        {
            if (!isPaused)
            {
                pause();
                uiRef.menuPause.SetActive(true);
            }
            else
            {
                unpause();
                uiRef.menuPause.SetActive(false);

            }
        }


        if (isPaused)
        {
            currentTimeDialation = 0;
            if (Input.GetKeyDown("joystick button 0"))
                clearNextClutter();
            if (Input.GetKeyDown("joystick button 3"))
                quit();
        }
        else
            currentTimeDialation = desiredTimeDialation;
    }

    public void quit()
    {
        Application.Quit();
    }

    [SerializeField]
    private List<Clutter> clutterList;

    public void clearNextClutter()
    {
        clutterList[0].clear();
        clutterList.RemoveAt(0);
    }
}