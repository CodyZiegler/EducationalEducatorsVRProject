using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class BlockHandler : MonoBehaviour
{
    public List<GameObject> block = new List<GameObject>();
    public GameObject placement;
    public int numOfBlocks, timer;

    private InputBridge input;
    private List<Vector3> vecs;

    void Start()
    {
        input = GetComponent<InputBridge>();
        GenerateBlocks();
        vecs = new List<Vector3> {new Vector3(.5f, 1f, 0f), new Vector3(.5f, 1f, .2f), new Vector3(.5f, 1f, -.2f), new Vector3(-.5f, 1f, 0f),
                                   new Vector3(-.5f, 1f, .2f), new Vector3(-.5f, 1f, -.2f), new Vector3(.2f, 1f, -.5f), new Vector3(-.2f, 1f, -.5f)};
    }

    private void Update()
    {
        if (input.RightThumbstickDown) {
            checkBlocks();
        }
    }


    void GenerateBlocks() // This will create all the blocks
    {
        int xInc = numOfBlocks / 2;
        float x = -xInc * .2f;
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, block.Count);
            Instantiate(placement, new Vector3(x, 1f, 0), placement.transform.rotation);
            Instantiate(block[rand], new Vector3(x, 1f, 0.5f), Quaternion.identity);
            block.RemoveAt(rand);
            x += .2f;
        }

        Invoke("BlockFall", timer);
    }

    void BlockFall() // This will move the blocks after a set amount of time.
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, vecs.Count);
            blocks[i].transform.position = vecs[rand];
            vecs.RemoveAt(rand);
        }
    }

    void CorrectPlacement() {
        ClearBlocks();
        numOfBlocks++;
        GenerateBlocks();
    }

    void WrongPlacements() {
        // This will need to output to a file both the time and amount of levels correct.
    }

    void ClearBlocks() {
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] allPlacements = GameObject.FindGameObjectsWithTag("Placement");
        foreach (GameObject b in allBlocks) {
            Destroy(b);
        }
        foreach (GameObject p in allPlacements) {
            Destroy(p);
        }
    }

    void checkBlocks() {
        GameObject[] foundBlocks = GameObject.FindGameObjectsWithTag("Block");
        bool blocksOK = true;
        foreach (GameObject fb in foundBlocks) {
            if (!fb.GetComponent<BlockPosition>().checkPosition()) {
                blocksOK = false;
            }
        }
        if (blocksOK)
        {
            CorrectPlacement();
        }
        else {
            WrongPlacements();
        }
    }
}
