using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    KlikKlik inputy;
    Rigidbody myRb;
    [SerializeField] float speed = 10, targetSpeed = 10, startSpeed = 15, speedUpTime = 2;
    [SerializeField, Range(0,100)] float CamSpeedY = 1, CamSpeedX = 1;
    [SerializeField] float minAngle, maxAngle, stickiness=5, targetSSpeed = 10, startSSpeed = 15, SspeedUpTime = 2, divider=44, timeToStopMiAir = 3, AirDragXZ = 2;
    Vector2 val = Vector2.zero;
    Transform MyCamera, Camholder, rotator, Gimbal;
    [SerializeField]bool climb, isGrounded, wasGrounded, slope;
    int CamCanc = 0;
    Coroutine SIActive;
    
    [SerializeField] float jumpForce = 500;
    [SerializeField] float slideSize = 0.5f;
    private float properSize = 1f;

    private void Start()
    {
        Application.targetFrameRate = 0;
        Cursor.lockState = CursorLockMode.Locked;
        MyCamera = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        Gimbal = transform.GetChild(0).GetChild(0).GetChild(0);
        Camholder = transform.GetChild(0).GetChild(0);
        rotator = transform.GetChild(0);

        //StartCoroutine(RotateInTime(-45, rotator, Space.Self, 5f));
        inputy = new KlikKlik();
        inputy.main.Enable();
        myRb = GetComponent<Rigidbody>();
        inputy.main.move.performed += v => {
            if (val == Vector2.zero)
            {
                if (SIActive != null)
                    StopCoroutine(SIActive);
                SIActive = StartCoroutine(InterpolateSpeed(startSpeed, targetSpeed, speedUpTime));
            }
            //Debug.Log("Why nufyn");
            val = v.ReadValue<Vector2>();
        };
        inputy.main.move.canceled += v => {
            if (val == Vector2.zero)
            {
                if(SIActive!=null)
                    StopCoroutine(SIActive);
                SIActive = StartCoroutine(InterpolateSpeed(startSpeed, targetSpeed, speedUpTime));
            }
            //Debug.Log("Why nufynC");
            val = v.ReadValue<Vector2>();
        };
        inputy.main.Camera.performed += v =>
        {
            MDelta = v.ReadValue<Vector2>();
            angles = MyCamera.localEulerAngles+ new Vector3(-MDelta.y * (CamSpeedY), 0, 0);
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

        inputy.main.JumpSlide.performed += v => {
            var direction = v.ReadValue<float>();
            if(direction == -1) {
                Slide(slideSize);
            } else {
                Jump();
            }
        };

        inputy.main.JumpSlide.canceled += v => {
            var direction = v.ReadValue<float>();
            Slide(1f);
        };
        inputy.main.Hook.started += v =>
        {
            RaycastHit hit;
            if (Physics.Raycast(MyCamera.position, MyCamera.forward, out hit, 64))
            {
                myRb.AddForce(MyCamera.forward*32,ForceMode.Impulse);
            }
        };
    }
    Vector3 angles, lastVel;
    Vector2 MDelta;
    [SerializeField]float ClimbForce = 0;
    private void Update()
    {
        if (!climb && !isGrounded && !slope)
        {
            val = Vector2.zero;
            //myRb.velocity = Vector3.Lerp(myRb.velocity, Physics.gravity, Time.deltaTime / timeToStopMiAir);
            Vector3 vel = transform.InverseTransformDirection(myRb.velocity);
            vel.x *= AirDragXZ;
            if(vel.y>0)
                vel.y *= AirDragXZ;
            vel.z *= AirDragXZ;
            myRb.velocity = transform.TransformDirection(vel);
            //Debug.Log(myRb.velocity);
        }
        else if (val == Vector2.zero)
        {
            val = inputy.main.move.ReadValue<Vector2>();
            if (val != Vector2.zero)
            {
                if (SIActive != null)
                    StopCoroutine(SIActive);
                SIActive = StartCoroutine(InterpolateSpeed(startSpeed, targetSpeed, speedUpTime));
            }
        }
        if (MDelta!=Vector2.zero)
        {
            MyCamera.localEulerAngles = angles;
            //Debug.Log(angles);
            if (!climb)
                transform.Rotate(new Vector3(0, MDelta.x * (CamSpeedX), 0), Space.World);
            else
                Gimbal.Rotate(new Vector3(0, MDelta.x * (CamSpeedX), 0), Space.Self);
        }
        if (!climb)
            myRb.AddForce(transform.forward * val.y * speed * Time.deltaTime, ForceMode.Force);
        else
        {
            myRb.AddForce(transform.up * ClimbForce * val.y, ForceMode.Force);
            //Debug.Log(ClimbForce);
        }
        myRb.AddForce(transform.right * val.x * speed * Time.deltaTime, ForceMode.Force);
        if ((climb||slope)&&((climb&&ClimbForce>0)||slope))
            ClimbForce -= (AirDragXZ/(climb?0.06f:0.15f)) * Time.deltaTime;
        if(climb && isGrounded && !wasGrounded)
        {
            climb = false;
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f, ResetRot));
        }
            
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
            ClimbForce = (lastVel.z/divider) - Physics.gravity.y;
            //Debug.Log(lastVel);
        }
        if(collision.gameObject.CompareTag("Slope"))
        {
            slope = true;
            ClimbForce = (Mathf.Abs(lastVel.z*lastVel.x) / divider) - Physics.gravity.y;
            StartCoroutine(InterpolateStick(startSSpeed,targetSSpeed,SspeedUpTime));
            //Debug.Log(lastVel);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded=true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall") && climb)
        {
            myRb.AddForce(transform.forward*val.y*5, ForceMode.Force);
            Debug.DrawRay(transform.position, transform.forward * val.y*5);
        }
        if (collision.gameObject.CompareTag("Slope"))
        {
            myRb.AddForce(Vector3.Cross(transform.right,collision.contacts[0].normal) * ClimbForce*val.y*stickiness, ForceMode.Force);
            Debug.DrawRay(transform.position,Vector3.Cross(transform.right, collision.contacts[0].normal) * ClimbForce * val.y * stickiness);
        }
        if (collision.gameObject.CompareTag("Ground"))
            wasGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            climb = false;
            
            /*transform.localEulerAngles = Vector3.zero;
            Camholder.localEulerAngles = Vector3.zero;*/
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f, ResetRot));
        }
        if (collision.gameObject.CompareTag("Slope"))
        {
            slope = false;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            wasGrounded = false;
            if (climb)
            {
                StartCoroutine(RotateInTime(-45, rotator, Space.Self, 0.25f));
                StartCoroutine(RotateInTime(45 * 0.9f, Camholder, Space.Self, 0.25f));
            }
        }
    }
    IEnumerator InterpolateSpeed(float speedStart, float speedEnd, float time)
    {
        float timed = 0;
        speed = speedStart;
        while (timed < time)
        {
            timed += Time.deltaTime;
            speed = Mathf.SmoothStep(speedStart, speedEnd, timed / time);
            yield return null;
        }
        speed = speedEnd;
    }

    IEnumerator InterpolateStick(float speedStart, float speedEnd, float time)
    {
        float timed = 0;
        stickiness = speedStart;
        while (timed < time)
        {
            timed += Time.deltaTime;
            stickiness = Mathf.SmoothStep(speedStart, speedEnd, timed / time);
            yield return null;
        }
        stickiness = speedEnd;
    }
    void ResetRot()
    {
        if (Gimbal.localEulerAngles.y != 0)
        {
            transform.localEulerAngles += Vector3.up * Gimbal.localEulerAngles.y;
            Gimbal.localEulerAngles -= Vector3.up * Gimbal.localEulerAngles.y;
        }
    }
    delegate void OnTheEnd();
    IEnumerator RotateInTime(float angle, Transform what, Space spaceType, float time, OnTheEnd RunOnFinish=null, int axis=0)
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
        while (timed < time)
        {
            timed += Time.deltaTime;
            //Debug.Log(timed);
            what.localEulerAngles = Vector3.Lerp(actEuler, new Vector3(axis == 0 ? angle : actEuler.x, axis == 1 ? angle : actEuler.y, axis == 2 ? angle : actEuler.z), timed / time);
            //Debug.Log(actEuler);
            yield return null;
        }
        if (RunOnFinish != null)
            RunOnFinish.Invoke();
    }

    private void Jump() {
        if(isGrounded){
            myRb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
        }
    }

    private void Slide(float size) {
        properSize = size;
    }
}
