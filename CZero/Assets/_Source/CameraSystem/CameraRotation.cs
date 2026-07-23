using UnityEngine;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class CameraRotation : MonoBehaviour
    {
        [SerializeField, Min(1f)]
        private float _rotationStep = 90f;

        [SerializeField, Min(1f)]
        private float _rotationSpeed = 540f;

        private float _targetYaw;
        private float _pitch;
        private float _roll;

        private void Awake()
        {
            Vector3 rotation = transform.eulerAngles;

            _pitch = rotation.x;
            _targetYaw = rotation.y;
            _roll = rotation.z;
        }

        public void RequestRotation(float direction)
        {
            if (Mathf.Approximately(direction, 0f))
                return;

            _targetYaw += Mathf.Sign(direction) * _rotationStep;
        }

        public void Tick(float deltaTime)
        {
            float currentYaw = transform.eulerAngles.y;

            float nextYaw = Mathf.MoveTowardsAngle(
                currentYaw,
                _targetYaw,
                _rotationSpeed * deltaTime);

            transform.rotation = Quaternion.Euler(
                _pitch,
                nextYaw,
                _roll);
        }
    }
}