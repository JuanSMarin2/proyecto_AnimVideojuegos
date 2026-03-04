using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts_4
{
    internal class reachTestKey: MonoBehaviour
    {
        public Animator animator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Reach");
            }
        }
    }
}
