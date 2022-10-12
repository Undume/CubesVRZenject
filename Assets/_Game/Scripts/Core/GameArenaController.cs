using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using DG.Tweening;
using SharedUtils;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace ShootBoxes.Core
{
    public interface IArenaController
    {
        public void Setup();
        public void TransitionToGame();
        public void HideArenaGame();
        public void AppStartAtMenu();
    }

    public class GameArenaController : IArenaController
    {
        private GameController m_gameController;
        private List<Transform> m_arenaWalls;
        private float m_inAnimationDuration = 2f;
        private float m_outAnimationDuration = 1.5f;

        private List<Vector3> m_initialScaleWalls = new List<Vector3>();

        [Inject]
        public void Construct(Settings setting, GameController gameController)
        {
            m_gameController = gameController;
            m_arenaWalls = setting.ArenaWalls;
            m_inAnimationDuration = setting.InAnimationDuration;
            m_outAnimationDuration = setting.OutAnimationDuration;
        }

        public void Setup()
        {
            foreach (var wall in m_arenaWalls)
                m_initialScaleWalls.Add(wall.localScale);
        }

        public void AppStartAtMenu()
        {
            SetArenaActive(false);
        }

        public void TransitionToGame()
        {
            foreach (var item in m_arenaWalls)
                item.localScale = Vector3.zero;
            SetArenaActive(true);
            _ = AppearingAnimation();
        }

        public void HideArenaGame()
        {
            _ = DisappearingAnimation();
        }


        private void SetArenaActive(bool active)
        {
            foreach (var wall in m_arenaWalls)
                wall.gameObject.SetActive(active);
        }

        private async Task AppearingAnimation()
        {
            for (var i = 0; i < m_arenaWalls.Count; i++)
            {
                m_arenaWalls[i].DOScale(m_initialScaleWalls[i], m_inAnimationDuration).SetEase(Ease.InOutSine);
            }

            await Task.Delay((int) (m_inAnimationDuration * 1000));
            m_gameController.StartGame();
        }


        private async Task DisappearingAnimation()
        {
            for (var i = 0; i < m_arenaWalls.Count; i++)
            {
                m_arenaWalls[i].DOScale(Vector3.zero, m_outAnimationDuration).SetEase(Ease.InOutSine);
            }

            await Task.Delay((int) (m_outAnimationDuration * 1000));
            SetArenaActive(false);
        }

        [Serializable]
        public class Settings
        {
            public List<Transform> ArenaWalls;
            public float InAnimationDuration = 2f;
            public float OutAnimationDuration = 1.5f;
        }
    }
}