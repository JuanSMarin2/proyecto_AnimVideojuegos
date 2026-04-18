using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Clases.Clase_2.Scripts
{
    internal class AttackHitBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject[] hitBoxes;

        public void ToggleHitboxes(int attackId)
        {

            for (int hitBoxIndex = 0; hitBoxIndex < hitBoxes.Length; hitBoxIndex++)
            {
                GameObject hitBox = this.hitBoxes[hitBoxIndex];
                hitBox.SetActive(!hitBox.activeSelf);
            }

        }

        public void CleanupHitBoxes()
        {
            foreach (GameObject colliders in hitBoxes)
            {
                colliders.SetActive(false);
            }
        }
    }
}
