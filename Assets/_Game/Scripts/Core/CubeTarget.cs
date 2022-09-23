using System;
using System.Collections;
using SharedUtils;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ShootBoxes.Core
{
    public class CubeTarget : MonoBehaviour, IHittable
    {
        private IScoreCounter scoreCounterController;
        [SerializeField] private RandomSFX m_hitted;

        private bool m_alreadyCounted;
        private Rigidbody m_rigidBody;

        [Inject]
        public void Construct(IScoreCounter scoreCounterController, Rigidbody rigidBody)
        {
            this.scoreCounterController = scoreCounterController;
            m_rigidBody = rigidBody;
        }

        private void OnEnable()
        {
            m_alreadyCounted = false;
        }

        public void Hit(RaycastHit hit, Bullet bullet)
        {
            m_hitted.PlayRandomSFX();
            m_rigidBody.isKinematic = false;
            m_rigidBody.AddForceAtPosition(
                Random.Range(bullet.ForceRange.x, bullet.ForceRange.y) * bullet.transform.forward,
                hit.point, ForceMode.Impulse);
            if (!m_alreadyCounted)
            {
                m_alreadyCounted = true;
                scoreCounterController.BoxShot();
                StartCoroutine(SelfDestruction());
            }
        }

        private IEnumerator SelfDestruction()
        {
            yield return new WaitForSeconds(2.5f);
            gameObject.SetActive(false);
        }
    }
}