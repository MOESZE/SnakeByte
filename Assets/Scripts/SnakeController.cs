using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;


// basically the whole game controller that controls the movement and the UI elements as well as the trigger collision detection and sounds
public class SnakeController : MonoBehaviour
{
    public static Vector3 dir = Vector3.forward;
    public float updateAdjustment = 0.05f;

    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;

    public int initialSize = 5;

    private bool[] wrongDirectionFlag = new bool[4];
    private bool gameOn = true;

    private int count = 0;
    public Text countText;
    public Text gameOver;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        //reset state
        for (int i = 1; i < segments.Count; i++)    // making sure that at the start the list of segments is reset with objects destroyed and cleared.
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            segments.Add(Instantiate(this.segmentPrefab));  // instanciating 4 segments in order to meet the initial size requirments
        }
        //reset state
        
        Time.fixedDeltaTime = updateAdjustment;     // setting the fixedupdate frame time update in order to increase or decrease speed of the snake movement potentially
        dir = Vector3.forward;

        restartButton.gameObject.SetActive(false);      // standard UI with restart button hidden and initilization of the text items
        count = 0;
        countText.text = "Count: " + count.ToString();
        gameOver.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOn)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !wrongDirectionFlag[1])    // script to control the direction the snake is going in
            {
                dir = Vector3.forward;
                wrongDirectionFlag[0] = true;
                wrongDirectionFlag[1] = false;
                wrongDirectionFlag[2] = false;
                wrongDirectionFlag[3] = false;
            }
            if (Input.GetKey(KeyCode.DownArrow) && !wrongDirectionFlag[0])
            {
                dir = Vector3.back;
                wrongDirectionFlag[0] = false;
                wrongDirectionFlag[1] = true;
                wrongDirectionFlag[2] = false;
                wrongDirectionFlag[3] = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !wrongDirectionFlag[3])
            {
                dir = Vector3.left;
                wrongDirectionFlag[0] = false;
                wrongDirectionFlag[1] = false;
                wrongDirectionFlag[2] = true;
                wrongDirectionFlag[3] = false;
            }
            if (Input.GetKey(KeyCode.RightArrow) && !wrongDirectionFlag[2])
            {
                dir = Vector3.right;
                wrongDirectionFlag[0] = false;
                wrongDirectionFlag[1] = false;
                wrongDirectionFlag[2] = false;
                wrongDirectionFlag[3] = true;
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)    // script for controlling the position of the snake and its segments following
        {
            segments[i].position = segments[i - 1].position;
        }
        if (gameOn)
        {
            this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x, 1f, Mathf.Round(this.transform.position.z) + dir.z);
        } else
        {
            dir = Vector3.zero;
            this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
        }
    }


    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);    // script to grow the snake after it encounters an apple
        segment.position = segments[segments.Count - 1].position;

        segments.Add(segment);
    }

    private void OnTriggerEnter(Collider other)     // script handling all triggers by tag and updating size/UI items
    {
        if (other.tag == "Apple")
        {
            AudioSource[] sounds = GetComponents<AudioSource>();
            sounds[1].Play();
            Grow();
            Grow();
            Grow();
            count += 1;
            countText.text = "Count: " + count.ToString();
        } else if (other.tag == "Wall")
        {
            AudioSource[] sounds = GetComponents<AudioSource>();
            sounds[0].Play();
            gameOn = false;
            gameOver.text = "Game Over!";
            restartButton.gameObject.SetActive(true);
        }
    }

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SnakeByte"); //restart game
    }
}
