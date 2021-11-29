using Cinemachine;
using UnityEngine;

public class MixingCameraControl : MonoBehaviour
{
    public float transitionDuration = 1f;
    public CinemachineVirtualCameraBase cameraNarrow;
    public CinemachineVirtualCameraBase cameraWide;

    private float _currentCamera = 0f;
    private CinemachineMixingCamera _mixer;
    private SmoothTransition _smoothTransition = new SmoothTransition();

    // Start is called before the first frame update
    private void Start()
    {
        _mixer = GetComponent<CinemachineMixingCamera>();
        SetCamera(_currentCamera);
    }

    // Update is called once per frame
    private void Update()
    {
        _smoothTransition.Update();
        if (!_smoothTransition.IsTransiting)
            return;
        SetCamera(_smoothTransition.CurrentValue);
    }

    private void SetCamera(float value)
    {
        _mixer.SetWeight(cameraNarrow, 1 - value);
        _mixer.SetWeight(cameraWide, value);
    }

    public void SwitchCamera()
    {
        _smoothTransition.AddAction(transitionDuration, _currentCamera, 1 - _currentCamera, SmoothTransition.Quadratic);
        _currentCamera = 1 - _currentCamera;
    }
}
