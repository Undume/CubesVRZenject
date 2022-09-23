using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootBoxes.Core
{
    public class RandomSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip[] m_impactClips;
        [SerializeField] private AudioSource m_source;

        public void PlayRandomSFX()
        {
            if (m_source == null || m_source.isPlaying) return;
            m_source.clip = m_impactClips[Random.Range(0, m_impactClips.Length)];
            m_source.Play();
        }
    }
}