using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts2
{
    public interface ICharacterComponent
    {
        Character ParentCharacter { get; set; }
    }
}
