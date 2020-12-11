using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    KlikKlik inputy;
    Rigidbody myRb;
    [SerializeField] float speed = 10;
    [SerializeField, Range(0,3)] float CamSpeedY = 1, CamSpeedX = 1;
    [SerializeField] float minAngle, maxAngle;
    Vector2 val = Vector2.zero;
    Transform MyCamera;

    private void Start()
    {
        MyCamera = transform.GetChild(0);
        inputy = new KlikKlik();
        inputy.main.Enable();
        myRb = GetComponent<Rigidbody>();
        inputy.main.move.performed += v => {
            //Debug.Log("Why nufyn");
            val = v.ReadValue<Vector2>();
        };
        inputy.main.move.canceled += v => {
            //Debug.Log("Why nufynC");
            val = v.ReadValue<Vector2>();
        };
        inputy.main.Camera.performed += v =>
        {
            MyCamera.Rotate(new Vector3(-v.ReadValue<Vector2>().y*(CamSpeedY/10), 0,0),Space.Self);
            Vector3 angles = MyCamera.localEulerAngles;
            angles.x -= angles.x > 90 ? 360 : 0;
            if (angles.x < minAngle)
                angles.x = minAngle;
            else if (angles.x > maxAngle)
                angles.x = maxAngle;
            MyCamera.localEulerAngles = angles;
            //Debug.Log(angles);
            transform.Rotate(new Vector3(0, v.ReadValue<Vector2>().x * (CamSpeedX / 10), 0), Space.World);
        };
    }

    private void Update()
    {
        myRb.AddForce(transform.forward * val.y * speed, ForceMode.Force);
        myRb.AddForce(transform.right * val.x * speed, ForceMode.Force);
        //Debug.Log(inputy.main.Camera.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
