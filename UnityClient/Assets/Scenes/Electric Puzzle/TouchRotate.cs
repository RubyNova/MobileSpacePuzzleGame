using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameControl.youWin)
            transform.Rotate(0f, 0f, 90f);
    }
}
