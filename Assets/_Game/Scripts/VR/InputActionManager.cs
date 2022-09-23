using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShootBoxes.Core
{
    public class InputActionManager : IInitializable, IDisposable
    {
        [Inject] private List<InputActionAsset> m_ActionAssets;

        public void Initialize()
        {
            if (m_ActionAssets == null)
                return;

            foreach (var actionAsset in m_ActionAssets)
            {
                if (actionAsset != null)
                {
                    actionAsset.Enable();
                }
            }
        }

        public void Dispose()
        {
            if (m_ActionAssets == null)
                return;

            foreach (var actionAsset in m_ActionAssets)
            {
                if (actionAsset != null)
                {
                    actionAsset.Disable();
                }
            }
        }
    }
}