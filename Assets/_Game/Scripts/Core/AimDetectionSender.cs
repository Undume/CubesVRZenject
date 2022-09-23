using System.Collections.Generic;
using SharedUtils.UpdateManager;
using UnityEngine;

namespace ShootBoxes.Core
{
    public class AimDetectionSender : UpdateManagedMonoBehaviour
    {
        [SerializeField] private float m_maxDistance = 100f;
        [SerializeField] private LayerMask m_layerMask;

        private Transform m_lastTransform;
        private IAimedDetector m_lastAimed;

        void Update()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                    m_maxDistance,
                    m_layerMask))
            {
                if (hit.transform != m_lastTransform)
                {
                    m_lastTransform = hit.transform;
                    m_lastAimed?.UnAimed();
                    m_lastAimed = hit.transform.GetComponentInParent<IAimedDetector>();
                    m_lastAimed.Aimed();
                }
            }
            else if (m_lastTransform)
            {
                m_lastTransform = null;
                m_lastAimed.UnAimed();
                m_lastAimed = null;
            }
        }
    }
}