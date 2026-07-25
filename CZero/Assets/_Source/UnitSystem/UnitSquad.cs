using System;
using System.Collections.Generic;

namespace UnitSystem
{
    public sealed class UnitSquad
    {
        public const int MaxUnitCount = 4;

        private readonly SortedDictionary<int, Unit> _units = new();

        public event Action<Unit> UnitRemoved;

        public int Count => _units.Count;

        public bool TryRegister(
            Unit unit,
            int squadIndex)
        {
            if (unit == null)
                return false;

            if (!IsValidIndex(squadIndex))
                return false;

            if (_units.ContainsKey(squadIndex))
                return false;

            _units.Add(squadIndex, unit);
            return true;
        }

        public bool Contains(Unit unit)
        {
            if (ReferenceEquals(unit, null))
                return false;

            foreach (Unit registeredUnit in _units.Values)
            {
                if (ReferenceEquals(registeredUnit, unit))
                    return true;
            }

            return false;
        }

        public bool TryGetFirstAvailableUnit(out Unit unit)
        {
            foreach (Unit registeredUnit in _units.Values)
            {
                if (registeredUnit == null)
                    continue;

                unit = registeredUnit;
                return true;
            }

            unit = null;
            return false;
        }

        public void Unregister(
            Unit unit,
            int squadIndex)
        {
            if (ReferenceEquals(unit, null))
                return;

            if (!_units.TryGetValue(
                    squadIndex,
                    out Unit registeredUnit))
            {
                return;
            }

            if (!ReferenceEquals(registeredUnit, unit))
                return;

            _units.Remove(squadIndex);
            UnitRemoved?.Invoke(unit);
        }

        private static bool IsValidIndex(int squadIndex)
        {
            return squadIndex >= 1
                   && squadIndex <= MaxUnitCount;
        }
    }
}