using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundAnimation : MonoBehaviour
{
    private Animator anim;
    private float poss;
    private bool turn;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        turn = IsTurned();
        if (turn == true)
        {
            anim.gameObject.GetComponent<girlMovement>().speed = 0f;
            anim.SetBool("isTurning", true);
            turn = false;
        }
    }

    private bool IsTurned()
    {
        Debug.Log("oops!");
        poss = Random.Range(0, 1f);
        if (poss < 0.1f)
        {
            return true;
        }
        return false;
    }
}
