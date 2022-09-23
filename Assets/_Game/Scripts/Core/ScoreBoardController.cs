using SharedUtils.UpdateManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShootBoxes.Core
{
    public class ScoreBoardController : UpdateManagedMonoBehaviour
    {
        [Inject] private IScoreCounter m_gameController;

        [SerializeField] private Text m_text;
        [SerializeField] private float m_refreshRate;
        private float m_timer = 10f;

        public override void UpdateMe(float scaledTime)
        {
            m_timer += Time.deltaTime * scaledTime;
            if (m_timer > m_refreshRate)
            {
                m_timer = 0;
                m_text.text = $"{m_gameController.Score()}□";
            }
        }
    }
}