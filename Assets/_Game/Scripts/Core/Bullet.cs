using System.Collections;
using SharedUtils;
using SharedUtils.UpdateManager;
using ShootBoxes.Constants;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public class Bullet : UpdateManagedMonoBehaviour
    {
        public Vector2 ForceRange;

        [SerializeField] private float m_immediateHitDistance = 0.5f;
        [SerializeField] private GameObject m_hitPrefab;
        [SerializeField] private ProjectileVFX m_projectileVFX;
        [SerializeField] private int m_maxBounces;
        [SerializeField] private float m_speed;
        [SerializeField] private float m_maxTime = 4.0f;
        [SerializeField] private int m_skippedFramesVFX = 2;

        [Inject] private PoolManager m_poolManager;

        private LayerMask m_hittableLayer;
        private float m_deactivateTimer;
        private int m_bounceCount = 0;
        private bool m_impacted = false;

        public void Activate(LayerMask hittableLayer)
        {
            m_hittableLayer = hittableLayer;
            m_projectileVFX.ToggleParticles(false);
            m_impacted = false;
            m_bounceCount = 0;
            m_deactivateTimer = 0;
            if (Physics.Raycast(transform.position, transform.forward, out var hit,
                    m_immediateHitDistance, m_hittableLayer.value))
            {
                m_projectileVFX.gameObject.SetActive(true);
                HandleHit(hit);
            }
            else
            {
                StartCoroutine(FirstFramesBulletVFXInvisible());
            }
        }

        public override void UpdateMe(float scaledTime)
        {
            if (m_impacted) return;
            var travelDistance = Time.deltaTime * m_speed * scaledTime;
            m_deactivateTimer += Time.deltaTime * scaledTime;
            if (Physics.Raycast(transform.position, transform.forward, out var hit, travelDistance,
                    m_hittableLayer.value))
            {
                HandleHit(hit);
            }
            else if (m_deactivateTimer > m_maxTime)
            {
                gameObject.SetActive(false);
                m_projectileVFX.ToggleParticles(false);
            }
            else transform.position += transform.forward * travelDistance;
        }

        private void HandleHit(RaycastHit hit)
        {
            var hittedTransform = hit.transform;
            if (hittedTransform.CompareTag(Tags.Hittable))
            {
                var hittable = hittedTransform.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.Hit(hit, this);
                    m_impacted = true;
                    gameObject.SetActive(false);
                    m_projectileVFX.ToggleParticles(false);
                }
            }
            else if (hittedTransform.CompareTag(Tags.BouncingSurface))
            {
                m_poolManager.Get(m_hitPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                if (m_bounceCount < m_maxBounces)
                {
                    BounceOnSurface(hit);
                }
                else
                {
                    m_impacted = true;
                    m_projectileVFX.ToggleParticles(false);
                    gameObject.SetActive(false);
                }
            }
        }

        private void BounceOnSurface(RaycastHit hit)
        {
            m_bounceCount++;
            transform.position += hit.distance * 0.99f * transform.forward;
            transform.forward = Vector3.Reflect(transform.forward, hit.normal);
        }

        private IEnumerator FirstFramesBulletVFXInvisible()
        {
            var counter = 0;
            while (counter < m_skippedFramesVFX)
            {
                counter++;
                yield return new WaitForEndOfFrame();
            }

            m_projectileVFX.ToggleParticles(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }
    }
}