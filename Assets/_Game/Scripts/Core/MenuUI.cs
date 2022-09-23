using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public class MenuUI : MonoBehaviour
    {
        [Inject] private IAppManagement m_appManager;

        public void PlayButtonShoot()
        {
            m_appManager.StartGame();
        }

        public void ExitButtonShoot()
        {
            m_appManager.ExitGame();
        }
    }
}