using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnAroundAnimation : MonoBehaviour
{
    [Tooltip("Minimal interval between turn-around.")]
    public float minInterval = 5;

    [Tooltip("Maximal interval between turn-around.")]
    public float maxInterval = 10;

    [Tooltip("The manager script.")]
    public manager manager;

    [Tooltip("The maximal angle to detect.")]
    public float detectAngle = 45;

    [Tooltip("The maximal distance to detect.")]
    public float detectDistance = 10;
    
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
            // If detected, fail the game
            if (manager.GetAngle() < detectAngle && manager.GetDistance() < detectDistance)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void TurnAround()
    {
        anim.gameObject.GetComponent<girlMovement>().speed = 0f;
        anim.SetBool("isTurning", true);
        turn = false;
    }
}
