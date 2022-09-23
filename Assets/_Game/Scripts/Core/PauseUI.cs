using System.Collections;
using System.Collections.Generic;
using ShootBoxes.Core;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public class PauseUI : MonoBehaviour
    {
        [Inject] private IPauseController m_pauseController;
        [Inject] private IGameController m_gameController;

        public void ResumeButtonShoot()
        {
            m_pauseController.HidePause();
        }

        public void ExitButtonShoot()
        {
            m_gameController.FinishGame();
        }
    }
}