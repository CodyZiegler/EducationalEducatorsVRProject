using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class SubmitHandle : MonoBehaviour
{
    private bool theyGotMe = true;
    private Grabbable grabbable;
    private BlockHandler bh;
    private Vector3 originSpot;
    private Quaternion originRot;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = gameObject.GetComponent<Grabbable>();
        bh = GameObject.FindObjectOfType<BlockHandler>();
        originSpot = this.gameObject.transform.position;
        originRot = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.BeingHeld == true && theyGotMe == true)
        {
            Debug.Log("Im grabbed");
            bh.checkBlocks();
            theyGotMe = false;
        }
        else if(grabbable.BeingHeld==false)
        {
            this.gameObject.transform.position = originSpot;
            this.gameObject.transform.rotation = originRot;

            theyGotMe = true;
        }
    }
}
