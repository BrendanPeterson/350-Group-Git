
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(PlayerMotor))]
//Require Component automatically adds the component to the game object
public class PlayerController1 : NetworkBehaviour {

    //tells unity to serialize the data even though it is private
    //serializing tells unity to make a disc representation which can be recreated later
    //Basically lets you show private vars, floats, etc.. in the inspector
    [SerializeField]
    private LayerMask environmentMask;

    [SerializeField]
    private float startSpeed = 2f;
    private float speed = 2f;

    [SerializeField]
    private float horizontalSensitivity = 10f;

    [SerializeField]
    private float verticalSensitivity = 10f;

    [SerializeField]
    private float jumpForce = 500f;

   // [SerializeField]
    //private float thrsuterFuelRegenSpeed = 0.5f;

    //[SerializeField]
   // private float thrusterFuelBurnSpeed = 1f;

   // private float thrusterFuelAmount = 1f;

    public CapsuleCollider playerCol;
    public LayerMask groundLayers;

    public static bool isGrounded;
    public static bool isRunning;
    

   /* public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }*/


    private PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        playerCol = GetComponent<CapsuleCollider>();
       
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Run"))
        {
            speed = startSpeed * 2;
            isRunning = true;
        }

        if (Input.GetButtonUp("Run"))
        {
            speed = startSpeed;
            isRunning = false;
        }
        
        if (PauseMenu.IsOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);

            return;
        }
            

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //calculate movement velocity as a 3D vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov; 
        Vector3 movVertical = transform.forward * zMov;

        //Final Movement Vector
        Vector3 _velocity = (movHorizontal + movVertical).normalized * speed;

        //Apply movement
        //Takes our velocity above and sends it to motor script
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector
        //Lets us turn the player with the mouse
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * horizontalSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector
        //Lets us turn the camera with the mouse
        float xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = xRot * verticalSensitivity;

        //Apply rotation
        motor.RotateCamera(_cameraRotationX);

        //Calculate thruster force based on input
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump") && isGrounded)
        {

            _thrusterForce = Vector3.up * jumpForce;
            isGrounded = false;

        }

        //Apply Thruster Force
        motor.ApplyThruster(_thrusterForce);

    }

     void FixedUpdate()
    {
        //Debug.Log(isGrounded);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

}
