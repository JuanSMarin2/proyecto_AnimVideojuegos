using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCrouch : MonoBehaviour, ICharacterComponent
{
    public Character ParentCharacter { get; set; }

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject crouchImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ParentCharacter.IsCrouching = false;
    }


    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        ParentCharacter.IsCrouching = !ParentCharacter.IsCrouching;
        animator.SetBool("isCrouching", ParentCharacter.IsCrouching);

        if (crouchImage != null)
        {
            crouchImage.SetActive(ParentCharacter.IsCrouching);

        }
    }
}
