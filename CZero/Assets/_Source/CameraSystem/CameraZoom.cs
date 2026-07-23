using UnityEngine;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class CameraZoom : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _zoomSensitivity = 0.02f;

        [SerializeField]
        private float _minimumHeight = 6f;

        [SerializeField]
        private float _maximumHeight = 20f;

        public void Zoom(float input)
        {
            if (Mathf.Abs(input) < 0.01f)
                return;

            Vector3 movement =
                transform.forward * input * _zoomSensitivity;

            Vector3 nextPosition = transform.position + movement;

            if (nextPosition.y < _minimumHeight
                || nextPosition.y > _maximumHeight)
            {
                float targetHeight = Mathf.Clamp(
                    nextPosition.y,
                    _minimumHeight,
                    _maximumHeight);

                float allowedHeightChange =
                    targetHeight - transform.position.y;

                if (Mathf.Abs(movement.y) < 0.001f)
                    return;

                float multiplier =
                    allowedHeightChange / movement.y;

                movement *= Mathf.Clamp01(multiplier);
            }

            transform.position += movement;
        }
    }
}