using GridSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    public sealed class UnitMoveController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _primaryAction;

        [SerializeField]
        private GridCursorController _gridCursor;

        [SerializeField]
        private Unit _controlledUnit;

        private void Awake()
        {
            if (_primaryAction?.action != null
                && _gridCursor != null
                && _controlledUnit != null)
            {
                return;
            }

            Debug.LogError(
                $"{nameof(UnitMoveController)}: " +
                "зависимости не назначены.",
                this);

            enabled = false;
        }

        private void OnEnable()
        {
            _primaryAction.action.Enable();
        }

        private void OnDisable()
        {
            _primaryAction?.action?.Disable();
        }

        private void Update()
        {
            if (!_primaryAction.action.WasPressedThisFrame())
                return;

            if (!_gridCursor.TryGetCurrentPosition(
                    out GridPosition position))
            {
                return;
            }

            _controlledUnit.Place(position);
        }
    }
}