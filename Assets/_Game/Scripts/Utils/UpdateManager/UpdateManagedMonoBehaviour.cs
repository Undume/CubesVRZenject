using UnityEngine;

namespace SharedUtils.UpdateManager
{
    public class UpdateManagedMonoBehaviour : MonoBehaviour
    {
        public UpdateType updateType = UpdateType.General;

        protected virtual void OnEnable()
        {
            UpdateManager.AddItem(this, updateType);
        }

        protected virtual void OnDisable()
        {
            UpdateManager.RemoveSpecificItem(this, updateType);
        }

        public virtual void UpdateMe(float scaledTime)
        {
        }

        public virtual void FixedUpdateMe(float scaledTime)
        {
        }

        public virtual void LateUpdateMe(float scaledTime)
        {
        }
    }
}