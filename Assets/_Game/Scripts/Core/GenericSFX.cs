using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace ShootBoxes.Core
{
    public class GenericSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;

        public void Play(bool force = false, float delay = -0.1f)
        {
            if (m_audioSource == null || m_audioSource.isPlaying && !force) return;
            if (delay > 0.0) m_audioSource.PlayDelayed(delay);
            else m_audioSource.Play();
        }
        
        public void PlayUI()
        {
            Play();
        }
    }
}