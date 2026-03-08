using UnityEngine;
using UnityEngine.InputSystem;

public class EmoteController : MonoBehaviour
{
    public Animator animator;

    public string emoteTrigger = "Emote";
    public string shootingState = "Shoot";
    public string reloadState = "Reload";

    void Awake()
    {
        Debug.Log("EmoteController inicializado.");
    }

    public void OnEmote(InputAction.CallbackContext ctx)
    {
        Debug.Log("Input de Emote recibido.");

        if (!ctx.performed)
        {
            Debug.Log("El input no está en estado 'performed'. Se ignora.");
            return;
        }

        Debug.Log("Input Emote confirmado (performed).");

        if (CanPlayEmote())
        {
            Debug.Log("Se puede reproducir el Emote. Activando trigger.");
            animator.SetTrigger(emoteTrigger);
        }
        else
        {
            Debug.Log("No se puede reproducir el Emote porque el personaje está en un estado bloqueado.");
        }
    }

    bool CanPlayEmote()
    {
        Debug.Log("Verificando si el Emote puede ejecutarse...");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        string currentState = stateInfo.IsName(shootingState) ? shootingState :
                              stateInfo.IsName(reloadState) ? reloadState :
                              "Otro estado";

        Debug.Log("Estado actual del Animator: " + currentState);

        if (stateInfo.IsName(shootingState))
        {
            Debug.Log("Bloqueado: el personaje está disparando.");
            return false;
        }

        if (stateInfo.IsName(reloadState))
        {
            Debug.Log("Bloqueado: el personaje está recargando.");
            return false;
        }

        Debug.Log("No hay estados bloqueantes. Emote permitido.");
        return true;
    }
}