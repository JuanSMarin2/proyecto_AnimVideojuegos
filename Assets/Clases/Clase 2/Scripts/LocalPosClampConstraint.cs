using UnityEngine; 
using UnityEngine.Animations.Rigging;

namespace Assets.Clases.Clase_2.Scripts
{
    [AddComponentMenu("Animation Rigging/Custom/Local Pos Clamp Constraint")]
   public class LocalPosClampConstraint: RigConstraint<LocalPosClampJob, LocalPosClamptData, LocalPosClamptBinder>
    {
        
    }

}
