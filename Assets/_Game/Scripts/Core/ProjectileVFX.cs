using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharedUtils;
using UnityEngine;

namespace ShootBoxes.Core
{
    public class ProjectileVFX : MonoBehaviour
    {
        private ParticleSystem[] m_particles;
        private TrailRenderer m_trailRenderer;

        private void Awake()
        {
            m_particles = GetComponentsInChildren<ParticleSystem>();
            m_trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        private void Start()
        {
            ToggleParticles(false);
        }

        public void ToggleParticles(bool shouldPlay)
        {
            foreach (var particle in m_particles)
            {
                if (shouldPlay)
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                    particle.Clear();
                }
            }

            m_trailRenderer.Clear();
        }
    }
}