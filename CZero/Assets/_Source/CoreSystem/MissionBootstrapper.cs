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
        private UnitCommandController _unitCommandController;

        [SerializeField]
        private Vector2Int[] _testSpawnPositions =
        {
            new Vector2Int(24, 25),
            new Vector2Int(26, 25)
        };

        private UnitOccupancy _occupancy;
        private UnitSquad _squad;

        private void Start()
        {
            if (!ValidateDependencies())
            {
                enabled = false;
                return;
            }

            _occupancy = new UnitOccupancy();
            _squad = new UnitSquad();

            if (!_unitFactory.Initialize(
                    _grid,
                    _occupancy))
            {
                enabled = false;
                return;
            }

            if (!_unitCommandController.Initialize(
                    _grid,
                    _occupancy,
                    _squad))
            {
                enabled = false;
                return;
            }

            if (!TrySpawnTestUnits())
            {
                enabled = false;
                return;
            }

            if (!_unitCommandController
                    .SelectFirstAvailableUnit())
            {
                Debug.LogError(
                    "Не удалось автоматически выбрать юнита.",
                    this);

                enabled = false;
            }
        }

        private bool TrySpawnTestUnits()
        {
            for (int i = 0;
                 i < _testSpawnPositions.Length;
                 i++)
            {
                Vector2Int position =
                    _testSpawnPositions[i];

                GridPosition gridPosition =
                    new GridPosition(
                        position.x,
                        position.y);

                int squadIndex = i + 1;

                Unit unit = _unitFactory.Create(
                    gridPosition,
                    _squad,
                    squadIndex);

                if (unit != null)
                    continue;

                Debug.LogError(
                    $"Не удалось создать юнита " +
                    $"с индексом {squadIndex} " +
                    $"в клетке ({position.x}, {position.y}).",
                    this);

                return false;
            }

            return true;
        }

        private bool ValidateDependencies()
        {
            if (_grid == null
                || _unitFactory == null
                || _unitCommandController == null)
            {
                Debug.LogError(
                    $"{nameof(MissionBootstrapper)}: " +
                    "зависимости не назначены.",
                    this);

                return false;
            }

            if (_testSpawnPositions == null
                || _testSpawnPositions.Length == 0)
            {
                Debug.LogError(
                    $"{nameof(MissionBootstrapper)}: " +
                    "позиции появления юнитов не заданы.",
                    this);

                return false;
            }

            if (_testSpawnPositions.Length
                > UnitSquad.MaxUnitCount)
            {
                Debug.LogError(
                    $"В отряде может быть не больше " +
                    $"{UnitSquad.MaxUnitCount} юнитов.",
                    this);

                return false;
            }

            return true;
        }
    }
}