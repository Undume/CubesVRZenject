using UnityEngine;

namespace ShootBoxes.Core
{
    public interface IHittable
    {
        void Hit(RaycastHit hit, Bullet bullet);
    }
}