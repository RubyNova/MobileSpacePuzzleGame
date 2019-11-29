using UnityEngine;
using System.Collections;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Camera _entranceRoomCamera;

    [SerializeField]
    private Camera _engineRoomCamera;

    [SerializeField]
    private Camera _labRoomCamera;

    [SerializeField]
    private Camera _AIRoomCamera;

    [SerializeField]
    private Camera _labRoomCamera2;


    public void ShowMainCamera()
    {
        _mainCamera.enabled = true;
        _entranceRoomCamera.enabled = false;
        _engineRoomCamera.enabled = false;
        _labRoomCamera.enabled = false;
        _AIRoomCamera.enabled = false;
        _labRoomCamera2.enabled = false;
    }


    public void ShowEntranceRoom()
    {
        _mainCamera.enabled = false;
        _entranceRoomCamera.enabled = true;
        _engineRoomCamera.enabled = false;
        _labRoomCamera.enabled = false;
        _AIRoomCamera.enabled = false;
        _labRoomCamera2.enabled = false;
    }

    public void ShowEngineRoom()
    {
        _mainCamera.enabled = false;
        _entranceRoomCamera.enabled = false;
        _engineRoomCamera.enabled = true;
        _labRoomCamera.enabled = false;
        _AIRoomCamera.enabled = false;
        _labRoomCamera2.enabled = false;
    }

    public void ShowLabRoom()
    {
        _mainCamera.enabled = false;
        _entranceRoomCamera.enabled = false;
        _engineRoomCamera.enabled = false;
        _labRoomCamera.enabled = true;
        _AIRoomCamera.enabled = false;
        _labRoomCamera2.enabled = false;
    }

    public void ShowAIRoom()
    {
        _mainCamera.enabled = false;
        _entranceRoomCamera.enabled = false;
        _engineRoomCamera.enabled = false;
        _labRoomCamera.enabled = false;
        _AIRoomCamera.enabled = true;
        _labRoomCamera2.enabled = false;
    }

    public void ShowLabRoom2()
    {
        _mainCamera.enabled = false;
        _entranceRoomCamera.enabled = false;
        _engineRoomCamera.enabled = false;
        _labRoomCamera.enabled = false;
        _AIRoomCamera.enabled = false;
        _labRoomCamera2.enabled = true;
    }

}