using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPosition : MonoBehaviour
{
    Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = this.transform.position;
    }

    public bool checkPosition()
    {
        Vector3 currentPos = this.transform.position;
        if ((currentPos.x >= originalPos.x - .05 && currentPos.x <= originalPos.x + .05) && (currentPos.y >= (originalPos.y-.15) - .07 && currentPos.y <= (originalPos.y-.15) + .07) &&
            (currentPos.z >= originalPos.z - .05 && currentPos.z <= originalPos.z + .05))
        {
            return true;
        }
        else
            return false;
    }
}
