using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKFromParams : MonoBehaviour
{
    [Header("Targets")]
    public Transform rightHandTarget;
    public Transform rightElbowHint;
    public Transform leftHandTarget;
    public Transform leftElbowHint;
    public Transform lookTarget;

    public bool readFromAnimator = true;

    public string pLook = "Look_IK";
    public string pRHPos = "RH_IK", pRHRot = "RH_IKRot", pLHPos = "LH_IKPos", pLHRot = "LH_IKRot";
    public string pRHHint = "RH_Hint", pLHHint = "LH_Hint";

    public string pLH = "LH_IK";

    [Header("Pesos manuales")]
    [Range(0, 1)] public float RHPos = 1, RHRot = 1, RHHint = 1 , LHPos = 1, LHRot = 1, LHHint = 1 , Look = 0.8f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(!animator) return;

        float wLook = readFromAnimator ? animator.GetFloat(pLook) : Look;
        float wRHPos = readFromAnimator ? animator.GetFloat(pRHPos) : RHPos;
        float wRHRot = readFromAnimator ? animator.GetFloat(pRHRot) : RHRot;
        float wRHHint = readFromAnimator ? animator.GetFloat(pRHHint) : RHHint;
        float wLHPos = readFromAnimator ? animator.GetFloat(pLHPos) : LHPos;
        float wLHRot = readFromAnimator ? animator.GetFloat(pLHRot) : LHRot;
        float wLHHint = readFromAnimator ? animator.GetFloat(pLHHint) : LHHint;

        if (lookTarget)
        {
            animator.SetLookAtWeight(wLook);
            animator.SetLookAtPosition(lookTarget.position);
        }
        else
        {
                       animator.SetLookAtWeight(0);
        }

        if(rightHandTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, wRHPos);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, wRHRot);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }

        if(rightElbowHint)
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, wRHHint);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);
        }
        else
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
        }

        if(leftHandTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, wLHPos);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, wLHRot);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

        if(leftElbowHint)
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, wLHHint);
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowHint.position);
        }
        else
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
        }
    }

}
