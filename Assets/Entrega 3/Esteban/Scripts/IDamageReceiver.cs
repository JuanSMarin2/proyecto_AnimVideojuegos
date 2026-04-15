using UnityEngine;

namespace Assets.Entrega_3.Esteban.Scripts
{
    public interface IDamageReceiver<TDamage> where TDamage : struct
    {
        void ReceiveDamage(TDamage damage);
    }
}
