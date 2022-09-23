using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace SharedUtils.UpdateManager
{
    public enum UpdateType
    {
        General,
        Timed
    }

    public class UpdateManager : SingletonMonoBehaviour<UpdateManager>
    {
        public UpdateGroup GetUpdateGroup(UpdateType type) => m_updateGroups[type];
        protected override bool DoNotDestroyOnLoad => false;

        private static Dictionary<UpdateType, UpdateGroup> m_updateGroups = new Dictionary<UpdateType, UpdateGroup>();
        [SerializeField] private List<UpdateGroup> m_updateList = new List<UpdateGroup>();

        private static readonly Type s_overridableMonoBehaviourType = typeof(UpdateManagedMonoBehaviour);

        protected override void SingletonAwake()
        {
            foreach (UpdateType type in Enum.GetValues(typeof(UpdateType)))
            {
                var updateGroup = new UpdateGroup(type);
                m_updateGroups.Add(type, updateGroup);
                m_updateList.Add(updateGroup);
            }
        }


        private static void SubscribeToUpdate(Action<float> callback, UpdateType type)
        {
            if (Instance == null) return;

            m_updateGroups[type].OnUpdateEvent += callback;
        }

        private static void SubscribeToFixedUpdate(Action<float> callback, UpdateType type)
        {
            if (Instance == null) return;

            m_updateGroups[type].OnFixedUpdateEvent += callback;
        }

        private static void SubscribeToLateUpdate(Action<float> callback, UpdateType type)
        {
            if (Instance == null) return;

            m_updateGroups[type].OnLateUpdateEvent += callback;
        }

        private static void UnsubscribeFromUpdate(Action<float> callback, UpdateType type)
        {
            m_updateGroups[type].OnUpdateEvent -= callback;
        }

        private static void UnsubscribeFromFixedUpdate(Action<float> callback, UpdateType type)
        {
            m_updateGroups[type].OnFixedUpdateEvent -= callback;
        }

        private static void UnsubscribeFromLateUpdate(Action<float> callback, UpdateType type)
        {
            m_updateGroups[type].OnLateUpdateEvent -= callback;
        }

        public static void AddItem(UpdateManagedMonoBehaviour behaviour, UpdateType type = UpdateType.General)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to add is null!");
            AddItemToArray(behaviour, type);
        }

        public static void RemoveSpecificItem(UpdateManagedMonoBehaviour behaviour,
            UpdateType type = UpdateType.General)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to remove is null!");
            if (Instance != null) RemoveSpecificItemFromArray(behaviour, type);
        }

        public static void RemoveSpecificItemAndDestroyComponent(UpdateManagedMonoBehaviour behaviour,
            UpdateType type = UpdateType.General)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to remove is null!");
            if (Instance != null) RemoveSpecificItemFromArray(behaviour, type);
            Destroy(behaviour);
        }

        public static void RemoveSpecificItemAndDestroyGameObject(UpdateManagedMonoBehaviour behaviour,
            UpdateType type = UpdateType.General)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to remove is null!");
            if (Instance != null) RemoveSpecificItemFromArray(behaviour, type);
            Destroy(behaviour.gameObject);
        }

        private static void AddItemToArray(UpdateManagedMonoBehaviour behaviour, UpdateType type)
        {
            Type behaviourType = behaviour.GetType();

            if (behaviourType.GetMethod("UpdateMe").DeclaringType != s_overridableMonoBehaviourType)
                SubscribeToUpdate(behaviour.UpdateMe, type);

            if (behaviourType.GetMethod("FixedUpdateMe").DeclaringType != s_overridableMonoBehaviourType)
                SubscribeToFixedUpdate(behaviour.FixedUpdateMe, type);

            if (behaviourType.GetMethod("LateUpdateMe").DeclaringType != s_overridableMonoBehaviourType)
                SubscribeToLateUpdate(behaviour.LateUpdateMe, type);
        }

        private static void RemoveSpecificItemFromArray(UpdateManagedMonoBehaviour behaviour, UpdateType type)
        {
            UnsubscribeFromUpdate(behaviour.UpdateMe, type);
            UnsubscribeFromFixedUpdate(behaviour.FixedUpdateMe, type);
            UnsubscribeFromLateUpdate(behaviour.LateUpdateMe, type);
        }

        private void Update()
        {
            for (var i = 0; i < m_updateList.Count; i++) m_updateList[i].Update();
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < m_updateList.Count; i++) m_updateList[i].FixedUpdate();
        }

        private void LateUpdate()
        {
            for (var i = 0; i < m_updateList.Count; i++) m_updateList[i].LateUpdate();
        }

        [Serializable]
        public class UpdateGroup
        {
            public UpdateType Type => m_type;
            public void Update() => OnUpdateEvent?.Invoke(Speed);
            public void FixedUpdate() => OnFixedUpdateEvent?.Invoke(Speed);
            public void LateUpdate() => OnLateUpdateEvent?.Invoke(Speed);
            public event Action<float> OnUpdateEvent;
            public event Action<float> OnFixedUpdateEvent;
            public event Action<float> OnLateUpdateEvent;
            public float Speed = 1.0f;

            private UpdateType m_type;
            public UpdateGroup(UpdateType type) => m_type = type;
        }
    }
}