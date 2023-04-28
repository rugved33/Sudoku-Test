using UnityEngine;
using System;
using Zenject;

public class VirtualJoystick : MonoBehaviour
{
    private const float INPUT_RANGE = 0.001f;
    private bool _isTouching;
    private Vector2 _direction;
    private Vector3 _startTouchPos;
    private Vector3 _currentTouchPos;
    private InputSystem _inputSystem;

    public bool IsTouching
    {
        get => _isTouching;
    }

    public Vector2 Offset
    {
        get => _direction;
    }

    [Inject]
    private void Construct(InputSystem inputSystem)
    {
        _inputSystem = inputSystem;
        Initialize();
    }

    private void Initialize()
    {
        try
        {
            _inputSystem.OnTouchStartAction += OnTouchStart;
            _inputSystem.OnTouchMovedAction += OnTouchMoved;
            _inputSystem.OnTouchEndAction += OnTouchEnd;
        }
        catch(Exception exception)
        {
            Debug.Log("<color=red>ERROR : InputSystem is not injected </color>" + exception );
        }
    }

#region  INPUT
    private void OnTouchStart(Touch touch)
    {
        _isTouching = true;
        _startTouchPos = touch.position;
    }

    private void OnTouchMoved(Touch touch)
    {
        _currentTouchPos = touch.position;
        _currentTouchPos = _startTouchPos + Vector3.ClampMagnitude(_currentTouchPos - _startTouchPos, INPUT_RANGE);
        _direction = (_currentTouchPos - _startTouchPos).normalized;
    }

    private void OnTouchEnd(Touch touch)
    {
        _isTouching = false;
        _direction = Vector3.zero;
    }
#endregion

}
