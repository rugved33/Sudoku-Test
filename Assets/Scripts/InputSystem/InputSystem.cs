using UnityEngine;
using UnityEngine.Events;

public class InputSystem : MonoBehaviour
{

    private Vector2 initTouchLoc;
    private Vector2 lastTouchLoc;

    private float elapsedTime = -1f;
    private float tapThreshold = 0.2f;
    private float swipeResist = 0.2f;

    public UnityAction<Touch> OnTouchStartAction;
    public UnityAction<Touch> OnTouchEndAction;
    public UnityAction<Touch> OnTouchMovedAction;

    private void Start ()
    {
#if UNITY_ANDROID
        swipeResist = 10f;
#endif
    }
	private void Update ()
    {

        Touch touch;

#if UNITY_EDITOR
        touch = new Touch();

        if (Input.GetMouseButtonDown(0))
        {
            touch.phase = TouchPhase.Began;
            touch.position = Input.mousePosition;
            ProcessTouch(touch);
        }
        else if (Input.GetMouseButton(0)
            && !Mathf.Approximately(Input.mousePosition.x, initTouchLoc.x)
            && !Mathf.Approximately(Input.mousePosition.y, initTouchLoc.y))
        {
            touch.phase = TouchPhase.Moved;
            touch.position = Input.mousePosition;
            ProcessTouch(touch);
        }
        else if (Input.GetMouseButton(0))
        {
            touch.phase = TouchPhase.Moved;
            touch.position = Input.mousePosition;
            ProcessTouch(touch);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touch.phase = TouchPhase.Ended;
            touch.position = Input.mousePosition;
            ProcessTouch(touch);
        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1 && Input.touches[0].fingerId == 0)
        {
            touch = Input.touches[0];
            ProcessTouch(touch);
        }
#endif
    }

#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
    private void ProcessTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:

                OnTouchStartAction?.Invoke(touch);

                break;
            case TouchPhase.Moved:

                OnTouchMovedAction?.Invoke(touch);

                break;
            case TouchPhase.Ended:

                OnTouchEndAction?.Invoke(touch);

                break;
            case TouchPhase.Canceled:

                break;
        }
    }
#endif
}
