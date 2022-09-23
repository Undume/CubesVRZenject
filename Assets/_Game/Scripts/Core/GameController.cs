using SharedUtils;
using SharedUtils.UpdateManager;
using ShootBoxes.Constants;
using ShootBoxes.Persistance;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public interface IGameController
    {
        public void StartGame();
        public void PauseGame();
        public void ResumeGame();
        public void FinishGame(bool canceled = false);
    }

    public interface IScoreCounter
    {
        public void BoxShot();
        public int Score();
    }


    public class GameController : IInitializable, IGameController, IScoreCounter

    {
        public int Score() => m_score;

        [Inject] private IPauseController m_pauseController;
        [Inject] private ICubesSpawner randomCubesSpawner;
        [Inject] private IAppManagement m_appManager;
        [Inject] private TimeController m_timeController;
        [Inject] private PersistanceController m_persistanceController;

        private int m_score;

        public void Initialize()
        {
            randomCubesSpawner.Setup();
        }

        public void StartGame()
        {
            randomCubesSpawner.SpawnCubes();
            m_timeController.RestartTime();
            m_score = 0;
        }

        public void PauseGame()
        {
            m_appManager.SetPauseScene();
            UpdateManager.Instance.GetUpdateGroup(UpdateType.Timed).Speed = 0;
            m_pauseController.ShowPause();
        }

        public void ResumeGame()
        {
            m_appManager.SetInGameScene();
            UpdateManager.Instance.GetUpdateGroup(UpdateType.Timed).Speed = 1;
            m_pauseController.HidePause();
        }

        public void FinishGame(bool canceled = false)
        {
            if (!canceled) CheckIfNewHighscore();
            m_pauseController.HidePause();
            randomCubesSpawner.DespawnCubes();
            UpdateManager.Instance.GetUpdateGroup(UpdateType.Timed).Speed = 1;
            m_appManager.BackToMenu();
        }

        public void BoxShot()
        {
            m_score++;
            CheckIfLastBoxDestroyed();
        }

        private void CheckIfLastBoxDestroyed()
        {
            if (m_score >= randomCubesSpawner.NumberOfCubes())
            {
                FinishGame();
            }
        }

        private void CheckIfNewHighscore()
        {
            var highscores = m_persistanceController.Model.TimeHighscores;
            if (highscores.Count < 10)
            {
                highscores.Add((int) m_timeController.AccumulatedTime);
                highscores.Sort();
                m_persistanceController.Model.TimeHighscores = highscores;
                m_persistanceController.Save();
            }
            else
            {
                for (var i = 0; i < highscores.Count; i++)
                {
                    if ((int) m_timeController.AccumulatedTime < highscores[i])
                    {
                        highscores.Add((int) m_timeController.AccumulatedTime);
                        highscores.Sort();
                        highscores.RemoveAt(highscores.Count - 1);
                        m_persistanceController.Model.TimeHighscores = highscores;
                        m_persistanceController.Save();
                        break;
                    }
                }
            }
        }
    }
}