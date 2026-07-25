using GridSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    public sealed class UnitFactory : MonoBehaviour
    {
        [SerializeField]
        private Unit _unitPrefab;

        [FormerlySerializedAs("_unitRoot")]
        [SerializeField]
        private Transform _spawnedUnitsRoot;

        private GridFacade _grid;
        private UnitOccupancy _occupancy;

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            if (_unitPrefab != null)
                return;

            Debug.LogError(
                $"{nameof(UnitFactory)}: Unit Prefab не назначен.",
                this);

            enabled = false;
        }

        public bool Initialize(
            GridFacade grid,
            UnitOccupancy occupancy)
        {
            if (IsInitialized)
            {
                Debug.LogError(
                    $"{nameof(UnitFactory)} уже инициализирована.",
                    this);

                return false;
            }

            if (grid == null || occupancy == null)
            {
                Debug.LogError(
                    $"{nameof(UnitFactory)} получила некорректные зависимости.",
                    this);

                return false;
            }

            if (_unitPrefab == null)
            {
                Debug.LogError(
                    $"{nameof(UnitFactory)}: Unit Prefab не назначен.",
                    this);

                return false;
            }

            _grid = grid;
            _occupancy = occupancy;

            IsInitialized = true;
            return true;
        }

        public Unit Create(GridPosition position)
        {
            if (!IsInitialized)
            {
                Debug.LogError(
                    $"{nameof(UnitFactory)} не инициализирована.",
                    this);

                return null;
            }

            Unit unit = Instantiate(
                _unitPrefab,
                _spawnedUnitsRoot);

            UnitSpawnContext context = new UnitSpawnContext(
                _grid,
                _occupancy,
                position);

            if (unit.Initialize(context))
                return unit;

            Destroy(unit.gameObject);
            return null;
        }
    }
}