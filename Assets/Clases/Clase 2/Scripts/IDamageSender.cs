using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Clases.Clase_2.Scripts
{
    public interface IDamageSender<TDamage> where TDamage : struct
    {
        void SendDamage(IDamageReceiver<TDamage> receiver);
    }
}
