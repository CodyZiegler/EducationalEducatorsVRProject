using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class BlockHandler : MonoBehaviour
{
    public List<GameObject> block = new List<GameObject>();
    public List<GameObject> tempBlock = new List<GameObject>();
    public GameObject placement;
    public int numOfBlocks, timer;

    private InputBridge input;
    private List<Vector3> vecs;
    private List<Vector3> tempVecs = new List<Vector3>();

    private AudioSource audioPlayer;
    public AudioClip rightSound, wrongSound;

    public TMP_Text timerText;
    private float startTime;

    private bool done = true;
    private PlayerPackage player;

    void Start()
    {
        input = GetComponent<InputBridge>();
        vecs = new List<Vector3> {new Vector3(.5f, 1f, 0f), new Vector3(.5f, 1f, .2f), new Vector3(.5f, 1f, -.2f), new Vector3(-.5f, 1f, 0f),
                                   new Vector3(-.5f, 1f, .2f), new Vector3(-.5f, 1f, -.2f), new Vector3(.2f, 1f, -.5f), new Vector3(-.2f, 1f, -.5f),
                                    new Vector3(.5f, 1.2f, 0f), new Vector3(-.5f, 1.2f, 0f)};
        populateTempBlock();
        GenerateBlocks();
        startTime = Time.time;
        audioPlayer = gameObject.GetComponent<AudioSource>();
        //timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (input.RightThumbstickDown)
        {
            checkBlocks();
            isdone();

        }
        if (!done)
        {
            return;
        }
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }


    void GenerateBlocks() // This will create all the blocks
    {
        for (int i = 0; i < numOfBlocks; i++) {
            switch (i) {
                case 0: spawnBlock(0f, 0f); break;
                case 1: spawnBlock(.2f, 0f); break;
                case 2: spawnBlock(-.2f, 0f); break;
                case 3: spawnBlock(.4f, 0f); break;
                case 4: spawnBlock(-.4f, 0f); break;
                case 5: spawnBlock(0f, .24f); break;
                case 6: spawnBlock(.2f, .24f); break;
                case 7: spawnBlock(-.2f, .24f); break;
                case 8: spawnBlock(.4f, .24f); break;
                case 9: spawnBlock(-.4f, .24f); break;
            }
        }

        /*int xInc = numOfBlocks / 2;
        float x = -xInc * .2f;
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, tempBlock.Count);
            Instantiate(placement, new Vector3((x * 1.2f), 1f, 0), placement.transform.rotation);
            Instantiate(tempBlock[rand], new Vector3(x, 1f, 0.5f), Quaternion.identity);
            tempBlock.RemoveAt(rand);
            x += .2f;
        }*/
        tempBlock.Clear();
        Invoke("BlockFall", timer);
    }

    void BlockFall() // This will move the blocks after a set amount of time.
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, tempVecs.Count);
            blocks[i].transform.position = tempVecs[rand];
            tempVecs.RemoveAt(rand);
        }
        tempVecs.Clear();
    }

    void populateTempBlock()
    {
        foreach (GameObject b in block)
        {
            tempBlock.Add(b);
        }
        foreach (Vector3 v in vecs)
        {
            tempVecs.Add(v);
        }
    }

    IEnumerator CorrectPlacement()
    {
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        ParticleSystem p;
        foreach (GameObject b in allBlocks)
        {
            if (b.transform.GetChild(0).TryGetComponent<ParticleSystem>(out p))
            {
                p.Play();
            }
        }
        audioPlayer.clip = rightSound;
        audioPlayer.Play();

        yield return new WaitForSeconds(2.0f);

        populateTempBlock();
        ClearBlocks();
        numOfBlocks++;
        GenerateBlocks();
    }

    void WrongPlacements()
    {
        audioPlayer.clip = wrongSound;
        audioPlayer.Play();

        // This will need to output to a file both the time and amount of levels correct.
    }

    void ClearBlocks()
    {
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] allPlacements = GameObject.FindGameObjectsWithTag("Placement");
        foreach (GameObject b in allBlocks)
        {
            Destroy(b);
        }
        foreach (GameObject p in allPlacements)
        {
            Destroy(p);
        }
    }

    void checkBlocks()
    {
        GameObject[] foundBlocks = GameObject.FindGameObjectsWithTag("Block");
        bool blocksOK = true;
        foreach (GameObject fb in foundBlocks)
        {
            if (!fb.GetComponent<BlockPosition>().checkPosition())
            {
                blocksOK = false;
            }
        }
        if (blocksOK)
        {
            StartCoroutine(CorrectPlacement());
        }
        else
        {
            WrongPlacements();
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        int xInc = numOfBlocks / 2;
        float x = -xInc * .2f;
        if (other.gameObject.CompareTag("Placement"))
        {
            //Snap it into position
            transform.position = new Vector3((transform.position.x - .4f),(transform.position.y-.4f),(transform.position.z-.4f));
            transform.eulerAngles = Vector3.zero;
        }
        // Only continue if it's in a block trigger zone & not grabbed
        else
        {
            return;
        }
        //Check if it's the right pedestal

    }*/
    public void isdone()
    {
        done = true;
        timerText.color = Color.blue;
    }

    private void spawnBlock(float x, float z) {
        int rand = Random.Range(0, tempBlock.Count);
        Instantiate(placement, new Vector3((x * 1.2f), 1f, z), placement.transform.rotation);
        Instantiate(tempBlock[rand], new Vector3(x * 1.2f, 1f, z + .55f), Quaternion.identity);
        tempBlock.RemoveAt(rand);
    }
}
