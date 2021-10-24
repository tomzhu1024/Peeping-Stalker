using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gitlMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private float girlStep = manager.step;
    private float dist; //distance to follower
    public float threshold = 9f; //safe distance

    private float susp;
    private CharacterController _controller;
    private Vector3 movedirection;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        //read distance 
        dist = manager.dist;
        // Debug.Log("girl:"+dist)
        if (dist <= threshold)
        {
            Debug.Log("getting close: " + dist);
            susp = manager.susp;
            if (susp < 1f)
            {
                susp += 0.01f;
                manager.susp = susp <= 1f ? susp : 1f;
            }

        }
        else
        {
            susp = susp > 0f ? susp - 0.005f : susp; //if not within threshold, decrease susp by some extent
            manager.susp = susp;
        }

    }

    //girl will keep walking
    private void move()
    {
        movedirection = new Vector3(0, 0, 1);
        movedirection *= speed;
        _controller.Move(movedirection * Time.deltaTime);
    }
}
