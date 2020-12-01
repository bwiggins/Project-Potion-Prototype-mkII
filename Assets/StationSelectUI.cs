using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationSelectUI : MonoBehaviour
{
    [SerializeField]
    private Text tName;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setName(string name)
    {
        tName.text = name;
    }
}
