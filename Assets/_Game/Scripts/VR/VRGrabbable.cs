using SharedUtils.UpdateManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ShootBoxes.Core
{
    public class VRGrabbable : UpdateManagedMonoBehaviour
    {
        public VRController Controller;

        [SerializeField] private UnityEvent<InputAction.CallbackContext> m_onTriggerStart;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> m_onTriggerEnd;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> m_onPrimaryStart;

        public void TriggerStart(InputAction.CallbackContext obj) => m_onTriggerStart.Invoke(obj);
        public void TriggerEnd(InputAction.CallbackContext obj) => m_onTriggerEnd.Invoke(obj);
        public void PrimaryStart(InputAction.CallbackContext obj) => m_onPrimaryStart.Invoke(obj);
    }
}