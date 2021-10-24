using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
	public static float step = 3f; //share between player and girl
	public static float dist;
	public Transform player;
	public Transform girl;
	public static float susp = 0f;//girl's suspiciousness; 

    // Update is called once per frame
    void Update()
    {
        getPlayerDistance();
    }

    //monitor distance of player to the girl
    void getPlayerDistance(){
    	dist = Vector3.Distance(player.position,girl.position);
    }
}
