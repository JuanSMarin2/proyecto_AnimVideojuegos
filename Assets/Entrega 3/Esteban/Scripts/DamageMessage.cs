using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Entrega_3.Esteban.Scripts
{
    [Serializable]
    public struct DamageMessage
    {
        public enum DamageLevel
        {
            Small,
            Medium,
            Big
        }
        public GameObject sender;
        public float amount;
        public DamageLevel damageLevel;
    }
}
