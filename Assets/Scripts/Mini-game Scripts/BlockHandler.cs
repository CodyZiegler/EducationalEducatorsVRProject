using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    public List<GameObject> block = new List<GameObject>();
    public GameObject placement;
    public int numOfBlocks, timer;

    private List<Vector3> vecs;

    void Start()
    {
        GenerateBlocks();
        vecs = new List<Vector3> {new Vector3(.5f, 1f, 0f), new Vector3(.5f, 1f, .2f), new Vector3(.5f, 1f, -.2f), new Vector3(-.5f, 1f, 0f),
                                   new Vector3(-.5f, 1f, .2f), new Vector3(-.5f, 1f, -.2f), new Vector3(.2f, 1f, -.5f), new Vector3(-.2f, 1f, -.5f)};
    }


    void GenerateBlocks() // This will create all the blocks
    {
        int xInc = numOfBlocks / 2;
        float x = -xInc * .2f;
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, block.Count);
            Instantiate(placement, new Vector3(x, 1f, 0.55f), placement.transform.rotation);
            Instantiate(block[rand], new Vector3(x, 1f, 0.5f), Quaternion.identity);
            block.RemoveAt(rand);
            x += .2f;
        }

        Invoke("BlockFall", timer);
    }

    void BlockFall() // This will move the blocks after a set amount of time.
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < numOfBlocks; i++) {
            int rand = Random.Range(0, vecs.Count);
            blocks[i].transform.position = vecs[rand];
            vecs.RemoveAt(rand);
        }
    }

    void blockChecker() {
        bool correct = false, incorrect = false;
        // This will check to make sure blocks are in the correct place
        if (correct)
        {
            nextBlockLevel();
        }
        else if (incorrect) {
            // End mini-game and record results into a text file for review later
        }
    }

    void nextBlockLevel() {
        numOfBlocks++;
        GenerateBlocks();
    }
}
