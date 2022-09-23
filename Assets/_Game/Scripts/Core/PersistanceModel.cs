using System;
using System.Collections.Generic;
using Zenject;

namespace ShootBoxes.Persistance
{
    [Serializable]
    public class PersistanceModel
    {
        public List<int> TimeHighscores = new();

        [Inject]
        private void Construct(bool test)
        {
            if (test)
            {
                TimeHighscores = new List<int>() {300, 360, 420, 480, 560, 620, 680, 740, 800, 860};
            }
        }

        public void Reset()
        {
            TimeHighscores = new();
        }
    }
}