using UnityEngine;
using System;
using Zenject;

public class DragControl : MonoBehaviour
{
    [Header("Joystick dead area in screen terms [0,1]")] [SerializeField]
    private float _deadZone = 0.05f;
    [Header("Factor multiplier for the output (optional)")]
    [SerializeField]
    private float _factor = 1f;
    private bool _isTouching;
    private Vector3 _startTouchPos;
    private bool _moving;
    private InputSystem _inputSystem;
    private Vector2 _offset;
    public Vector2 Offset
    {
        get => _offset;
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
            _inputSystem.OnTouchEndAction += OnTouchEnd;
        }
        catch(Exception Exception)
        {
            Debug.Log("<color=red> ERROR </color>" + Exception);
        }
    }

#region  INPUT
    private void OnTouchStart(Touch touch)
    {
        _isTouching = true;
        _startTouchPos = touch.position;
    }

    private void OnTouchEnd(Touch touch)
    {
        _isTouching = false;
        _moving = false;
    }
#endregion
    private void ResetOffset()
    {
        if (_isTouching)
        {
            _startTouchPos = Input.mousePosition;
        }
    }

    protected void Update()
    {
        if(_isTouching)
        {
            Vector3 offset = (Input.mousePosition - _startTouchPos);
            offset /= Screen.width;

            float magnitude = offset.magnitude;
            if(magnitude > _deadZone)
            {
                _moving = true;
            }

            if(_moving)
            {
                Vector3 factorizedOffset = offset * _factor;
                _offset = new Vector2(factorizedOffset.x, factorizedOffset.y);
                ResetOffset();
            }
        }
    }
}
