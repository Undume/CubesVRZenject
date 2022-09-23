using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SharedUtils;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public interface IMenuController
    {
        public void Setup();
        public void AppStartsPlaying();
        public void TransitionToMenu();
        public void HideMenu();
    }
    
    public class MenuController : IMenuController
    {
        [Inject]
        public void Construct(Settings setting, MonoHelper monoHelper)
        {
            m_parentMenu = setting.ParentMenu;
            m_menuItems = setting.MenuItems;
            m_highscorePanel = setting.HighscorePanel;
            m_inAnimationDuration = setting.InAnimationDuration;
            m_outAnimationDuration = setting.OutAnimationDuration;
            m_monoHelper = monoHelper;
        }

        private GameObject m_parentMenu;
        private List<Transform> m_menuItems;
        private HighscorePanel m_highscorePanel;
        private float m_inAnimationDuration = 1.5f;
        private float m_outAnimationDuration = 0.5f;
        private MonoHelper m_monoHelper;

        private readonly List<Vector3> m_initialScales = new();

        public void Setup()
        {
            foreach (var item in m_menuItems)
                m_initialScales.Add(item.localScale);
        }

        public void AppStartsPlaying()
        {
            m_parentMenu.SetActive(false);
            m_highscorePanel.RefreshRank();
        }

        public void TransitionToMenu()
        {
            foreach (var item in m_menuItems)
                item.localScale = Vector3.zero;
            m_parentMenu.SetActive(true);
            AppearingAnimation();
            m_highscorePanel.RefreshRank();
        }

        public void HideMenu()
        {
            DisappearingAnimation();
            m_monoHelper.StartCoroutine(DeactivateAfterDisappearingAnimation());
        }

        private void AppearingAnimation()
        {
            for (var i = 0; i < m_menuItems.Count; i++)
            {
                m_menuItems[i].DOScale(m_initialScales[i], m_inAnimationDuration).SetEase(Ease.InOutSine);
            }
        }

        private void DisappearingAnimation()
        {
            for (var i = 0; i < m_menuItems.Count; i++)
            {
                m_menuItems[i].DOScale(Vector3.zero, m_outAnimationDuration).SetEase(Ease.InOutSine);
            }
        }

        private IEnumerator DeactivateAfterDisappearingAnimation()
        {
            yield return new WaitForSeconds(m_outAnimationDuration);
            m_parentMenu.SetActive(false);
        }

        [Serializable]
        public class Settings
        {
            public GameObject ParentMenu;
            public List<Transform> MenuItems;
            public HighscorePanel HighscorePanel;
            [Space] public float InAnimationDuration = 1.5f;
            [Space] public float OutAnimationDuration = 0.5f;
        }
    }
}