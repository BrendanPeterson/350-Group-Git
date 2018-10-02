using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

//Using require component will automatically add the type of required component to the gameobject
public class PlayerMotor : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 90f;

    private Rigidbody rb;

    public static bool walking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets velocity from player controller and passes it in
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Gets a rotation for the player
    public void Rotate (Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //Gets a rotation for the camera
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //Gets a thrust from the PlayerController
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    void FixedUpdate()
    {
        PerformMovement();
       
    }

    void Update()
    {
        PerformRotation();
    }

    //Perform movement based on velocity
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            //will stop the rigidbody if it collides with an object
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            walking = true;
            //Debug.Log("Motor Walking");
        }
        else
        {
            walking = false;
        }
     

        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    //Perform rotation
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            currentCameraRotationX -= cameraRotationX;
            
            //Limit camera rotation
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply rotation to transform of camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }
    }

}
