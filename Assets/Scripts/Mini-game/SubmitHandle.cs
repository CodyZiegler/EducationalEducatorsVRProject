using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class SubmitHandle : MonoBehaviour
{
    private bool theyGotMe = true;
    private Grabbable grabbable;
    private BlockHandler bh;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = gameObject.GetComponent<Grabbable>();
        bh = GameObject.FindObjectOfType<BlockHandler>();
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
            theyGotMe = true;
        }
    }
}
