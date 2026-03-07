using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    public class CharacterAnimatiorControler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterInputFactori.InputType inputType = CharacterInputFactori.InputType.Player;



        private ICgharacterInput _input;
        private CharacterAnimator _characterAnimator;

        private void Awake()
        {
            _input = CharacterInputFactori.CreateInput(inputType);
            _characterAnimator = new CharacterAnimator(animator);
        }

        private void Update()
        {
            float speed = _input.GetSpeedInput();
            _characterAnimator.UpdateSpeed(speed);
        }
    }
}
