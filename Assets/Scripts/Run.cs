using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    KlikKlik inputy;
    Rigidbody myRb;
    [SerializeField] float speed = 10, targetSpeed = 10, startSpeed = 15, speedUpTime = 2;
    [SerializeField, Range(0,100)] float CamSpeedY = 1, CamSpeedX = 1;
    [SerializeField] float minAngle, maxAngle, stickiness=5, targetSSpeed = 10, startSSpeed = 15, SspeedUpTime = 2, divider=44, timeToStopMiAir = 3;
    Vector2 val = Vector2.zero;
    Transform MyCamera, Camholder, rotator;
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
        MyCamera = transform.GetChild(0).GetChild(0).GetChild(0);
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
            } else if(myRb.velocity.y == 0) {
                Jump();
            }
        };

        inputy.main.JumpSlide.canceled += v => {
            var direction = v.ReadValue<float>();
            Slide(1f);
        };
    }
    Vector3 angles, lastVel;
    Vector2 MDelta;
    float ClimbForce = 0;
    private void Update()
    {
        if (!climb && !isGrounded && !slope)
        {
            val = Vector2.zero;
            myRb.velocity = Vector3.Lerp(myRb.velocity, Physics.gravity, Time.deltaTime / timeToStopMiAir);
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
            transform.Rotate(new Vector3(0, MDelta.x * (CamSpeedX), 0), Space.World);
        }
        if (!climb)
            myRb.AddForce(transform.forward * val.y * speed * Time.deltaTime, ForceMode.Force);
        else
        {
            myRb.AddForce(transform.up * ClimbForce * val.y, ForceMode.Force);
            //Debug.Log(ClimbForce);
        }
        myRb.AddForce(transform.right * val.x * speed * Time.deltaTime, ForceMode.Force);
        if ((climb||slope) && ClimbForce > 10)
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
            ClimbForce = (lastVel.z / divider) - Physics.gravity.y;
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
            myRb.AddForce(transform.forward*ClimbForce*val.y, ForceMode.Force);
            Debug.DrawRay(transform.position, transform.forward * ClimbForce);
        }
        if (collision.gameObject.CompareTag("Slope"))
        {
            myRb.AddForce(transform.forward * ClimbForce*val.y*stickiness, ForceMode.Force);
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
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f));
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
        while (timed < time)
        {
            timed += Time.deltaTime;
            //Debug.Log(timed);
            what.localEulerAngles = Vector3.Lerp(actEuler, new Vector3(axis == 0 ? angle : 0, axis == 1 ? angle : 0, axis == 2 ? angle : 0), timed / time);
            //Debug.Log(actEuler);
            yield return null;
        }
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
