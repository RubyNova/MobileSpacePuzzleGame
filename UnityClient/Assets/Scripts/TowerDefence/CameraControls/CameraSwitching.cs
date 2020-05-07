using UnityEngine;
using System.Collections;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField]
    private GameObject _cameraChoice;

    [SerializeField]
    private Transform _allCameras;

    public void ChangeCamera()
    {

        foreach (Transform child in _allCameras.transform)
        {
            child.gameObject.SetActive(false);
        }

        _cameraChoice.SetActive(true);
    }
}
