using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ShootBoxes.Core
{
    public class AutomaticSingleVFX : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnable;
        private ParticleSystem[] m_particleSytems;
        private WaitForSeconds m_waitForSeconds;
        private bool m_firstOnEnable = true;

        private void Awake()
        {
            m_particleSytems = GetComponentsInChildren<ParticleSystem>();
            var longestDuration = 0f;
            foreach (var particle in m_particleSytems)
            {
                if (particle.main.duration > longestDuration) longestDuration = particle.main.duration;
            }

            m_waitForSeconds = new WaitForSeconds(longestDuration);
        }

        private void OnEnable()
        {
            if (m_firstOnEnable)
            {
                m_firstOnEnable = false;
                return;
            }

            foreach (var particle in m_particleSytems)
            {
                particle.Play();
            }

            StartCoroutine(DeactivateFX());
            onEnable.Invoke();
        }

        private IEnumerator DeactivateFX()
        {
            yield return m_waitForSeconds;
            yield return null;
            gameObject.SetActive(false);
        }
    }
}