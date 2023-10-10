using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mohammed ALHasani, 12/05/2021 script that handles the positioning of the apple at the start and when it is triggered by the snake
public class AppleController : MonoBehaviour
{
    public BoxCollider AppleBoundary;
    // Start is called before the first frame update
    void Start()
    {
        RandomizePosition();    // start at a random position
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RandomizePosition()    // function to call everytime the apple gets 'eaten' to randomize position
    {
        Bounds bounds = this.AppleBoundary.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        this.transform.position = new Vector3(Mathf.Round(x), 1f, Mathf.Round(z));
    }

    private void OnTriggerEnter(Collider other)     // script handling the detection of the collision in order to rerandomize position
    {
        if (other.tag == "Snake")
        {
            RandomizePosition();
        }
    }
}
