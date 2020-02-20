using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeCamera : MonoBehaviour
{
    private void Start() => Input.gyro.enabled = true;

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);

    }
}