using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Entrega_3.Esteban.Scripts
{
    internal class DamageReceiver: MonoBehaviour, IDamageReceiver<float>
    {
        public void ReceiveDamage(float damage)
        {
            Debug.Log($"Received {damage} damage.");
        }
    }
}
