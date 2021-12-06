using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
	public Transform player;
	public float zoffset = 3f;
	public float yoffset = 1.5f;
	
	public float speed = 1f; 
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
    	float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(
        	transform.position,
        	new Vector3(player.position.x,player.position.y+yoffset,player.position.z+zoffset),
        	step

        	);
    }
}
