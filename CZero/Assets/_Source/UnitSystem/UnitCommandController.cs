using GridSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnitSystem
{
    [DisallowMultipleComponent]
    public sealed class UnitCommandController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _primaryAction;

        [SerializeField]
        private GridCursorController _gridCursor;

        [SerializeField]
        private UnitSelectionView _selectionView;

        private GridFacade _grid;
        private UnitOccupancy _occupancy;
        private UnitSquad _squad;
        private Unit _selectedUnit;

        public Unit SelectedUnit => _selectedUnit;

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            if (_primaryAction?.action != null
                && _gridCursor != null
                && _selectionView != null)
            {
                return;
            }

            Debug.LogError(
                $"{nameof(UnitCommandController)}: " +
                "Inspector-зависимости не назначены.",
                this);

            enabled = false;
        }

        private void OnEnable()
        {
            _primaryAction?.action?.Enable();

            if (IsInitialized && _selectedUnit == null)
                SelectFirstAvailableUnit();
        }

        private void OnDisable()
        {
            _primaryAction?.action?.Disable();
            ClearSelection();
        }

        private void OnDestroy()
        {
            if (_squad != null)
                _squad.UnitRemoved -= HandleUnitRemoved;
        }

        public bool Initialize(
            GridFacade grid,
            UnitOccupancy occupancy,
            UnitSquad squad)
        {
            if (IsInitialized)
            {
                Debug.LogError(
                    $"{nameof(UnitCommandController)} " +
                    "уже инициализирован.",
                    this);

                return false;
            }

            if (grid == null
                || occupancy == null
                || squad == null)
            {
                Debug.LogError(
                    $"{nameof(UnitCommandController)} получил " +
                    "некорректные зависимости.",
                    this);

                return false;
            }

            _grid = grid;
            _occupancy = occupancy;
            _squad = squad;

            _squad.UnitRemoved += HandleUnitRemoved;

            IsInitialized = true;
            return true;
        }

        public bool SelectFirstAvailableUnit()
        {
            if (!IsInitialized)
                return false;

            if (!_squad.TryGetFirstAvailableUnit(
                    out Unit unit))
            {
                ClearSelection();
                return false;
            }

            SelectUnit(unit);
            return true;
        }

        private void Update()
        {
            if (!IsInitialized)
                return;

            if (!_primaryAction.action.WasPressedThisFrame())
                return;

            if (!_gridCursor.TryGetCurrentPosition(
                    out GridPosition position))
            {
                return;
            }

            if (_occupancy.TryGetUnit(
                    position,
                    out Unit unit))
            {
                if (_squad.Contains(unit))
                    SelectUnit(unit);

                return;
            }

            MoveSelectedUnit(position);
        }

        private void SelectUnit(Unit unit)
        {
            if (unit == null)
                return;

            if (!_squad.Contains(unit))
                return;

            _selectedUnit = unit;

            UpdateSelectionView();
        }

        private void MoveSelectedUnit(
            GridPosition targetPosition)
        {
            if (_selectedUnit == null)
                return;

            if (!_selectedUnit.TryMoveTo(targetPosition))
                return;

            UpdateSelectionView();
        }

        private void HandleUnitRemoved(Unit removedUnit)
        {
            if (!ReferenceEquals(
                    _selectedUnit,
                    removedUnit))
            {
                return;
            }

            _selectedUnit = null;

            SelectFirstAvailableUnit();
        }

        private void UpdateSelectionView()
        {
            if (_selectedUnit == null)
            {
                _selectionView.Hide();
                return;
            }

            _selectionView.Show(
                _grid,
                _selectedUnit.Position);
        }

        private void ClearSelection()
        {
            _selectedUnit = null;

            if (_selectionView != null)
                _selectionView.Hide();
        }
    }
}