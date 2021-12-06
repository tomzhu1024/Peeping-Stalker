using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Cinemachine;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{
    public enum Subject
    {
        Position,
        RotationTowards,
        MixingCamera,
        RotationEuler,
        Enable,
        Disable
    }

    public enum Curve
    {
        Linear,
        Quadratic
    }

    private enum State
    {
        NotAnimating,
        Animating
    }

    private readonly struct Animation
    {
        public Subject Subject { get; }
        public Vector3 StartValue { get; }
        public Vector3 EndValue { get; }
        public float Duration { get; }
        public Curve Curve { get; }

        public Animation(Subject subject, Vector3 startValue, Vector3 endValue, float duration, Curve curve)
        {
            Subject = subject;
            StartValue = startValue;
            EndValue = endValue;
            Duration = duration;
            Curve = curve;
        }
    }

    // For chess
    [SerializeField] private Animator[] chessAnimators;
    // For mixing camera
    [SerializeField] private CinemachineMixingCamera cameraMixer;
    [SerializeField] private CinemachineVirtualCameraBase camera0;
    [SerializeField] private CinemachineVirtualCameraBase camera1;
    // For enable/disable
    [SerializeField] private GameObject[] gameObjects;

    private State _state = State.NotAnimating;
    private readonly Queue<Animation> _animations = new Queue<Animation>();
    private float _startTime;

    public bool IsAnimating => _state == State.Animating || _animations.Count > 0;
    public Vector3 Value { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.NotAnimating:
            {
                // New animations incoming
                if (_animations.Count > 0)
                {
                    _state = State.Animating;
                    _startTime = Time.time;
                }
                break;
            }
            case State.Animating:
            {
                // Continue animation
                var anim = _animations.Peek();
                // Calculate animation progress percentage
                var timeProg = (Time.time - _startTime) / anim.Duration;
                var animProg = 0f;
                switch (anim.Curve)
                {
                    case Curve.Linear:
                    {
                        animProg = CurveLinear(timeProg);
                        break;
                    }
                    case Curve.Quadratic:
                    {
                        animProg = CurveQuadratic(timeProg);
                        break;
                    }
                }
                // Apply to subject
                var value = anim.StartValue * (1 - animProg) + anim.EndValue * animProg;
                Value = value;
                switch (anim.Subject)
                {
                    case Subject.Position:
                    {
                        transform.position = value;
                        // Start walking animation, for chess only
                        foreach (var chessAnimator in chessAnimators)
                        {
                            if (chessAnimator != null)
                            {
                                chessAnimator.SetBool("IsWalking", true);
                            }
                        }
                        break;
                    }
                    case Subject.RotationTowards:
                    {
                        transform.rotation = Quaternion.Euler(value);
                        break;
                    }
                    case Subject.MixingCamera:
                    {
                        // Updating camera weights, for mixing camera only
                        if (cameraMixer != null && camera0 != null && camera1 != null)
                        {
                            // Only use the X-dimension value
                            cameraMixer.SetWeight(camera0, 1 - value.x);
                            cameraMixer.SetWeight(camera1, value.x);
                        }
                        break;
                    }
                    case Subject.RotationEuler:
                    {
                        transform.rotation = Quaternion.Euler(value);
                        break;
                    }
                    case Subject.Enable:
                    {
                        // Enable game object for render
                        foreach (var go in gameObjects)
                        {
                            print("try to activate " + go);
                            if (go != null)
                            {
                                go.SetActive(true);
                            }
                        }
                        break;
                    }
                    case Subject.Disable:
                    {
                        // Disable game object for render
                        foreach (var go in gameObjects)
                        {
                            if (go != null)
                            {
                                go.SetActive(false);
                            }
                        }
                        break;
                    }
                }
                // Check if animation ends
                if (Time.time - _startTime >= anim.Duration)
                {
                    _state = State.NotAnimating;
                    _animations.Dequeue();
                    // Stop walking animation, for chess only
                    foreach (var chessAnimator in chessAnimators)
                    {
                        chessAnimator.SetBool("IsWalking", false);
                    }
                }
                break;
            }
        }
    }

    public void AddToSequence(Subject subject, Vector3 startValue, Vector3 endValue, float duration, Curve curve)
    {
        if (subject == Subject.RotationTowards)
        {
            startValue = Quaternion.LookRotation(startValue).eulerAngles;
            endValue = Quaternion.LookRotation(endValue).eulerAngles;
        }
        if (subject == Subject.Disable || subject == Subject.Enable)
        {
            duration = 0;
        }
        _animations.Enqueue(new Animation(subject, startValue, endValue, duration, curve));
    }

    private float CurveLinear(float x)
    {
        if (x < 0f)
            return 0f;
        if (x > 1f)
            return 1f;
        return x;
    }

    private float CurveQuadratic(float x)
    {
        if (x < 0f)
            return 0f;
        if (x > 1)
            return 1f;
        if (x <= 0.5f)
            return 2 * (float)Math.Pow(x, 2);
        return -2 * (float)Math.Pow(x, 2) + 4 * x - 1;
    }

}
