using GridSystem;
using UnityEngine;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    public sealed class Unit : MonoBehaviour
    {
        [SerializeField]
        private GridFacade _grid;

        public GridPosition Position { get; private set; }

        public bool IsPlaced { get; private set; }

        private void Awake()
        {
            if (_grid != null)
                return;

            Debug.LogError(
                $"{nameof(Unit)}: Grid не назначен.",
                this);

            enabled = false;
        }

        private void Start()
        {
            if (!_grid.TryWorldToGrid(
                    transform.position,
                    out GridPosition position))
            {
                Debug.LogError(
                    $"{nameof(Unit)} находится за пределами сетки.",
                    this);

                enabled = false;
                return;
            }

            Place(position);
        }

        public bool Place(GridPosition position)
        {
            if (_grid == null || !_grid.Contains(position))
                return false;

            Position = position;
            IsPlaced = true;

            transform.position = _grid.GridToWorld(position);

            return true;
        }
    }
}