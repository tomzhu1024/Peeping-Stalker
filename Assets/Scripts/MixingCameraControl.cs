using Cinemachine;
using UnityEngine;

public class MixingCameraControl : MonoBehaviour
{
    public AnimationSequence animationSequence;
    public CinemachineMixingCamera cameraMixer;
    public CinemachineVirtualCameraBase cameraNarrow;
    public CinemachineVirtualCameraBase cameraWide;
    public float transitionDuration = 1f;

    private float _currentValue;

    // Start is called before the first frame update
    void Start()
    {
        _currentValue = 0;
        SetCamera(_currentValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (animationSequence.IsAnimating)
        {
            SetCamera(animationSequence.Value.x);
        }
    }

    public void SwitchCamera()
    {
        animationSequence.AddToSequence(AnimationSequence.Subject.Nothing, new Vector3(_currentValue, 0, 0),
            new Vector3(1 - _currentValue, 0, 0),
            transitionDuration, AnimationSequence.Curve.Quadratic);
        _currentValue = 1 - _currentValue;
    }

    private void SetCamera(float value)
    {
        cameraMixer.SetWeight(cameraNarrow, 1 -value);
        cameraMixer.SetWeight(cameraWide, value);
    }
}
