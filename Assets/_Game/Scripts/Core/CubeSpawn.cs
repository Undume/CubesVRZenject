using DG.Tweening;
using SharedUtils.UpdateManager;
using UnityEngine;
using Zenject;


namespace ShootBoxes.Core
{
    public class CubeSpawn : UpdateManagedMonoBehaviour
    {
        [SerializeField] private Vector2 m_scaleRange = new Vector2(0.4f, 0.6f);
        [SerializeField] private float m_maxSecondsToAppear = 6f;
        [SerializeField] private float m_secondsToGrow = 1f;

        private MeshRenderer m_renderer;
        private Rigidbody m_rigidBody;
        private bool m_isGrown;
        private float m_appearingTimer;
        private float m_randomTimeToAppear;
        private Vector3 m_randomScale;
        private float m_growTimer;

        [Inject]
        public void Construct(MeshRenderer meshRenderer, Rigidbody rigidBody)
        {
            m_renderer = meshRenderer;
            m_rigidBody = rigidBody;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(Material randomMaterial, Vector3 position)
        {
            m_isGrown = false;
            m_growTimer = 0;
            m_appearingTimer = 0;
            m_renderer.sharedMaterial = randomMaterial;
            m_rigidBody.isKinematic = true;
            var newScale = Random.Range(m_scaleRange.x, m_scaleRange.y);
            m_randomScale = new Vector3(newScale, newScale, newScale);
            transform.localScale = new Vector3(0, 0, 0);
            var randomYRotation = Random.Range(0f, 360f);
            transform.eulerAngles = new Vector3(0, randomYRotation, 0);
            m_randomTimeToAppear = Random.Range(0.0f, m_maxSecondsToAppear);
            transform.position = position;
            gameObject.SetActive(true);
        }

        public override void UpdateMe(float scaledTime)
        {
            Grow(scaledTime);
        }

        private void Grow(float scaledTime)
        {
            if (!m_isGrown)
            {
                m_appearingTimer += Time.deltaTime * scaledTime;
                if (m_appearingTimer > m_randomTimeToAppear)
                {
                    m_growTimer += Time.deltaTime * scaledTime;
                    transform.localScale = Vector3.Lerp(Vector3.zero, m_randomScale, m_growTimer / m_secondsToGrow);
                    if (m_growTimer > m_secondsToGrow) m_isGrown = true;
                }
            }
        }

        public void Despawn(float animationDuration)
        {
            var delay = Random.Range(0, animationDuration * .5f);
            transform.DOScale(Vector3.zero, animationDuration - delay).SetEase(Ease.InOutSine).SetDelay(delay)
                .OnComplete(() => gameObject.SetActive(false));
        }
        
        public class Factory : PlaceholderFactory<CubeSpawn>
        {
        }
    }
}