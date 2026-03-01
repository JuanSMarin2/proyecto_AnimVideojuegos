using Assets.Clases.Clase_2.Scripts;
using System;
using System.Xml.Serialization;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace Assets.Clases.Clase_2.Scripts
{
    internal class RecoilCameraKick : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera[] _cameras;
        private CinemachineBasicMultiChannelPerlin[] perlins;
        private float[] baseAmplitud;

        private void Awake()
        {
            perlins = new CinemachineBasicMultiChannelPerlin[_cameras.Length];
            baseAmplitud = new float[_cameras.Length];

            for(int i = 0; i < _cameras.Length; i++)
            {
                if (!_cameras[i]) continue;
                perlins[i] = _cameras[i].GetComponent<CinemachineBasicMultiChannelPerlin>();
                if(!perlins[i]) baseAmplitud[i] = perlins[i].AmplitudeGain;
            }
        }
    }
}
