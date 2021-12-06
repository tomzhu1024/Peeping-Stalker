using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotating : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed = 72;
    
    private float _deg = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 180, _deg);
        _deg += rotatingSpeed * Time.deltaTime;
    }
}
