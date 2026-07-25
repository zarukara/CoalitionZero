using GridSystem;

namespace UnitSystem
{
    public readonly struct UnitSpawnContext
    {
        public GridFacade Grid { get; }
        public UnitOccupancy Occupancy { get; }
        public GridPosition Position { get; }

        public UnitSpawnContext(
            GridFacade grid,
            UnitOccupancy occupancy,
            GridPosition position)
        {
            Grid = grid;
            Occupancy = occupancy;
            Position = position;
        }
    }
}