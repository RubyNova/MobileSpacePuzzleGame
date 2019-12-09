using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileZoom : MonoBehaviour
{
    [SerializeField]
    private float _zoomSpeed = 0.1f;

    [SerializeField]
    private Camera _targetCamera;

    private const float YFov = 0.1f;
    private const float ZFov = 100f;

    private void Update()
    {
        if (Input.touchCount < 2) return;

        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        _targetCamera.fieldOfView += deltaMagnitudeDiff * _zoomSpeed;
        _targetCamera.fieldOfView = Mathf.Clamp(_targetCamera.fieldOfView, YFov, ZFov);
    }
}
