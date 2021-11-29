using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_script : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform _LymeFloat;
    public Transform _LymeGround;
    private CinemachineVirtualCamera _camera = new CinemachineVirtualCamera();

    // function that switch from ground to float

    public void changetoground()
    {
        _camera.Follow = _LymeGround;
        _camera.LookAt = _LymeGround;
    }
    
    public void changestate(Transform CurrentState)
    {
        // if current state is floating, change it to ground
        if (CurrentState == _LymeFloat)
        {
            _camera.Follow = _LymeGround;
            _camera.LookAt = _LymeGround;
        }
        else // if current state is ground, change it to floating
        {
            _camera.Follow = _LymeFloat;
            _camera.LookAt = _LymeFloat;
        }
    }
    // function that switch from float to ground
    void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }
}
