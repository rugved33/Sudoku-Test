using Zenject;
using UnityEngine;
using UnityEngine.Events;
public class TapComponent : MonoBehaviour
{
    private const float THRESHOLD = 0.2f;
    private InputSystem _inputSystem;
    private UnityAction _tapAction;
    private float _elapsedTime;
    private bool _isTouching;
    public UnityAction TapAction
    {
        get => _tapAction;
        set => _tapAction = value;
    }

    [Inject]
    private void Construct(InputSystem inputSystem)
    {
        _inputSystem = inputSystem;
        _inputSystem.OnTouchEndAction += OnTouchEnd;
        _inputSystem.OnTouchStartAction += OnTouchStart;
    }
    private void OnTouchStart(Touch touch)
    {
        _isTouching = true;
        _elapsedTime = 0;
    }
    private void Update()
    {
        if(_isTouching)
        {
            _elapsedTime += Time.deltaTime;
        }
    }
    private void OnTouchEnd(Touch touch)
    {
        if(_elapsedTime < THRESHOLD)
        {
            TapAction?.Invoke();
            _elapsedTime = 0;
            _isTouching = false;
        }
    }
}
