using System;
using System.Collections;
using System.Collections.Generic;
using SharedUtils;
using UnityEngine;
using Zenject;


namespace SharedUtils
{
    [Serializable]
    public class PoolItem
    {
        public GameObject Item;
        public int Number;
    }

    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private Transform m_parent;
        [SerializeField] private List<PoolItem> m_prefabs = new List<PoolItem>();
        private Dictionary<string, List<GameObject>> m_instancedItems = new Dictionary<string, List<GameObject>>();
        private DiContainer m_container;

        [Inject]
        public void Construct(DiContainer container)
        {
            m_container = container;
        }

        private void Awake()
        {
            foreach (var item in m_prefabs)
            {
                Add(item.Item, item.Number);
            }
        }

        public void Add(GameObject prefab, int number)
        {
            if (prefab == null)
            {
                Debug.LogError("Adding null");
                return;
            }

            if (m_instancedItems == null)
            {
                Debug.LogError("Dictionary not initialized");
                return;
            }

            if (string.IsNullOrEmpty(prefab.name))
            {
                Debug.LogError("Prefab require a name");
                return;
            }

            if (!m_instancedItems.ContainsKey(prefab.name))
            {
                m_instancedItems.Add(prefab.name, new List<GameObject>());
            }

            for (var i = 0; i < number; i++)
            {
                var instancedGO = m_container.InstantiatePrefab(prefab, m_parent);
                instancedGO.SetActive(false);
                m_instancedItems[prefab.name].Add(instancedGO);
            }
        }

        public GameObject Get(GameObject prefab)
        {
            GameObject returningGO = null;

            if (!m_instancedItems.ContainsKey(prefab.name))
            {
                Add(prefab, 2);
            }

            foreach (var item in m_instancedItems[prefab.name])
            {
                if (!item.activeSelf)
                {
                    returningGO = item;
                    break;
                }
            }

            if (!returningGO)
            {
                returningGO =  m_container.InstantiatePrefab(prefab, m_parent);
                m_instancedItems[prefab.name].Add(returningGO);
            }

            returningGO.SetActive(true);
            return returningGO;
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var returningGO = Get(prefab);
            returningGO.transform.position = position;
            returningGO.transform.rotation = rotation;
            return returningGO;
        }

        public GameObject Get(GameObject prefab, Transform transform)
        {
            return Get(prefab, transform.position, transform.rotation);
        }
    }
}