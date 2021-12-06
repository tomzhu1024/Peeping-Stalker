using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MixingCameraControl : MonoBehaviour
{
    [SerializeField] private AnimationSequence animationSequence;
    [SerializeField] private float transitionDuration = 0.5f;

    private float _currentValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCamera()
    {
        animationSequence.AddToSequence(AnimationSequence.Subject.MixingCamera, new Vector3(_currentValue, 0, 0),
            new Vector3(1 - _currentValue, 0, 0),
            transitionDuration, AnimationSequence.Curve.Quadratic);
        _currentValue = 1 - _currentValue;
    }
}
