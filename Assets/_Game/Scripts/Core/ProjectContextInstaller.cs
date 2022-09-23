using UnityEngine;
using Zenject;

namespace ShootBoxes.Persistance
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private string m_fileName = "game";

        public override void InstallBindings()
        {
            AddPersistance();
        }

        private void AddPersistance()
        {
            Container.Bind<PersistanceController>().AsSingle().WithArguments(m_fileName);
        }
    }
}