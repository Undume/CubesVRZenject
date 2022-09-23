using SharedUtils.UpdateManager;
using UnityEngine;

namespace ShootBoxes.Core
{
    public class TimeController : UpdateManagedMonoBehaviour
    {
        public float AccumulatedTime => m_accumulatedTime;

        private float m_accumulatedTime;

        public void RestartTime()
        {
            m_accumulatedTime = 0;
        }

        public override void UpdateMe(float scaledTime)
        {
            m_accumulatedTime += Time.deltaTime * scaledTime;
        }
    }
}