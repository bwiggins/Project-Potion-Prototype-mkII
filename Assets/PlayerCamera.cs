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

        originalTrans = transform;
    }

    private Transform originalTrans;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTarget(GameObject go)
    {

        /*if (go == null)
        {
            Debug.Log("reset camera");
            transform.position = originalTrans.position;
            Quaternion o = originalTrans.localRotation;
            Quaternion c =transform.localRotation;
            transform.localRotation = new Quaternion(o.x-c.x,o.y-c.y,o.z-c.z,o.w-c.w);//Quaternion.identity + originalTrans.rotation;
            //Debug.Log(originalTrans.rotation);
            transform.localScale = originalTrans.localScale;
        }
        else
        {
            transform.LookAt(go.transform);
            //transform.Translate(Vector3.forward * 10);
        }*/

    }
}
