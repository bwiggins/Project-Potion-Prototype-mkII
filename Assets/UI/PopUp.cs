﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            gameObject.SetActive(false);
        }
    }
}
