using UnityEngine;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public sealed class CameraMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _movementSpeed = 12f;

        public void Move(Vector2 input, float deltaTime)
        {
            if (input.sqrMagnitude < 0.001f)
                return;

            Vector3 forward = Vector3.ProjectOnPlane(
                transform.forward,
                Vector3.up).normalized;

            Vector3 right = Vector3.ProjectOnPlane(
                transform.right,
                Vector3.up).normalized;

            Vector3 direction =
                forward * input.y +
                right * input.x;

            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            transform.position +=
                direction * (_movementSpeed * deltaTime);
        }
    }
}