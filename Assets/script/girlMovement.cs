using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class girlMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    public float girlStep = manager.step;
    private float dist; //distance to follower
    public float threshold = 10f; //safe distance, not causing susp to grow

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
