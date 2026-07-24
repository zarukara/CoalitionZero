using System;
using UnityEngine;

namespace GridSystem
{
    [DisallowMultipleComponent]
    public sealed class GridFacade : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private int _width = 20;

        [SerializeField, Min(1)]
        private int _height = 20;

        [SerializeField, Min(0.1f)]
        private float _cellSize = 1f;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public bool Contains(GridPosition position)
        {
            return position.X >= 0
                   && position.X < _width
                   && position.Y >= 0
                   && position.Y < _height;
        }

        public GridPosition WorldToGrid(Vector3 worldPosition)
        {
            Vector3 localPosition =
                transform.InverseTransformPoint(worldPosition);

            int x = Mathf.FloorToInt(
                localPosition.x / _cellSize);

            int y = Mathf.FloorToInt(
                localPosition.z / _cellSize);

            return new GridPosition(x, y);
        }

        public bool TryWorldToGrid(
            Vector3 worldPosition,
            out GridPosition gridPosition)
        {
            gridPosition = WorldToGrid(worldPosition);
            return Contains(gridPosition);
        }

        public Vector3 GridToWorld(GridPosition gridPosition)
        {
            if (!Contains(gridPosition))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(gridPosition),
                    gridPosition,
                    "Позиция находится за пределами сетки.");
            }

            Vector3 localPosition = new Vector3(
                (gridPosition.X + 0.5f) * _cellSize,
                0f,
                (gridPosition.Y + 0.5f) * _cellSize);

            return transform.TransformPoint(localPosition);
        }
    }
}