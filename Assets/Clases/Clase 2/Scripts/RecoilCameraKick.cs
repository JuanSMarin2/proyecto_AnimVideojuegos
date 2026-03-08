using Assets.Clases.Clase_2.Scripts;
using System;
using System.Xml.Serialization;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using System.Collections;

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
                if(perlins[i]) baseAmplitud[i] = perlins[i].AmplitudeGain;
            }
        }

        public void Kick(float peak, float strength, float recover)
        {
            StopAllCoroutines();
            StartCoroutine(routine: KickCoroutine(peak, strength, recover));
        }

        IEnumerator KickCoroutine(float peak, float strength, float recover)
        {
            float t = 0f;
            while (t < peak)
            {
                t += Time.deltaTime;
                float k = t / Mathf.Max(0.0001f, peak);

                for (int i = 0; i < perlins.Length; i++)
                {
                    if (perlins[i]) perlins[i].AmplitudeGain = Mathf.Lerp(baseAmplitud[i], baseAmplitud[i] + strength, k);
                }
                yield return null; // <-- Esto es esencial
            }

            t = 0f;

            while (t < recover)
            {
                t += Time.deltaTime;
                float k = t / Mathf.Max(0.0001f, recover);
                for (int i = 0; i < perlins.Length; i++)
                {
                    if (perlins[i]) perlins[i].AmplitudeGain = Mathf.Lerp(baseAmplitud[i] + strength, baseAmplitud[i], k);
                }
                yield return null; // <-- Esto es esencial
            }

            for (int i = 0; i < perlins.Length; i++)
            {
                if (perlins[i]) perlins[i].AmplitudeGain = baseAmplitud[i];
            }
        }
    }
}
