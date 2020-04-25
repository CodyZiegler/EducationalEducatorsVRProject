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
    private List<GameObject> tempBlock = new List<GameObject>();
    public GameObject placement;
    public int numOfBlocks, timer;

    private InputBridge input;
    private List<Vector3> vecs;
    private List<Vector3> tempVecs = new List<Vector3>();

    private AudioSource audioPlayer;
    public AudioClip rightSound, wrongSound;

    

    private bool done = true;
    private PlayerPackage player;

    //Kennedy Additions
    public TMP_Text timerText;
    private float startTime;
    public int CountdownTime;
    public TMP_Text countdownDisplay;
    public TMP_Text HowToPlay;
    //public static BlockHandler instance;
    private DataRecorder _dataRecorder;

    void Start()
    {
        input = GetComponent<InputBridge>();
        _dataRecorder = FindObjectOfType<DataRecorder>();
        vecs = new List<Vector3> {new Vector3(.5f, 1f, 0f), new Vector3(.5f, 1f, .2f), new Vector3(.5f, 1f, -.2f), new Vector3(-.5f, 1f, 0f),
                                   new Vector3(-.5f, 1f, .2f), new Vector3(-.5f, 1f, -.2f), new Vector3(.5f, 1.2f, .2f), new Vector3(-.5f, 1.2f, .2f),
                                    new Vector3(.5f, 1.2f, 0f), new Vector3(-.5f, 1.2f, 0f)};
        startTime = Time.time;
        audioPlayer = gameObject.GetComponent<AudioSource>();
        //timerText = GetComponent<TMP_Text>();
        StartCoroutine(CountdownToStart());
    }
    /*private void Awake()
    {
        instance = this;
    }*/

    private void Update()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }
    private void FixedUpdate()
    {
        if (input.RightThumbstickDown)
        {
            checkBlocks();
            //isdone();
        }
        //if (!done)
        //{
        //    return;
        //}
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
        tempBlock.Clear();
        Invoke("BlockFall", timer);
    }

    void BlockFall() // This will move the blocks after a set amount of time.
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < numOfBlocks; i++)
        {
            int rand = Random.Range(0, (tempVecs.Count-1));
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
        if (numOfBlocks == 10) {
            FinishPlacements();
        }
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
    IEnumerator CountdownToStart()
    {
        while (CountdownTime >= 0)
        {
            countdownDisplay.text = CountdownTime.ToString();
            yield return new WaitForSeconds(1f);
            CountdownTime--;
        }
        countdownDisplay.text = "START!";
        //BlockHandler.instance.GenerateBlocks();
        _dataRecorder.StartRecording();
        populateTempBlock();
        GenerateBlocks();
        yield return new WaitForSeconds(1f);
        countdownDisplay.enabled = false;
        HowToPlay.enabled = false;
    }
    void FinishPlacements()
    {
        audioPlayer.clip = wrongSound;
        audioPlayer.Play();
        _dataRecorder.StopRecording("Puzzle", numOfBlocks);
        GameObject.FindObjectOfType<PlayerPackage>().LoadNextScene();
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

    public void checkBlocks()
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
        if (foundBlocks.Length == 0) { return; }
        else if (blocksOK)
        {
            StartCoroutine(CorrectPlacement());
        }
        else
        {
            FinishPlacements();
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
    }
    public void isdone()
    {
        done = true;
        timerText.color = Color.blue;
    }*/

    private void spawnBlock(float x, float z) {
        int rand = Random.Range(0, (tempBlock.Count-1));
        Instantiate(placement, new Vector3((x * 1.2f), 1f, z), placement.transform.rotation);
        Instantiate(tempBlock[rand], new Vector3(x * 1.2f, 1f, z + .55f), Quaternion.identity);
        tempBlock.RemoveAt(rand);
    }
}
