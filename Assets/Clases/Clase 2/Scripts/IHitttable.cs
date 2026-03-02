using UnityEngine; // 👈 IMPORTANTE

namespace Assets.Clases.Clase_2.Scripts
{
    public static class IHitttable   // 👈 También lo hacemos static (explico abajo)
    {
        public interface IHittable
        {
            void ApplyHit(HitInfo info);
        }

        public struct HitInfo
        {
            public Vector3 point;
            public Vector3 normal;
            public float damage;
        }
    }
}
