using System.Collections.Generic;
using GridSystem;

namespace UnitSystem
{
    public sealed class UnitOccupancy
    {
        private readonly Dictionary<GridPosition, Unit> _units = new();

        public bool IsOccupied(GridPosition position)
        {
            return _units.ContainsKey(position);
        }

        public bool TryGetUnit(
            GridPosition position,
            out Unit unit)
        {
            return _units.TryGetValue(position, out unit);
        }

        public bool TryRegister(
            Unit unit,
            GridPosition position)
        {
            if (unit == null || IsOccupied(position))
                return false;

            _units.Add(position, unit);
            return true;
        }

        public bool TryMove(
            Unit unit,
            GridPosition from,
            GridPosition to)
        {
            if (unit == null)
                return false;

            if (!_units.TryGetValue(
                    from,
                    out Unit registeredUnit))
            {
                return false;
            }

            if (registeredUnit != unit)
                return false;

            if (from == to)
                return true;

            if (IsOccupied(to))
                return false;

            _units.Remove(from);
            _units.Add(to, unit);

            return true;
        }

        public void Unregister(
            Unit unit,
            GridPosition position)
        {
            if (!_units.TryGetValue(
                    position,
                    out Unit registeredUnit))
            {
                return;
            }

            if (registeredUnit == unit)
                _units.Remove(position);
        }
    }
}