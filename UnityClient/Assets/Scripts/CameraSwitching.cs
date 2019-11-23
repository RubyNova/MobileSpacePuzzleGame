using UnityEngine;
using System.Collections;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Camera _overheadCamera;

    public void ShowOverheadView()
    {
        _mainCamera.enabled = false;
        _overheadCamera.enabled = true;
    }

    public void ShowFirstPersonView()
    {
        _mainCamera.enabled = true;
        _overheadCamera.enabled = false;
    }
}
