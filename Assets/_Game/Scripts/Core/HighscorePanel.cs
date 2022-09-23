using System.ComponentModel;
using ShootBoxes.Persistance;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShootBoxes.Core
{
    public class HighscorePanel : MonoBehaviour
    {
        [SerializeField] private Text[] m_rankPositions;

        [Inject] private PersistanceController m_persistanceController;

        public void RefreshRank()
        {
            var highscores = m_persistanceController.Model.TimeHighscores;
            if (m_rankPositions.Length >= 10)
            {
                for (var i = 0; i < highscores.Count; i++)
                    m_rankPositions[i].text = $"{highscores[i]}";
            }
        }
    }
}