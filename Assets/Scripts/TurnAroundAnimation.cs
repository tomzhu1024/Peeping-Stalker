using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundAnimation : MonoBehaviour
{
    [Tooltip("Minimal interval between turn-around.")]
    public float minInterval = 5;

    [Tooltip("Maximal interval between turn-around.")]
    public float maxInterval = 10;
    
    private Animator anim;
    private float poss;
    private bool turn;
    private float nextTurn;

    void Start()
    {
        anim = GetComponent<Animator>();
        nextTurn = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        if (Time.time >= nextTurn)
        {
            // Refresh `nextTurn`
            nextTurn = Time.time + Random.Range(minInterval, maxInterval);
            // Turn around
            TurnAround();
            Debug.Log("[her] turn around");
        }
    }

    private void TurnAround()
    {
        anim.gameObject.GetComponent<girlMovement>().speed = 0f;
        anim.SetBool("isTurning", true);
        turn = false;
    }
}
