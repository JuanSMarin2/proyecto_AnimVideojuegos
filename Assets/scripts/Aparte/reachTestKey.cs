using UnityEngine;

namespace Assets.scripts.Aparte
{
    public class reachTestKey : MonoBehaviour
    {
        public Animator animator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger(name: "Reach");
            }
        }




    }
}
