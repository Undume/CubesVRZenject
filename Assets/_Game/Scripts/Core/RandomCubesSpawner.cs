using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ShootBoxes.Core
{
    public interface ICubesSpawner
    {
        public void Setup();
        public void SpawnCubes();
        public void DespawnCubes();
        public int NumberOfCubes();
    }

    [Serializable]
    public class RandomCubesSpawner : ICubesSpawner
    {
        [Serializable]
        public class Settings
        {
            public BoxCollider SpawningCollider;
            public Vector2Int RangeCubesNumber;
            public CubeSpawn CubeSpawnPrefab;
            public Transform CubesPoolParent;
            public int DifferentColors;
            public Material SampleMaterial;
            public float AnimationDuration;
        }

        public int NumberOfCubes() => m_randomCubesNumber;

        private Vector2Int m_rangeCubesNumber;
        private CubeSpawn m_cubeSpawnPrefab;
        private Transform m_cubesPoolParent;
        private int m_differentColors;
        private Material m_sampleMaterial;
        private float m_animationDuration;

        private List<CubeSpawn> m_cubesPool = new();
        private List<Material> m_sharedMaterial = new();
        private Bounds m_bounds;
        private int m_randomCubesNumber;
        private CubeSpawn.Factory m_cubeFactory;

        [Inject]
        public void Construct(Settings setting, CubeSpawn.Factory cubeFactory)
        {
            m_bounds = setting.SpawningCollider.bounds;
            m_animationDuration = setting.AnimationDuration;
            m_differentColors = setting.DifferentColors;
            m_sampleMaterial = setting.SampleMaterial;
            m_rangeCubesNumber = setting.RangeCubesNumber;
            m_cubeFactory = cubeFactory;
        }

        public void Setup()
        {
            InstantiateCubesPool();
            CreateMaterials();
        }

        public void SpawnCubes()
        {
            ChooseSharedColors();

            m_randomCubesNumber = Random.Range(m_rangeCubesNumber.x, m_rangeCubesNumber.y + 1);
            for (var i = 0; i < m_randomCubesNumber && i < m_cubesPool.Count; i++)
            {
                var randomX = m_bounds.center.x + Random.Range(-m_bounds.extents.x, m_bounds.extents.x);
                var randomY = m_bounds.center.y + Random.Range(-m_bounds.extents.y, m_bounds.extents.y);
                var randomZ = m_bounds.center.z + Random.Range(-m_bounds.extents.z, m_bounds.extents.z);
                m_cubesPool[i].Initialize(m_sharedMaterial[Random.Range(0, m_sharedMaterial.Count)],
                    new Vector3(randomX, randomY, randomZ));
            }
        }

        public void DespawnCubes()
        {
            foreach (var cube in m_cubesPool)
            {
                if (cube.gameObject.activeSelf)
                {
                    cube.Despawn(m_animationDuration);
                }
            }
        }

        private void CreateMaterials()
        {
            for (var i = 0; i < m_differentColors; i++) m_sharedMaterial.Add(new Material(m_sampleMaterial));
        }

        private void InstantiateCubesPool()
        {
            for (var i = 0; i < m_rangeCubesNumber.y; i++)
            {
                m_cubesPool.Add(m_cubeFactory.Create());
            }
        }

        private void ChooseSharedColors()
        {
            foreach (var mat in m_sharedMaterial)
            {
                mat.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);
            }
        }
    }
}