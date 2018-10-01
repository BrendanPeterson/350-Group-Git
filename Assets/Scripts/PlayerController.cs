
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
//Require Component automatically adds the component to the game object
public class PlayerController : MonoBehaviour {

    //tells unity to serialize the data even though it is private
    //serializing tells unity to make a disc representation which can be recreated later
    //Basically lets you show private vars, floats, etc.. in the inspector
    [SerializeField]
    private LayerMask environmentMask;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float horizontalSensitivity = 10f;

    [SerializeField]
    private float verticalSensitivity = 10f;

    [SerializeField]
    private float thrusterForce = 5000f;

    [SerializeField]
    private float thrsuterFuelRegenSpeed = 0.5f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;

    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }
    //Spring makes the character appear to be floating 
    //Also makes the charcater bounce when landing
    [Header("Springy Settings")]

    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    void Update()
    {

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

        //Calculate how high the player should float above ground
        RaycastHit _hit;
            if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
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

        if(Input.GetButton("Jump" )&& thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount >= 0.02f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            
        }else
        {
            thrusterFuelAmount += thrsuterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        //Apply Thruster Force
        motor.ApplyThruster(_thrusterForce);
        
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
