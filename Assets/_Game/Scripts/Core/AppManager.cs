using ShootBoxes.Persistance;
using SharedUtils;
using SharedUtils.UpdateManager;
using ShootBoxes.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ShootBoxes.Core
{
    public interface IAppManagement
    {
        public Enums.GameScene CurrentScene();
        public void SetMenuScene();
        public void SetPauseScene();
        public void SetInGameScene();
        public void StartGame();
        public void BackToMenu();
        public void ExitGame();
    }

    public class AppManager : IInitializable, IAppManagement
    {
        [Inject]
        public void Construct(IArenaController arenaController, IMenuController menuController, ResonanceAudioRoom audioRoom,
            Enums.GameScene startingGameScene)
        {
            gameArenaController = arenaController;
            m_menuController = menuController;
            m_audioRoom = audioRoom;
            m_startingGameScene = startingGameScene;
        }

        public Enums.GameScene CurrentScene() => m_currentGameScene;
        public void SetMenuScene() => m_currentGameScene = Enums.GameScene.Menu;
        public void SetPauseScene() => m_currentGameScene = Enums.GameScene.Pause;
        public void SetInGameScene() => m_currentGameScene = Enums.GameScene.InGame;

        private Enums.GameScene m_startingGameScene;
        private Enums.GameScene m_currentGameScene;
        private ResonanceAudioRoom m_audioRoom;
        private IArenaController gameArenaController;
        private IMenuController m_menuController;

        public void Initialize()
        {
            gameArenaController.Setup();
            m_menuController.Setup();

            m_currentGameScene = m_startingGameScene;
            if (m_currentGameScene == Enums.GameScene.Menu)
            {
                m_menuController.TransitionToMenu();
                gameArenaController.AppStartAtMenu();
                m_audioRoom.gameObject.SetActive(false);
            }
            else if (m_currentGameScene == Enums.GameScene.InGame)
            {
                m_menuController.AppStartsPlaying();
                gameArenaController.TransitionToGame();
                m_audioRoom.gameObject.SetActive(true);
            }
        }

        public void StartGame()
        {
            SetInGameScene();
            gameArenaController.TransitionToGame();
            m_menuController.HideMenu();
            m_audioRoom.gameObject.SetActive(true);
        }

        public void BackToMenu()
        {
            SetMenuScene();
            gameArenaController.HideArenaGame();
            m_menuController.TransitionToMenu();
            m_audioRoom.gameObject.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}