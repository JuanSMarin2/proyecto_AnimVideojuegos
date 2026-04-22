using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Clases.Clase_2.Scripts
{
    internal class AttackHitBox : MonoBehaviour, IDamageSender<DamageMessage>
    {
        [SerializeField] private DamageMessage damageMessage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageReceiver<DamageMessage> receiver))
            {
                SendDamage(receiver);
            }
        }

        public void SendDamage(IDamageReceiver<DamageMessage> receiver)
        {

            receiver.ReceiveDamage(damageMessage);
        }
    }
}
