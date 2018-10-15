using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKhandler : MonoBehaviour {


    Animator anim;

    public float ikWeight = 1;

    public Transform leftIKTarget;
    public Transform rightIKTarget;

    private GameObject leftIKObject;
    private GameObject rightIKObject;

    public Transform hintLeft;
    public Transform hintRight;

    private GameObject rightHintObj;
    private GameObject leftHintObj;

    Vector3 lFPos;
    Vector3 rFPos;

    Quaternion lFRot;
    Quaternion rFRot;

    float lFWeight;
    float rFWeight;

    Transform leftFoot;
    Transform rightFoot;

    public float offsetY;

    public float lookIKweight;
    public float bodyWeight;
    public float headWeight;
    public float eyesWeight;
    public float clampWeight;

    private GameObject firstPersonCamera;

    public Transform lookPos;
    private Ray lookAtRay;

	// Use this for initialization
	void Start ()
    {

        anim = GetComponent<Animator>();

        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);

        lFRot = leftFoot.rotation;
        rFRot = rightFoot.rotation;
        firstPersonCamera = GameObject.FindWithTag("FirstPersonCamera");
        //lookAtRay = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);

        //Debug.DrawRay(firstPersonCamera.transform.position, firstPersonCamera.transform.forward * 15);
       // lookPos.position = new Vector3(0,0,0); 
        //Debug.Log(lookAtRay.GetPoint(15));
        //Debug.Log("LookPosition", lookPos);

    }
	
	// Update is called once per frame
	void Update ()
    {

        Debug.DrawRay(firstPersonCamera.transform.position, firstPersonCamera.transform.forward * 15);
        lookAtRay = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
        

        //Debug.Log(lookAtRay.GetPoint(15));

        lookPos.position = lookAtRay.GetPoint(15);
        // Debug.Log("LookPosition", lookPos);

        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 lpos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rpos = rightFoot.TransformPoint(Vector3.zero);

        if(Physics.Raycast(lpos, -Vector3.up, out leftHit, 1))
        {
            lFPos = leftHit.point;
            lFRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }

        if(Physics.Raycast(rpos, -Vector3.up, out rightHit, 1))
        {
            rFPos = rightHit.point;
            rFRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }

        leftIKObject = GameObject.FindGameObjectWithTag("gunIkLeft");
        rightIKObject = GameObject.FindGameObjectWithTag("gunIkRight");

        leftIKTarget = leftIKObject.transform;
        rightIKTarget = rightIKObject.transform;

        leftHintObj = GameObject.FindGameObjectWithTag("LeftHint");
        rightHintObj = GameObject.FindGameObjectWithTag("RightHint");

        hintLeft = leftHintObj.transform;
        hintRight = rightHintObj.transform;





    }

    void OnAnimatorIK()
    {

        anim.SetLookAtWeight(lookIKweight, bodyWeight, headWeight, eyesWeight, clampWeight);
        anim.SetLookAtPosition(lookPos.position);
        #region

       /* //Feet IK
        lFWeight = anim.GetFloat("LeftFoot");
        rFWeight = anim.GetFloat("RightFoot");

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lFWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rFWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFPos + new Vector3(0, offsetY, 0));
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rFPos + new Vector3(0, offsetY, 0));

        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, lFWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rFWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFRot);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rFRot);*/

        //hands
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftIKTarget.position) ;
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightIKTarget.position);

       // Debug.DrawRay(anim.GetBoneTransform(HumanBodyBones.RightHand).position, rightIKTarget.position);
        //Debug.Log(anim.GetBoneTransform(HumanBodyBones.RightHand).position);
       // Debug.Log(rightIKTarget.position);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftIKTarget.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightIKTarget.rotation);

        anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ikWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ikWeight);

        anim.SetIKHintPosition(AvatarIKHint.LeftElbow, hintLeft.position);
        anim.SetIKHintPosition(AvatarIKHint.RightElbow, hintRight.position);



        #endregion

        #region
        //Hand IKS
        /*anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftIKTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightIKTarget.position);

        anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ikWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ikWeight);

        anim.SetIKHintPosition(AvatarIKHint.LeftElbow, hintLeft.position);
        anim.SetIKHintPosition(AvatarIKHint.RightElbow, hintRight.position);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftIKTarget.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightIKTarget.rotation);*/
        #endregion



    }
}
