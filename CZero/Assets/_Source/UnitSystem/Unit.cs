using GridSystem;
using UnityEngine;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    public sealed class Unit : MonoBehaviour
    {
        private GridFacade _grid;
        private UnitOccupancy _occupancy;
        private UnitSquad _squad;

        public GridPosition Position { get; private set; }

        public int SquadIndex { get; private set; }

        public bool IsInitialized { get; private set; }

        public bool Initialize(UnitSpawnContext context)
        {
            if (IsInitialized)
            {
                Debug.LogError(
                    $"{nameof(Unit)} уже инициализирован.",
                    this);

                return false;
            }

            if (context.Grid == null
                || context.Occupancy == null
                || context.Squad == null)
            {
                Debug.LogError(
                    $"{nameof(Unit)} получил некорректный контекст.",
                    this);

                return false;
            }

            if (!context.Grid.Contains(context.Position))
            {
                Debug.LogError(
                    $"Клетка {context.Position} находится " +
                    "за пределами сетки.",
                    this);

                return false;
            }

            if (!context.Occupancy.TryRegister(
                    this,
                    context.Position))
            {
                Debug.LogError(
                    $"Клетка {context.Position} уже занята.",
                    this);

                return false;
            }

            if (!context.Squad.TryRegister(
                    this,
                    context.SquadIndex))
            {
                context.Occupancy.Unregister(
                    this,
                    context.Position);

                Debug.LogError(
                    $"Не удалось зарегистрировать юнита " +
                    $"с индексом {context.SquadIndex}.",
                    this);

                return false;
            }

            _grid = context.Grid;
            _occupancy = context.Occupancy;
            _squad = context.Squad;

            Position = context.Position;
            SquadIndex = context.SquadIndex;

            transform.position =
                _grid.GridToWorld(Position);

            IsInitialized = true;
            return true;
        }

        public bool TryMoveTo(GridPosition targetPosition)
        {
            if (!IsInitialized)
                return false;

            if (!_grid.Contains(targetPosition))
                return false;

            if (!_occupancy.TryMove(
                    this,
                    Position,
                    targetPosition))
            {
                return false;
            }

            Position = targetPosition;

            transform.position =
                _grid.GridToWorld(Position);

            return true;
        }

        private void OnDestroy()
        {
            if (!IsInitialized)
                return;

            _occupancy?.Unregister(
                this,
                Position);

            _squad?.Unregister(
                this,
                SquadIndex);
        }
    }
}