using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    KlikKlik inputy;
    Rigidbody myRb;
    [SerializeField] float speed = 10, targetSpeed = 10, startSpeed = 15, speedUpTime = 2;
    [SerializeField, Range(0,100)] float CamSpeedY = 1, CamSpeedX = 1;
    [SerializeField] float minAngle, maxAngle, stickiness=5, targetSSpeed = 10, startSSpeed = 15, SspeedUpTime = 2, divider=44, divider2=44, divider3 = 3, dividerSlide = 100, dividerCrouch = 15, timeToStopMiAir = 3, AirDragXZ = 2, maxVelocity = 15;
    Vector2 val = Vector2.zero;
    Transform MyCamera, Camholder, rotator, Gimbal;
    [SerializeField]bool climb, isGrounded, wasGrounded, slope;
    int CamCanc = 0;
    Coroutine SIActive;
    
    [SerializeField] float jumpUpForce = 500;
    [SerializeField] float jumpForwardForce = 250;
    [SerializeField] float slideSize = 0.5f;
    [SerializeField] AudioClip[] steps;
    [SerializeField] AudioClip slide;
    [SerializeField] AudioSource SFX;
    private float properSize = 1f;
    public int powerUps = 0;
    private bool isSliding = false;

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
            if (!climb && !isGrounded && !slope)
            {
                val /= divider3;
                //Debug.Log(val);
            }

            if(isSliding && myRb.velocity.magnitude > 2){
                val /= dividerSlide;
            } else if(isSliding) {
                val /= dividerCrouch;
            }
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
            if (!climb && !isGrounded && !slope)
            {
                val /= divider3;
                //Debug.Log(val);
            }
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

        inputy.main.Jump.performed += v => {
            Jump();
        };
        inputy.main.Slide.performed += v => {
            Slide(.5f);
            if(isSliding){
                val /= dividerSlide;
            }
        };

        inputy.main.Slide.canceled += v => {
            Slide(1f);
        };
        inputy.main.Hook.started += v =>
        {
            RaycastHit hit;
            if (powerUps > 0 && Physics.Raycast(MyCamera.position, MyCamera.forward, out hit, 64))
            {
                myRb.AddForce(MyCamera.forward * 32, ForceMode.Impulse);
                powerUps--;
            }
        };
    }
    Vector3 angles, lastVel;
    Vector2 MDelta;
    [SerializeField]float ClimbForce = 0;

    private void OnDestroy()
    {
        inputy.main.move.Dispose();
        inputy.main.Camera.Dispose();
        inputy.main.Jump.Dispose();
        inputy.main.Slide.Dispose();
        inputy.main.Hook.Dispose();
    }

    public void addPowerUP(int ile = 1)
    {
        powerUps += ile;
    }
    private void Update()
    {
        // Debug.Log(val);
        /*if (!climb && !isGrounded && !slope)
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
        else */if (val == Vector2.zero)
        {
            val = inputy.main.move.ReadValue<Vector2>();
            if (val != Vector2.zero)
            {
                if (SIActive != null)
                    StopCoroutine(SIActive);
                SIActive = StartCoroutine(InterpolateSpeed(startSpeed, targetSpeed, speedUpTime));
            }
            //Debug.Log(val);
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
        if (!climb && !slope)
            myRb.AddForce(transform.forward * val.y * speed * Time.deltaTime, ForceMode.Force);
        else if (slope)
        {
            myRb.AddForce(Vector3.Cross(transform.right, slopeC.contacts[0].normal) * ClimbForce * val.y * stickiness, ForceMode.Force);
        }
        else
        {
            myRb.AddForce(transform.up * ClimbForce * val.y, ForceMode.Force);
            //Debug.Log(ClimbForce);
        }
        myRb.AddForce(transform.right * val.x * speed * Time.deltaTime, ForceMode.Force);
        if ((climb||slope)&&ClimbForce>0)
            ClimbForce -= (AirDragXZ/(climb?0.015f:0.15f)) * Time.deltaTime;
        if(climb && isGrounded && !wasGrounded)
        {
            climb = false;
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f, ResetRot));
        }
            
        //Debug.Log(inputy.main.Camera.ReadValue<Vector2>());

        //RaycastHit hit;
        if((transform.localScale.y > 0.99f && properSize == 1f) || (transform.localScale.y < 0.501 && properSize == 0.5f)){
            transform.localScale = new Vector3(1f, properSize, 1f);
        }
        if(properSize != transform.localScale.y && !Physics.Raycast(transform.position, transform.up, 1.5f)){
            transform.localScale = new Vector3(1f, Mathf.SmoothStep(transform.localScale.y, properSize, 25f * Time.deltaTime), 1f);
        }
        if (!climb && !isGrounded && !slope)
        {
            Vector3 vel = myRb.velocity;
            vel.x *= AirDragXZ;
            vel.z *= AirDragXZ;
            myRb.velocity = vel;
        }
        if (myRb.velocity.magnitude > maxVelocity)
        {
            Vector3 vel = myRb.velocity;
            vel.x *= maxVelocity / myRb.velocity.magnitude;
            vel.z *= maxVelocity / myRb.velocity.magnitude;
            vel.y *= vel.y > 0 ? maxVelocity / myRb.velocity.magnitude :1;
            myRb.velocity = vel;
        }
        if ((myRb.velocity.x != 0 || myRb.velocity.z != 0)&&myRb.velocity.magnitude>1f)
        {
            if (isSliding) {
                if (SFX.clip != slide)
                {
                    SFX.clip = slide;
                    SFX.loop = true;
                    SFX.Play();
                }
            }else if (isGrounded)
            {
                if (SFX.loop)
                    SFX.loop = false;
                if(SFX.clip == null || !SFX.isPlaying)
                {
                    SFX.clip = steps[Random.Range(0, steps.Length)];
                    SFX.Play();
                }
            }else if (SFX.clip != null)
            {
                SFX.Stop();
                if (SFX.loop)
                    SFX.loop = false;
                SFX.clip = null;
            }
        }
        else if (SFX.clip != null)
        {
            SFX.Stop();
            if (SFX.loop)
                SFX.loop = false;
            SFX.clip = null;
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

    List<ContactPoint> contactPoints = new List<ContactPoint>();
    List<int> contactId = new List<int>();

    private void OnCollisionEnter(Collision collision)
    {
        var angle = Vector3.SignedAngle(collision.contacts[0].normal, Vector3.up, Vector3.forward);
        if(angle >= 0.0f && angle <= 16.0f){
            isGrounded=true;
            ClimbForce = 0;
            if (!wasGrounded)
            {
                slope = false;
                climb = false;
            }
            contactPoints.Add(collision.contacts[0]);
            contactId.Add(collision.gameObject.GetInstanceID());
        } else if(angle > 16.0f && angle < 70.0f) {
            slope = true;
            slopeC = collision;
            ClimbForce = (Mathf.Abs(lastVel.z*lastVel.x) / divider2) - Physics.gravity.y;
            StartCoroutine(InterpolateStick(startSSpeed,targetSSpeed,SspeedUpTime));
            contactPoints.Add(collision.contacts[0]);
            contactId.Add(collision.gameObject.GetInstanceID());
        } else {
            climb = true;
            ClimbForce = (lastVel.z/divider) - Physics.gravity.y;
            contactPoints.Add(collision.contacts[0]);
            contactId.Add(collision.gameObject.GetInstanceID());
        }
    }
    Collision slopeC;

    private void OnCollisionStay(Collision collision)
    {
        var angle = Vector3.SignedAngle(collision.contacts[0].normal, Vector3.up, Vector3.forward);
        if(angle >= 0f && angle <= 16f){
            wasGrounded = true;
            isGrounded = true;
        } else if(angle > 16f && angle < 70f) {
            slopeC = collision;
            myRb.AddForce(Vector3.Cross(transform.right,collision.contacts[0].normal) * ClimbForce*val.y*stickiness, ForceMode.Force);
            Debug.DrawRay(transform.position,Vector3.Cross(transform.right, collision.contacts[0].normal) * ClimbForce * val.y * stickiness);

        } else {
            myRb.AddForce(transform.forward*val.y*5, ForceMode.Force);
            Debug.DrawRay(transform.position, transform.forward * val.y*5);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var collisionIndex = contactId.FindIndex((x) =>  x == collision.gameObject.GetInstanceID());
        var contactPoint = contactPoints[collisionIndex];
        var angle = Vector3.SignedAngle(contactPoint.normal, Vector3.up, Vector3.forward);
        if(angle >= 0f && angle <= 16f){
            isGrounded = false;
            wasGrounded = false;
            if (climb)
            {
                StartCoroutine(RotateInTime(-45, rotator, Space.Self, 0.25f));
                StartCoroutine(RotateInTime(45 * 0.9f, Camholder, Space.Self, 0.25f));
            }
            contactPoints.RemoveAt(collisionIndex);
            contactId.RemoveAt(collisionIndex);
        } else if(angle > 16f && angle < 70f) {
            slope = false;
            contactPoints.RemoveAt(collisionIndex);
            contactId.RemoveAt(collisionIndex);

        } else {
            climb = false;
            
            /*transform.localEulerAngles = Vector3.zero;
            Camholder.localEulerAngles = Vector3.zero;*/
            StartCoroutine(RotateInTime(0, rotator, Space.Self, 0.2f));
            StartCoroutine(RotateInTime(0, Camholder, Space.Self, 0.2f, ResetRot));
            contactPoints.RemoveAt(collisionIndex);
            contactId.RemoveAt(collisionIndex);
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
        if(isGrounded || slope || climb){
            myRb.AddForce(transform.up * jumpUpForce + MyCamera.forward * jumpForwardForce, ForceMode.Force);
        }
    }

    private void Slide(float size) {
        properSize = size;
        if(size == .5f){
            myRb.mass = .1f;
            isSliding = true;
            myRb.AddForce(MyCamera.forward * jumpForwardForce, ForceMode.Force);
        } else {
            myRb.mass = 1f;
            isSliding = false;
        }
    }
}
