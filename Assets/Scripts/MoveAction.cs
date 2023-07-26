using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;

    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive) return;

        Vector3 moveDirection = (positionList[currentPositionIndex] - transform.position).normalized;
        float stoppingDistance = 0.1f;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

        if (Vector3.Distance(transform.position, positionList[currentPositionIndex]) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        else
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                ActionComplete();

                OnStopMoving?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override bool TryTakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        positionList = new List<Vector3>();
        currentPositionIndex = 0;

        List<GridPosition> path = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        foreach (GridPosition position in path)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(position));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);

        return true;
    }

    public override List<GridPosition> GetAllValidGridPositionsListForThisAction()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position is out of bounds
                    continue;
                }

                if (testGridPosition == unitGridPosition)
                {
                    // grid position is the same one the unit is currently at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // grid position is occupied
                    continue;
                }

                if (!Pathfinding.Instance.IsGridPositionWalkable(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int distanceMultiplier = 10;

                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * distanceMultiplier)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = targetCountAtGridPosition * 10,
        };
    }

}
