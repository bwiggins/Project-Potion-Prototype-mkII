using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //singleton
    private static PlayerCamera _instance;
    public static PlayerCamera Instance { get { return _instance; } }

    private void Awake()
    {
        //singleton init
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private Transform originalTrans;
    // Start is called before the first frame update
    void Start()
    {
        originalTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTarget(GameObject go)
    {

        if (go == null)
        {
            transform.position = originalTrans.position;
            transform.rotation = originalTrans.rotation;
            transform.localScale = originalTrans.localScale;
        }
        else
        {
            transform.LookAt(go.transform);
            transform.Translate(Vector3.forward * 10);
        }

    }
}
