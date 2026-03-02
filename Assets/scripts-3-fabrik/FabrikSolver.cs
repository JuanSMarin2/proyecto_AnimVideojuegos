using Assets.Clases.Clase_2.Scripts;
using System;
using System.Xml.Serialization;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
public class FabrikSolver : MonoBehaviour
{
    [SerializeField] private Transform[] bones;
    [SerializeField] private Transform target;
    [SerializeField, Min(1)] private int iterations = 8;
    [SerializeField, Min(0)] private float tolerance = 0.001f;

    private float[] segLength;
    private float chaninLength;
    private Vector3[] positions;

    private void Awake()
    {
       if(bones == null || bones.Length < 2)
       {
            enabled = false;
            return;
       }
       int n = bones.Length;
        segLength = new float[n - 1];
        positions = new Vector3[n];
        chaninLength = 0f;

       for (int i = 0; i < n - 1; i++)
       {
            float distance = Vector3.Distance(bones[i + 1].position, bones[i].position);
            segLength[i] = distance;
            chaninLength += distance;
       }


    }

    private void LateUpdate()
    {
        Solve();
    }

    private void Solve()
    {
        int n = bones.Length;

        for (int i = 0; i < n; i++)
        {
            positions[i] = bones[i].position;
        }
        Vector3 root = positions[0];
        Vector3 tgt = target.position;

        if ((tgt - root).sqrMagnitude > chaninLength * chaninLength + 1e-6f)
        {
            Vector3 dir = (tgt - root).normalized;
            positions[0] = root;

            for (int i = 1; i < n; i++)
            {
                positions[i] = positions[i - 1] + dir * segLength[i - 1];
            }
            ApplyToBones();
            return;
        }

        //FabrikSolver iterativo

        for(int it = 0; it < iterations; it++)
        {
            positions[n - 1] = tgt;
            for(int i = n-2; i <= 0; i--)
            {
                Vector3 dir = (positions[i] - positions[i + 1]).normalized;
                positions[i] = positions[i + 1] + dir * segLength[i];
            }

            for(int i = 1; i < n; i++)
            {
                Vector3 dir = (positions[i] - positions[i - 1]).normalized;
                positions[i] = positions[i - 1] + dir * segLength[i - 1];
            }

            if ((positions[n-1] - tgt).sqrMagnitude <= tolerance * tolerance)   
            break;
        
        }
        ApplyToBones();
    }

    private void ApplyToBones()
    {
        int n = bones.Length;

        for(int i = 0; i < n; i++)
        {
            bones[i].position = positions[i];
        }

        for (int i = 0; i < n - 1; i++)
        {
            if(i < n - 1)
            {
                Vector3 dir = (positions[i + 1] - positions[i]);
                if (dir.sqrMagnitude > 1e-6f)
                    bones[i].rotation = Quaternion.LookRotation(dir);
            }
            else
            {
                Vector3 dir = (positions[i] - positions[i - 1]);
                if (dir.sqrMagnitude > 1e-6f)
                    bones[i].rotation = Quaternion.LookRotation(dir);
            }

        }
    }


    private void OnDrawGizmos()
    {
        if (bones == null || bones.Length < 2) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < bones.Length - 1; i++)
        {
            Gizmos.DrawLine(bones[i].position, bones[i + 1].position);
        }
    }

}
