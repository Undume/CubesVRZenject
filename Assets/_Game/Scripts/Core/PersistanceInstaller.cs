using System;
using SharedUtils;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Persistance
{
    public class PersistanceInstaller : MonoInstaller
    {
        [SerializeField] private string m_fileName = "game";
        [SerializeField] private bool m_test = true;
        [SerializeField] private GameObject m_debugView;

        public override void InstallBindings()
        {
            Container.Bind<PersistanceController>().AsSingle().WithArguments(m_fileName);

            if (FileSystem.DoesFileExist(m_fileName))
            {
                var model = FileSystem.LoadFile<PersistanceModel>(m_fileName);
                Container.BindInstance(model).AsSingle();
            }
            else
            {
                Container.Bind<PersistanceModel>().AsSingle().WithArguments(m_test);
            }

#if UNITY_EDITOR
            Container.Bind<DiContainer>()
                .WithId(Guid.NewGuid())
                .FromSubContainerResolve()
                .ByNewContextPrefab(m_debugView)
                .AsSingle()
                .NonLazy();

#endif
        }
    }
}