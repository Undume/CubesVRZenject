using System.Collections.Generic;
using ShootBoxes.Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace ShootBoxes.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Enums.GameScene startingGameScene;
        [SerializeField] private PauseController.Settings m_pauseSettings;
        [SerializeField] private RandomCubesSpawner.Settings m_cubesSpawnerSettings;
        [SerializeField] private MenuController.Settings m_menuControllerSettings;
        [SerializeField] private GameArenaController.Settings m_gameInitializerSettings;
        [SerializeField] private List<InputActionAsset> m_ActionAssets;
        [SerializeField] private TimeController m_timeControllerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputActionManager>().AsSingle().WithArguments(m_ActionAssets);
            Container.BindInterfacesAndSelfTo<AppManager>().AsSingle().WithArguments(startingGameScene);
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.Bind<IMenuController>().To<MenuController>().AsSingle().WithArguments(m_menuControllerSettings);
            Container.Bind<IArenaController>().To<GameArenaController>().AsSingle()
                .WithArguments(m_gameInitializerSettings);
            Container.Bind<IPauseController>().To<PauseController>().AsSingle().WithArguments(m_pauseSettings);
            Container.Bind<ICubesSpawner>().To<RandomCubesSpawner>().AsSingle().WithArguments(m_cubesSpawnerSettings);
            Container.Bind<TimeController>().FromComponentInNewPrefab(m_timeControllerPrefab).AsSingle();

            Container.BindFactory<CubeSpawn, CubeSpawn.Factory>()
                .FromComponentInNewPrefab(m_cubesSpawnerSettings.CubeSpawnPrefab)
                .UnderTransform(m_cubesSpawnerSettings.CubesPoolParent);
        }
    }
}