using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public WheelColliders colliders;
    public WheelMeshes meshes;
    public float forward;
    public float turn;
    public float ps = 1000f;
    private float speed;
    private Rigidbody ribo;
    public AnimationCurve curve;
    public float breakhardness;
    public float slipAngle;
    public float breakPush;
    public float boost = 100;
    public float maxSlipAngle = 3;
    public ParticleSystem driftSmokeLeft;
    public ParticleSystem driftSmokeRight;
    public ParticleSystem boostLeft;
    public ParticleSystem boostRight;
    public float posXBoostLeft;//Position of Boost
    public float posXBoostRight;
    // Start is called before the first frame update
    void Start()
    {
        ribo = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshWheels();
        CheckKeys();
        UseSpeed();
        speed = ribo.velocity.magnitude;
        ApplyCurve();
        Break();
        Drift();
        DriftSmoke();
        BoostAnimation();

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void CheckKeys()
    {
        forward = Input.GetAxis("Vertical");//forward and backwards
        turn = Input.GetAxis("Horizontal");//Left and Right
        slipAngle = Vector3.Angle(transform.forward, ribo.velocity - transform.forward);//transform for moving backwards after breaking

        if (slipAngle < 120f)//breaking when car goes forward
        {
            if (forward < 0)
            {
                breakPush = Mathf.Abs(forward);//get value
                forward = 0f;
            }
        }
        else
        {
            breakPush = 0;
        }
    }
    void Break()
    {
        colliders.FrontLeftWheel.brakeTorque = breakPush * breakhardness;
        colliders.FrontRightWheel.brakeTorque = breakPush * breakhardness;
        colliders.RearLeftWheel.brakeTorque = breakPush * breakhardness;
        colliders.RearRightWheel.brakeTorque = breakPush * breakhardness;
    }
    void UseSpeed()
    {
        colliders.RearLeftWheel.motorTorque = ps * forward;
        colliders.RearRightWheel.motorTorque = ps * forward;

        if (Input.GetKey(KeyCode.Space))
        {
            colliders.RearLeftWheel.motorTorque = ps * forward * boost;
            colliders.RearRightWheel.motorTorque = ps * forward * boost;
        }       
    }
    void RefreshWheels()
    {
        UpdateWheel(colliders.FrontLeftWheel, meshes.FrontLeftWheel);
        UpdateWheel(colliders.FrontRightWheel, meshes.FrontRightWheel);
        UpdateWheel(colliders.RearLeftWheel, meshes.RearLeftWheel);
        UpdateWheel(colliders.RearRightWheel, meshes.RearRightWheel);
    }
    void UpdateWheel(WheelCollider col, MeshRenderer rend)
    {
        Quaternion qua;//rotation of the car
        Vector3 position;//Position of the car
        col.GetWorldPose(out position, out qua);
        rend.transform.position = position;
        rend.transform.rotation = qua;
    }
    void ApplyCurve()
    {
        float steeringAngle = turn * curve.Evaluate(speed);
        colliders.FrontLeftWheel.steerAngle = steeringAngle;
        colliders.FrontRightWheel.steerAngle = steeringAngle;
    }
    void Drift()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            WheelFrictionCurve drift;
            drift = colliders.FrontRightWheel.sidewaysFriction;
            drift.extremumSlip = 2f;
            colliders.FrontRightWheel.sidewaysFriction = drift;

            drift = colliders.FrontLeftWheel.sidewaysFriction;
            drift.extremumSlip = 2f;
            colliders.FrontLeftWheel.sidewaysFriction = drift;

            drift = colliders.RearLeftWheel.sidewaysFriction;
            drift.extremumSlip = 2f;
            colliders.RearLeftWheel.sidewaysFriction = drift;

            drift = colliders.RearRightWheel.sidewaysFriction;
            drift.extremumSlip = 2f;
            colliders.RearRightWheel.sidewaysFriction = drift;
        }
        else
        {
            WheelFrictionCurve drift;
            drift = colliders.FrontRightWheel.sidewaysFriction;
            drift.extremumSlip = 0.2f;
            colliders.FrontRightWheel.sidewaysFriction = drift;

            drift = colliders.FrontLeftWheel.sidewaysFriction;
            drift.extremumSlip = 0.2f;
            colliders.FrontLeftWheel.sidewaysFriction = drift;

            drift = colliders.RearLeftWheel.sidewaysFriction;
            drift.extremumSlip = 0.2f;
            colliders.RearLeftWheel.sidewaysFriction = drift;

            drift = colliders.RearRightWheel.sidewaysFriction;
            drift.extremumSlip = 0.2f;
            colliders.RearRightWheel.sidewaysFriction = drift;
        }
    }
    void DriftSmoke()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            driftSmokeLeft.transform.position = colliders.RearLeftWheel.transform.position;
            driftSmokeRight.transform.position = colliders.RearRightWheel.transform.position;
            driftSmokeLeft.Play();
            driftSmokeRight.Play();
        }
        else if (Input.GetKey(KeyCode.LeftShift) != true)
        {
            driftSmokeLeft.transform.position = new Vector3(10000, 10000);//TODO: hide Particle System and not changing the position
            driftSmokeRight.transform.position = new Vector3(10000, 10000);
        }
    }
    void BoostAnimation()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            boostLeft.transform.position = colliders.RearLeftWheel.transform.position;//TODO: set position of x axis to around minus 10
            boostRight.transform.position = colliders.RearRightWheel.transform.position;//TODO: set position of x axis to around minus 10
            boostLeft.Play();
            boostRight.Play();
        }
        else if(Input.GetKey(KeyCode.Space) != true)
        {
            boostLeft.transform.position = new Vector3(10000, 10000);//TODO: hide Particle System and not changing the position
            boostRight.transform.position = new Vector3(10000, 10000);
        }
    }
}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FrontLeftWheel;
    public MeshRenderer FrontRightWheel;
    public MeshRenderer RearLeftWheel;
    public MeshRenderer RearRightWheel;
}


