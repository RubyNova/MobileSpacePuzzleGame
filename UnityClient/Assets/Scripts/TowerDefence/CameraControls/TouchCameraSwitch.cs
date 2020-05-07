using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraSwitch : MonoBehaviour
{

    [SerializeField]
    private GameObject _cameraChoice;

    [SerializeField]
    private Transform _allCameras;

    private void OnMouseDown()
    {
        foreach (Transform child in _allCameras.transform)
        {
            child.gameObject.SetActive(false);
        }

        _cameraChoice.SetActive(true);
    }

}
