using UnityEngine;
using Zenject;

namespace ShootBoxes.Persistance
{
    public class PersistanceView : MonoBehaviour
    {
        [SerializeField] private bool m_flagSave;
        [SerializeField] private bool m_flagDelete;
        [SerializeField] private PersistanceModel m_model;
        private PersistanceController m_controller;

        [Inject]
        public void Construct(PersistanceModel model, PersistanceController controller)
        {
            m_model = model;
            m_controller = controller;
        }


        private void Update()
        {
            if (Application.isEditor)
            {
                if (m_flagSave)
                {
                    m_flagSave = false;
                    m_controller.Save();
                }

                if (m_flagDelete)
                {
                    m_flagDelete = false;
                    m_controller.Recreate();
                }
            }
        }
    }
}