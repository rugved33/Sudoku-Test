using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private float _maxAngleDiff = 45f;
    [SerializeField] private float _minDist = 60.0f;
    [SerializeField] private float _minSpeed= 1500.0f;
    [SerializeField] private UnityEvent _onSwipeUp;
    [SerializeField] private UnityEvent _onSwipeDown;
    [SerializeField] private UnityEvent _onSwipeLeft;
    [SerializeField] private UnityEvent _onSwipeRight;

    private InputSystem _inputSystem;
    private Vector2 _startPos = Vector2.zero;
    private float _mouseDownTime = 0f;


    [Inject]
    private void Construct(InputSystem inputSystem)
    {
        _inputSystem = inputSystem;
        _inputSystem.OnTouchEndAction += OnTouchUp;
        _inputSystem.OnTouchStartAction += OnTouchDown;
    }

    private void OnTouchDown(Touch touch)
    {
        _startPos = new Vector2(touch.position.x, touch.position.y);
        _mouseDownTime = Time.time;
    }

    private void OnTouchUp(Touch touch)
    {
        Vector2 swipe = new Vector2(touch.position.x, touch.position.y) - _startPos;

        float timeDiff = Time.time - _mouseDownTime;
        float speed = swipe.magnitude / timeDiff;

        if (speed > _minSpeed && swipe.magnitude > _minDist)
        {
            swipe.Normalize();

            float angle = Mathf.Acos(Vector2.Dot(swipe, Vector2.left)) * Mathf.Rad2Deg;

            if (angle < _maxAngleDiff)
            {
                _onSwipeRight.Invoke();
            }
            else if ((180.0f - angle) < _maxAngleDiff)
            {
                _onSwipeLeft.Invoke();
            }
            else
            {
                angle = Mathf.Acos(Vector2.Dot(swipe, Vector2.up)) * Mathf.Rad2Deg;
                if (angle < _maxAngleDiff)
                {
                    _onSwipeUp.Invoke();
                }
                else if ((180.0f - angle) < _maxAngleDiff)
                {
                    _onSwipeDown.Invoke();
                }
            }
        }
    }
}