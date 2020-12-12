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
    [SerializeField] float jumpForce = 500;
    [SerializeField] float slideSize = 0.5f;
    private float properSize = 1f;

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

        inputy.main.JumpSlide.performed += v => {
            var direction = v.ReadValue<float>();
            if(direction == -1) {
                Slide(slideSize);
            } else if(myRb.velocity.y == 0) {
                Jump();
            }
        };

        inputy.main.JumpSlide.canceled += v => {
            var direction = v.ReadValue<float>();
            Slide(1f);
        };
    }

    private void Update()
    {
        myRb.AddForce(transform.forward * val.y * speed, ForceMode.Force);
        myRb.AddForce(transform.right * val.x * speed, ForceMode.Force);
        //Debug.Log(inputy.main.Camera.ReadValue<Vector2>());

        RaycastHit hit;

        if(transform.localScale.y > 0.9999f || transform.localScale.y < 0.50001){
            transform.localScale = new Vector3(1f, properSize, 1f);
        }
        if(properSize != transform.localScale.y && !Physics.Raycast(transform.position, transform.up, out hit, Mathf.Abs(properSize - transform.localScale.y))){
            Debug.Log(hit.transform?.gameObject?.tag);
            // var test1 = new Vector3(1f, properSize, 1f);
            // transform.localScale = Vector3.Lerp(transform.localScale, test1, 20f * Time.deltaTime);
            transform.localScale = new Vector3(1f, Mathf.SmoothStep(transform.localScale.y, properSize, 0.5f * Time.deltaTime), 1f);
        }
    }

    private void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Jump() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 20f)){
            Debug.Log(hit.transform.name);
            myRb.AddForce(transform.up * jumpForce, ForceMode.Force);
        }
        Debug.Log("Jump");
    }

    private void Slide(float size) {
        properSize = size;
    }
}
