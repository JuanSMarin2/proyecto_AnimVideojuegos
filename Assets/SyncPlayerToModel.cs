using UnityEngine;

public class SyncPlayerToModel : MonoBehaviour
{
    [SerializeField] private Transform model;

    private Vector3 lastModelPosition;

    private void Start()
    {
        lastModelPosition = model.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = model.position - lastModelPosition;

        transform.position += delta;
        transform.rotation = model.rotation;

        lastModelPosition = model.position;


        model.localPosition = Vector3.zero;
        model.localRotation = Quaternion.identity;
    }
}