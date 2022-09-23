using System;
using System.Collections.Generic;
using SharedUtils;
using Zenject;

namespace ShootBoxes.Persistance
{
    public class PersistanceController
    {
        public PersistanceModel Model => m_model;

        private PersistanceModel m_model;
        private string m_fileName;

        [Inject]
        public void Construct(PersistanceModel model, string filename)
        {
            m_model = model;
            m_fileName = filename;
        }

        public void Save()
        {
            FileSystem.SaveFile(m_fileName, m_model);
        }


        public void Recreate()
        {
            FileSystem.DeleteFile(m_fileName);
            m_model.Reset();
        }
    }
}