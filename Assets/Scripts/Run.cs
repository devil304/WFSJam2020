using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    KlikKlik inputy;
    Rigidbody myRb;
    [SerializeField] float speed = 10;
    [SerializeField, Range(0,100)] float CamSpeedY = 1, CamSpeedX = 1;
    [SerializeField] float minAngle, maxAngle;
    Vector2 val = Vector2.zero;
    Transform MyCamera, Camholder, rotator;
    [SerializeField]bool climb, isGrounded, wasGrounded;
    int CamCanc = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MyCamera = transform.GetChild(0).GetChild(0).GetChild(0);
        Camholder = transform.GetChild(0).GetChild(0);
        rotator = transform.GetChild(0);

        //StartCoroutine(RotateInTime(-45, rotator, Space.Self, 5f));
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
            MDelta = v.ReadValue<Vector2>();
            angles = MyCamera.localEulerAngles+ new Vector3(-MDelta.y * (CamSpeedY)*Time.deltaTime, 0, 0);
            angles.x -= angles.x > 90 ? 360 : 0;
            if (angles.x < minAngle)
                angles.x = minAngle;
            else if (angles.x > maxAngle)
                angles.x = maxAngle;
            //Debug.Log(MDelta);
            CamCanc = 0;
        };
        inputy.main.Camera.canceled += v =>
        {
            CamCanc = 1;
        };
    }
    Vector3 angles, lastVel;
    Vector2 MDelta;
    float ClimbForce = 0;
    private void Update()
    {
        if (MDelta!=Vector2.zero)
        {
            MyCamera.localEulerAngles = angles;
            //Debug.Log(angles);
            transform.Rotate(new Vector3(0, MDelta.x * (CamSpeedX) * Time.deltaTime, 0), Space.World);
        }
        if (!climb)
            myRb.AddForce(transform.forward * val.y * speed * Time.deltaTime, ForceMode.Force);
        else
        {
            myRb.AddForce(transform.up * ClimbForce, ForceMode.Force);
            //Debug.Log(ClimbForce);
        }
        myRb.AddForce(transform.right * val.x * speed * Time.deltaTime, ForceMode.Force);
        if (climb && ClimbForce > 10)
            ClimbForce -= (myRb.drag/2) * Time.deltaTime;
        else
        {
            ClimbForce = 0;
            //climb = false;
        }
        if(climb && isGrounded && !wasGrounded)
        {
            climb = false;
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f));
        }
            
        //Debug.Log(inputy.main.Camera.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        if (CamCanc == 1)
            CamCanc = 2;
        if (CamCanc == 2)
        {
            MDelta = Vector2.zero;
            CamCanc = 0;
        }
        lastVel = myRb.velocity!=Vector3.zero?myRb.velocity:lastVel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            climb = true;
            ClimbForce = (lastVel.z/44) - Physics.gravity.y;
            StartCoroutine(RotateInTime(-45, rotator, Space.Self, 0.35f));
            StartCoroutine(RotateInTime(45 * 0.9f, Camholder, Space.Self, 0.35f));
            //Debug.Log(lastVel);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded=true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            wasGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            climb = false;
            /*transform.localEulerAngles = Vector3.zero;
            Camholder.localEulerAngles = Vector3.zero;*/
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f));
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            wasGrounded = false;
        }
    }
    IEnumerator RotateInTime(float angle, Transform what, Space spaceType, float time, int axis=0)
    {
        float timed =0;
        Vector3 actEuler = what.localEulerAngles;
        if ((axis == 0 ? actEuler.x : axis == 1 ? actEuler.y : actEuler.z) > 180)
        {
            //Debug.Log("Work");
            switch (axis)
            {
                case 0:
                    actEuler.x -= 360;
                    break;
                case 1:
                    actEuler.y -= 360;
                    break;
                case 2:
                    actEuler.z -= 360;
                    break;
            }
        }
        while (true)
        {
            timed += Time.deltaTime;
            Debug.Log(timed);
            what.localEulerAngles = Vector3.Lerp(actEuler, new Vector3(axis == 0 ? angle : 0, axis == 1 ? angle : 0, axis == 2 ? angle : 0), timed / time);
            //Debug.Log(actEuler);
            if (timed >= time)
                break;
            yield return null;
        }
    }
}
