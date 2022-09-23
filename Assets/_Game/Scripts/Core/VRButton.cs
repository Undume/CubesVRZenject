using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ShootBoxes.Core
{
    public class VRButton : MonoBehaviour, IHittable, IAimedDetector
    {
        [SerializeField] private Sprite m_focused;
        [SerializeField] private UnityEvent m_onClick;
        
        private Image m_buttonBackground;
        private Sprite m_initial;

        private void Awake()
        {
            m_buttonBackground = GetComponent<Image>();
            m_initial = m_buttonBackground.sprite;
        }

        public void Hit(RaycastHit hit, Bullet bullet)
        {
            m_onClick.Invoke();
        }

        public void Aimed()
        {
            m_buttonBackground.sprite = m_focused;
        }

        public void UnAimed()
        {
            m_buttonBackground.sprite = m_initial;
        }
    }
}