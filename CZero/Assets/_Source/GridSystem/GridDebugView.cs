using UnityEngine;

namespace GridSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridFacade))]
    public sealed class GridDebugView : MonoBehaviour
    {
        [SerializeField]
        private GridFacade _grid;

        [SerializeField, Min(0f)]
        private float _heightOffset = 0.02f;

        [SerializeField]
        private Color _gridColor =
            new Color(0.2f, 0.8f, 1f, 0.7f);

        private void Reset()
        {
            _grid = GetComponent<GridFacade>();
        }

        private void OnValidate()
        {
            if (_grid == null)
                _grid = GetComponent<GridFacade>();
        }

        private void OnDrawGizmos()
        {
            if (_grid == null)
                return;

            Matrix4x4 previousMatrix = Gizmos.matrix;
            Color previousColor = Gizmos.color;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = _gridColor;

            DrawVerticalLines();
            DrawHorizontalLines();

            Gizmos.matrix = previousMatrix;
            Gizmos.color = previousColor;
        }

        private void DrawVerticalLines()
        {
            float gridHeight =
                _grid.Height * _grid.CellSize;

            for (int x = 0; x <= _grid.Width; x++)
            {
                float positionX = x * _grid.CellSize;

                Gizmos.DrawLine(
                    new Vector3(
                        positionX,
                        _heightOffset,
                        0f),
                    new Vector3(
                        positionX,
                        _heightOffset,
                        gridHeight));
            }
        }

        private void DrawHorizontalLines()
        {
            float gridWidth =
                _grid.Width * _grid.CellSize;

            for (int y = 0; y <= _grid.Height; y++)
            {
                float positionZ = y * _grid.CellSize;

                Gizmos.DrawLine(
                    new Vector3(
                        0f,
                        _heightOffset,
                        positionZ),
                    new Vector3(
                        gridWidth,
                        _heightOffset,
                        positionZ));
            }
        }
    }
}