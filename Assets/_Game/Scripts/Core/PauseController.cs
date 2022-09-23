using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShootBoxes.Core
{
    public interface IPauseController
    {
        public void ShowPause();
        public void HidePause();
    }

    [Serializable]
    public class PauseController : IPauseController
    {
        [Inject]
        public void Construct(Settings setting)
        {
            m_pauseParent = setting.PauseParent;
            m_cameraHead = setting.CameraHead;
            m_distanceForwardPlayer = setting.DistanceForwardPlayer;
            m_weapons = setting.Weapons;
        }

        private Transform m_pauseParent;
        private Transform m_cameraHead;
        private float m_distanceForwardPlayer = 3.5f;
        private List<VRWeapon> m_weapons;

        public void ShowPause()
        {
            PositionGameOverPanel();
            foreach (var weapon in m_weapons)
                weapon.SetUIShootingMode();
        }

        public void HidePause()
        {
            m_pauseParent.gameObject.SetActive(false);
            foreach (var weapon in m_weapons)
                weapon.SetNormalShootingMode();
        }

        private void PositionGameOverPanel()
        {
            m_pauseParent.gameObject.SetActive(true);
            var fwdCamera = m_cameraHead.forward;
            fwdCamera.y = 0;
            fwdCamera *= m_distanceForwardPlayer;
            var positionPanel = m_cameraHead.position + fwdCamera;
            m_pauseParent.transform.position = positionPanel;
            m_pauseParent.transform.rotation = Quaternion.LookRotation(positionPanel - m_cameraHead.position);
            m_pauseParent.transform.rotation = Quaternion.Euler(0f, m_pauseParent.transform.eulerAngles.y, 0f);
        }

        [Serializable]
        public class Settings
        {
            public Transform PauseParent;
            public Transform CameraHead;
            public float DistanceForwardPlayer = 4f;
            public List<VRWeapon> Weapons;
        }
    }
}