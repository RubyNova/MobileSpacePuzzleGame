using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseControl : MonoBehaviour
{

    private Vector2 _mouseDirection;

    private Transform _parentBody;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _parentBody = this.transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseChange = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        _mouseDirection += mouseChange;

        transform.localRotation = Quaternion.AngleAxis(-_mouseDirection.y, Vector3.right);

        _parentBody.localRotation = Quaternion.AngleAxis(_mouseDirection.x, Vector3.up);
    }
}
