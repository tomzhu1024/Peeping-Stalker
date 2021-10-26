using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
	[Tooltip("The minimal distance to fail the game.")]
	public static float minimalDist = 4f; //directly fail

	[Tooltip("The maximal distance to fail the game.")]
	public float maximalDist = 20f;
	
	public static float step = 3f; //share between player and girl
	public static float dist;
	public Transform player;
	public Transform girl;
	public static float susp = 0f;//girl's suspiciousness; 
    public AudioSource street; //sound effect of street

    void Start(){
        playSound();
    }


    // Update is called once per frame
    void Update()
    {
        getPlayerDistance();
        // Debug.Log(dist+" while minimal is: "+minimalDist);
        if (dist < minimalDist || dist > maximalDist)
        {
            Debug.Log(dist<minimalDist);
	        // SceneManager.LoadScene("GameOver");
        }
    }

    //monitor distance of player to the girl
    void getPlayerDistance(){
    	dist = Vector3.Distance(player.position,girl.position);
    }

    public float GetAngle()
    {	
	    var angle = Mathf.Asin((player.position.x - girl.position.x) / (player.position.z - girl.position.z));
	    angle = Mathf.Abs(angle);
	    angle *= Mathf.Rad2Deg;
	    return angle;
    }

    public float GetDistance()
    {
	    return Vector3.Distance(player.position, girl.position);
    }

    public void playSound(){
        street.Play();
    }
}
