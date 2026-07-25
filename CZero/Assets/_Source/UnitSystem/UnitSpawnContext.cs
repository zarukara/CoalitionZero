using GridSystem;

namespace UnitSystem
{
    public readonly struct UnitSpawnContext
    {
        public GridFacade Grid { get; }

        public UnitOccupancy Occupancy { get; }

        public UnitSquad Squad { get; }

        public GridPosition Position { get; }

        public int SquadIndex { get; }

        public UnitSpawnContext(
            GridFacade grid,
            UnitOccupancy occupancy,
            UnitSquad squad,
            GridPosition position,
            int squadIndex)
        {
            Grid = grid;
            Occupancy = occupancy;
            Squad = squad;
            Position = position;
            SquadIndex = squadIndex;
        }
    }
}