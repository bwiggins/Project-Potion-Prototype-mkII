using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> disabled;

    private void Awake()
    {
        //the list of disabled objects, is disabled at start
        setDisabledActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setDisabledActive(bool isEnabled)
    {
        foreach (GameObject g in disabled)
            g.SetActive(isEnabled);
    }

    public void clear()
    {
        setDisabledActive(true);
        this.gameObject.SetActive(false);
    }
}
