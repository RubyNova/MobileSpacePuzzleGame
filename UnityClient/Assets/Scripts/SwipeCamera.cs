using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCamera : MonoBehaviour
{

    private Touch _initTouch = new Touch();

    [SerializeField]
    private Camera _swipeCameraControl;

    [SerializeField]
    private float _rotationSpeed = 0.2f;

    [SerializeField]
    private float _direction = -1f;

    private float _rotX = 0f;
    private float _rotY = 0f;
    private Vector3 _origin;

    // Start is called before the first frame update
    void Start()
    {
        _origin = _swipeCameraControl.transform.eulerAngles;
        _rotX = _origin.x;
        _rotY = _origin.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Touch _touch in Input.touches)
        {
            if (_touch.phase == TouchPhase.Began)
            {
                _initTouch = _touch;
            }
            else if (_touch.phase == TouchPhase.Moved)
            {
                float _deltaX = _initTouch.position.x - _touch.position.x;
                float _deltaY = _initTouch.position.y - _touch.position.y;

                _rotX -= _deltaY * Time.deltaTime * _rotationSpeed * _direction;
                _rotY += _deltaX * Time.deltaTime * _rotationSpeed * _direction;

                _rotX = Mathf.Clamp(_rotX, -45f, 45f);

                _swipeCameraControl.transform.eulerAngles = new Vector3(_rotX, _rotY, 0f);
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                _initTouch = new Touch();
            }
        }
    }
}
