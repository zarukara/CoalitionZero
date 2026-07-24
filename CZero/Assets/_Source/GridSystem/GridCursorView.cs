using UnityEngine;

namespace GridSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class GridCursorView : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        [SerializeField, Range(0.1f, 1f)]
        private float _cellFill = 0.92f;

        [SerializeField, Min(0f)]
        private float _heightOffset = 0.03f;

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
            Vector3 worldPosition,
            float cellSize)
        {
            transform.position =
                worldPosition + Vector3.up * _heightOffset;

            transform.localScale = new Vector3(
                cellSize * _cellFill,
                _heightScale,
                cellSize * _cellFill);

            _renderer.enabled = true;
        }

        public void Hide()
        {
            if (_renderer != null)
                _renderer.enabled = false;
        }
    }
}