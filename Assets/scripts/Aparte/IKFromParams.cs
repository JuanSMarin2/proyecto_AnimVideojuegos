using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Animator))]
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
    private string pRHPos = "RH_IK";
    private string pRHRot = "RH_IKRot";
    private string pRHHint = "RH_Hint";
    private string pLH = "LH_IK"; //intentando arreglar idk

    [Header("Pesos manuales")]
    [Range(0, 1)] public float RHPos = 1, RHRot = 1, LHPos = 1, LHRot = 1;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /*private void OnAnimatorIK(int layerIndex)
    {
        if (!animator) return;

        float wLook = readFromAnimator ? animator.GetFloat(pLook) : Look;
        float wRHPos = readFromAnimator ? animator.GetFloat(pRHPos) : RHPos;
        float wRHRot = readFromAnimator ? animator.GetFloat(pRHRot) : RHRot;
        float wRHHnt = readFromAnimator ? animator.GetFloat(pRHHint) : RHHint;
        float wLH = readFromAnimator ? animator.GetFloat(pLH) : LH;

        if (lookTarget)
        {
            animator.SetLookAtWeight(wLook);
            animator.SetLookAtPosition(lookTarget.position);
        }
        else
        {
            animator.SetLookAtWeight(0);
        }

        if (rightHandTarget)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, wRHPos);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, wRHRot);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }

        if (rightElbowHint)
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, wRHHnt);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);
        }
        else animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
    }*/

}
