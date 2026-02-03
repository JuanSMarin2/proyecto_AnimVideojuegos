using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    public class CharacterAnimator
    {
        private readonly Animator _animator;
        private readonly int _speedHash =  Animator.StringToHash(name:"Speed");

        public CharacterAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void UpdateSpeed(float speed)
        {
            _animator.SetFloat(_speedHash, speed);
        }
    }
}
