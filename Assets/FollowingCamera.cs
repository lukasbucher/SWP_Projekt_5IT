using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform car;
    public Rigidbody ribo;
    public Vector3 difference;//difference between camera and car
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        ribo = car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()//LateUpdate to make the CameraMovement smoother
    {
        Vector3 forward = (ribo.velocity + car.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position, car.position + car.transform.TransformVector(difference) + forward * (-5f), speed * Time.deltaTime);
        transform.LookAt(car);
    }
}

