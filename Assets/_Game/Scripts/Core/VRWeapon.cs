using SharedUtils;
using SharedUtils.UpdateManager;
using ShootBoxes.Constants;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    [RequireComponent(typeof(VRGrabbable))]
    public class VRWeapon : UpdateManagedMonoBehaviour
    {
        [Inject] private IAppManagement m_appManager;
        [Inject] private PoolManager m_poolManager;

        [SerializeField] private float m_cooldownShoot;
        [SerializeField] private ParticleSystem m_shootFlash;
        [SerializeField] private GenericSFX m_shootSFX;
        [SerializeField] private Transform m_shootingPoint;

        [Header("Bullet prefabs")] [SerializeField]
        private GameObject m_bulletPrefab;

        [SerializeField] private GameObject m_pausedBulletPrefab;

        [Header("Shootable layers")] [SerializeField]
        private LayerMask m_NormalShootingMode;

        [SerializeField] private LayerMask m_UIVRShotingMode;
        [Header("Laser")] [SerializeField] private GameObject m_laser;
        [SerializeField] private GenericSFX m_turnOnLaserSFX;

        private LayerMask m_currentHittableLayer;
        private VRGrabbable m_vrGrabbable;
        private float m_cooldownShootTimer;

        private void Awake()
        {
            m_vrGrabbable = GetComponent<VRGrabbable>();
            m_currentHittableLayer = m_NormalShootingMode;
        }

        public void SetUIShootingMode() => m_currentHittableLayer = m_UIVRShotingMode;
        public void SetNormalShootingMode() => m_currentHittableLayer = m_NormalShootingMode;

        public override void UpdateMe(float speedTime)
        {
            m_cooldownShootTimer += Time.deltaTime * speedTime;
        }

        public void OnTrigger()
        {
            if (m_cooldownShootTimer > m_cooldownShoot || m_appManager.CurrentScene() == Enums.GameScene.Pause)
            {
                m_shootSFX.Play(true);
                m_cooldownShootTimer = 0;
                Shoot();
                ShootingVfx();
                Haptics();
            }
        }

        public void ToggleLaser()
        {
            if (m_laser.activeSelf)
            {
                m_laser.SetActive(false);
            }
            else
            {
                m_laser.SetActive(true);
                m_turnOnLaserSFX.Play(true);
            }
        }

        private void Shoot()
        {
            Bullet bullet = null;
            if (m_appManager.CurrentScene() == Enums.GameScene.Pause)
                bullet = m_poolManager.Get(m_pausedBulletPrefab, m_shootingPoint.position, m_shootingPoint.rotation)
                    .GetComponent<Bullet>();
            else
                bullet = m_poolManager.Get(m_bulletPrefab, m_shootingPoint.position, m_shootingPoint.rotation)
                    .GetComponent<Bullet>();
            bullet.Activate(m_currentHittableLayer);
        }

        private void ShootingVfx()
        {
            if (m_shootFlash == null) return;
            if (m_shootFlash.isPlaying) m_shootFlash.Clear();
            m_shootFlash.Play();
        }

        private void Haptics() => m_vrGrabbable.Controller.Haptic();
    }
}