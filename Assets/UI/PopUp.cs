using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    private bool _completed = false;
    public bool isCompleted { get { return _completed; } private set { _completed = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _completed = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkStatus();
    }

    protected virtual void checkStatus()
    {
        if(Input.GetKeyDown("q"))
        { 
            isCompleted = true;
            gameObject.SetActive(false);
        }
    }
}
