using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationController : NetworkBehaviour {

    [SerializeField]
    static Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

            if (PlayerMotor.walking == true)
            {
                anim.SetBool("isWalking", true);
                //Debug.Log("Walking");
            }
            else if (PlayerMotor.walking == false)
            {
                anim.SetBool("isWalking", false);
                // Debug.Log("IDle");
            }

            if (ShootingController.shooting == true)
            {
                anim.SetBool("isShooting", true);
                //Debug.Log("Shooting");
                //Debug.Log(ShootingController.shooting);
            }
            else if (ShootingController.shooting == false)
            {
                anim.SetBool("isShooting", false);

            }

            if (PlayerController1.isGrounded == false)
            {
                anim.SetBool("isJumping", true);
               // Debug.Log("Jumping");
            }
            else if (PlayerController1.isGrounded == true)
            {
                anim.SetBool("isJumping", false);
                //Debug.Log("Not Jumping");
            }

            if (PlayerController1.isRunning == true)
            {
                anim.SetBool("isRunning", true);
            }
            else if (PlayerController1.isRunning == false)
            {
                anim.SetBool("isRunning", false);
            }
	}
}
