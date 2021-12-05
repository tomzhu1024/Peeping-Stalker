using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{
    public enum Subject
    {
        Nothing,
        Position,
        Rotation
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

    public bool IsAnimating => _state == State.Animating;
    public Vector3 Value { get; private set; }

    private State _state;
    private Queue<Animation> _animations;
    private float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _state = 0;
        _animations = new Queue<Animation>();
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
                    case Subject.Nothing:
                    {
                        break;
                    }
                    case Subject.Position:
                    {
                        transform.position = value;
                        break;
                    }
                    case Subject.Rotation:
                    {
                        transform.rotation = new Quaternion(value.x, value.y, value.z, 1);
                        break;
                    }
                }
                // Check if animation ends
                if (Time.time - _startTime >= anim.Duration)
                {
                    _state = State.NotAnimating;
                    _animations.Dequeue();
                }
                break;
            }
        }
    }

    public void AddToSequence(Subject subject, Vector3 startValue, Vector3 endValue, float duration, Curve curve)
    {
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
