using SharedUtils.UpdateManager;
using ShootBoxes.Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;
using Zenject;

namespace ShootBoxes.Core
{
    public class VRController : UpdateManagedMonoBehaviour
    {
        [Inject] private IAppManagement m_appManager;
        [Inject] private IGameController gameController;

        [SerializeField] private Enums.Side m_side;

        [Header("InputActions")] [SerializeField]
        private InputActionReference m_trigger;

        [SerializeField] private InputActionReference m_primary;
        [SerializeField] private InputActionReference m_haptic;
        [SerializeField] private InputActionReference m_menu;

        [Header("Haptics")] [SerializeField] private float m_amplitude = 0.7f;
        [SerializeField] private float m_duration = 0.7f;


        [Space] [SerializeField] private VRGrabbable m_vrGrabbable;

        private void Awake()
        {
            m_trigger.action.started += TriggerStart;
            m_trigger.action.performed += TriggerEnd;
            m_primary.action.started += PrimaryStart;
            if (m_menu) m_menu.action.performed += MenuPerformed;
            if (m_vrGrabbable) m_vrGrabbable.Controller = this;
        }

        private void TriggerStart(InputAction.CallbackContext obj)
        {
            if (m_vrGrabbable) m_vrGrabbable.TriggerStart(obj);
        }


        private void TriggerEnd(InputAction.CallbackContext obj)
        {
            if (m_vrGrabbable) m_vrGrabbable.TriggerEnd(obj);
        }

        private void PrimaryStart(InputAction.CallbackContext obj)
        {
            if (m_vrGrabbable) m_vrGrabbable.PrimaryStart(obj);
        }

        private void MenuPerformed(InputAction.CallbackContext obj)
        {
            if (m_appManager.CurrentScene() == Constants.Enums.GameScene.InGame)
            {
                gameController.PauseGame();
            }
            else if (m_appManager.CurrentScene() == Constants.Enums.GameScene.Pause)
            {
                gameController.ResumeGame();
            }
        }

        public void Haptic()
        {
            OpenXRInput.SendHapticImpulse(m_haptic, m_amplitude, m_duration,
                m_side == Enums.Side.Left
                    ? UnityEngine.InputSystem.XR.XRController.leftHand
                    : UnityEngine.InputSystem.XR.XRController.rightHand);
        }
    }
}