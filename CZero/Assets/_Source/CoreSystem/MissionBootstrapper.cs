using GridSystem;
using UnitSystem;
using UnityEngine;

namespace CoreSystem
{
    [DisallowMultipleComponent]
    public sealed class MissionBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private GridFacade _grid;

        [SerializeField]
        private UnitFactory _unitFactory;

        [SerializeField]
        private UnitMoveController _unitMoveController;

        [SerializeField]
        private Vector2Int _testSpawnPosition =
            new Vector2Int(25, 25);

        private UnitOccupancy _occupancy;

        private void Start()
        {
            if (!ValidateDependencies())
            {
                enabled = false;
                return;
            }

            _occupancy = new UnitOccupancy();

            if (!_unitFactory.Initialize(
                    _grid,
                    _occupancy))
            {
                enabled = false;
                return;
            }

            SpawnTestUnit();
        }

        private void SpawnTestUnit()
        {
            GridPosition spawnPosition = new GridPosition(
                _testSpawnPosition.x,
                _testSpawnPosition.y);

            Unit unit = _unitFactory.Create(spawnPosition);

            if (unit == null)
            {
                Debug.LogError(
                    "Не удалось создать тестового юнита.",
                    this);

                enabled = false;
                return;
            }

            _unitMoveController.SetControlledUnit(unit);
        }

        private bool ValidateDependencies()
        {
            if (_grid != null
                && _unitFactory != null
                && _unitMoveController != null)
            {
                return true;
            }

            Debug.LogError(
                $"{nameof(MissionBootstrapper)}: " +
                "зависимости не назначены.",
                this);

            return false;
        }
    }
}