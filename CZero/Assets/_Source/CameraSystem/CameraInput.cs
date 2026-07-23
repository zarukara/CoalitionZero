using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class CameraInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _rotateAction;
        [SerializeField] private InputActionReference _zoomAction;

        public Vector2 MoveDirection =>
            _moveAction.action.ReadValue<Vector2>();

        public float ZoomDelta =>
            _zoomAction.action.ReadValue<float>();

        public bool TryGetRotation(out float direction)
        {
            direction = 0f;

            if (!_rotateAction.action.WasPressedThisFrame())
                return false;

            direction = Mathf.Sign(
                _rotateAction.action.ReadValue<float>());

            return !Mathf.Approximately(direction, 0f);
        }

        private void OnEnable()
        {
            if (!AreActionsAssigned())
            {
                Debug.LogError(
                    $"{nameof(CameraInput)}: действия ввода не назначены.",
                    this);

                enabled = false;
                return;
            }

            _moveAction.action.Enable();
            _rotateAction.action.Enable();
            _zoomAction.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.action?.Disable();
            _rotateAction?.action?.Disable();
            _zoomAction?.action?.Disable();
        }

        private bool AreActionsAssigned()
        {
            return _moveAction?.action != null
                   && _rotateAction?.action != null
                   && _zoomAction?.action != null;
        }
    }
}