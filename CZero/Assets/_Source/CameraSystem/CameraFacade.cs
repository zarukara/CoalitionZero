using UnityEngine;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class CameraFacade : MonoBehaviour
    {
        [SerializeField] private CameraInput _input;
        [SerializeField] private CameraMovement _movement;
        [SerializeField] private CameraRotation _rotation;
        [SerializeField] private CameraZoom _zoom;

        private void Awake()
        {
            if (_input != null
                && _movement != null
                && _rotation != null
                && _zoom != null)
            {
                return;
            }

            Debug.LogError(
                $"{nameof(CameraFacade)}: зависимости не назначены.",
                this);

            enabled = false;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _movement.Move(
                _input.MoveDirection,
                deltaTime);

            if (_input.TryGetRotation(out float direction))
                _rotation.RequestRotation(direction);

            _rotation.Tick(deltaTime);
            _zoom.Zoom(_input.ZoomDelta);
        }
    }
}