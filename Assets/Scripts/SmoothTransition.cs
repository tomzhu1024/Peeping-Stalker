using System;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition
{
    public static int Linear = 0;
    public static int Quadratic = 1;
    
    private readonly Queue<(float, float, float, int)> _actionQueue = new Queue<(float, float, float, int)>();
    private float _startTime;
    private float _duration;
    private float _fromValue;
    private float _toValue;
    private int _method;

    public bool IsTransiting;
    public float CurrentValue;

    public void AddAction(float duration, float fromValue, float toValue, int method)
    {
        _actionQueue.Enqueue((duration, fromValue, toValue, method));
    }

    public void Update()
    {
        if (!IsTransiting && _actionQueue.Count > 0)
        {
            IsTransiting = true;
            FetchNextAction();
        }
        if (!IsTransiting)
            return;
        switch (_method)
        {
            case 0:
            {
                CurrentValue = _fromValue + (_toValue - _fromValue) * CurveLinear((Time.time - _startTime) / _duration);
                break;
            }
            case 1:
            {
                CurrentValue = _fromValue + (_toValue - _fromValue) * CurveQuadratic((Time.time - _startTime) / _duration);
                break;
            }
        }
        if (Time.time - _startTime < _duration)
            return;
        if (_actionQueue.Count > 0)
        {
            FetchNextAction();
            return;
        }
        IsTransiting = false;
    }

    private void FetchNextAction()
    {
        var (duration, fromValue, toValue, method) = _actionQueue.Dequeue();
        _startTime = Time.time;
        _duration = duration;
        _fromValue = fromValue;
        _toValue = toValue;
        _method = method;
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
