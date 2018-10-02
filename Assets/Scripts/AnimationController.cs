using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    static Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerMotor.walking == true)
        {
            anim.SetBool("isWalking", true);
            Debug.Log("Walking");
        }
        else if(PlayerMotor.walking == false)
        {
            anim.SetBool("isWalking", false);
           // Debug.Log("IDle");
        }

        if (ShootingController.shooting == true)
        {
            anim.SetBool("isShooting", true);
            Debug.Log("Shooting");
            Debug.Log(ShootingController.shooting);
        }
        else if (ShootingController.shooting == false)
        {
            anim.SetBool("isShooting", false);
            
        }
	}
}
