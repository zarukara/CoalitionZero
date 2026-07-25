using GridSystem;
using UnityEngine;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class UnitSelectionView : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        [SerializeField, Range(0.1f, 1f)]
        private float _cellFill = 0.82f;

        [SerializeField, Min(0f)]
        private float _heightOffset = 0.02f;

        private float _heightScale;

        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();

            _heightScale = transform.localScale.y;

            Hide();
        }

        public void Show(
            GridFacade grid,
            GridPosition position)
        {
            if (grid == null)
            {
                Hide();
                return;
            }

            transform.position =
                grid.GridToWorld(position)
                + Vector3.up * _heightOffset;

            transform.localScale = new Vector3(
                grid.CellSize * _cellFill,
                _heightScale,
                grid.CellSize * _cellFill);

            _renderer.enabled = true;
        }

        public void Hide()
        {
            if (_renderer != null)
                _renderer.enabled = false;
        }
    }
}