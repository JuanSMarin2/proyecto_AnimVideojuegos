using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Clases.Clase_2.Scripts
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
