using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Entrega_3.Esteban.Scripts
{
    public interface IDamageSender<TDamage> where TDamage : struct
    {
        void SendDamage(IDamageReceiver<TDamage> receiver);
    }
}
