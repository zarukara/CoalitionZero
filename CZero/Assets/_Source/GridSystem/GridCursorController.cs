using UnityEngine;
using UnityEngine.InputSystem;

namespace GridSystem
{
    [DisallowMultipleComponent]
    public sealed class GridCursorController : MonoBehaviour
    {
        [SerializeField]
        private Camera _worldCamera;

        [SerializeField]
        private GridFacade _grid;

        [SerializeField]
        private GridCursorView _view;

        [SerializeField]
        private LayerMask _groundMask;

        [SerializeField, Min(1f)]
        private float _maximumRayDistance = 500f;

        private GridPosition _currentPosition;
        private bool _hasCurrentPosition;

        private void Awake()
        {
            if (_worldCamera != null
                && _grid != null
                && _view != null)
            {
                return;
            }

            Debug.LogError(
                $"{nameof(GridCursorController)}: " +
                "зависимости не назначены.",
                this);

            enabled = false;
        }

        private void Update()
        {
            if (!TryReadGridPosition(out GridPosition position))
            {
                ClearPosition();
                return;
            }

            if (_hasCurrentPosition
                && _currentPosition == position)
            {
                return;
            }

            _currentPosition = position;
            _hasCurrentPosition = true;

            _view.Show(
                _grid.GridToWorld(position),
                _grid.CellSize);
        }

        private void OnDisable()
        {
            ClearPosition();
        }

        public bool TryGetCurrentPosition(
            out GridPosition position)
        {
            position = _currentPosition;
            return _hasCurrentPosition;
        }

        private bool TryReadGridPosition(
            out GridPosition position)
        {
            position = default;

            if (Mouse.current == null)
                return false;

            Vector2 screenPosition =
                Mouse.current.position.ReadValue();

            Ray ray =
                _worldCamera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    _maximumRayDistance,
                    _groundMask,
                    QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            return _grid.TryWorldToGrid(
                hit.point,
                out position);
        }

        private void ClearPosition()
        {
            if (!_hasCurrentPosition)
                return;

            _hasCurrentPosition = false;
            _view.Hide();
        }
    }
}