using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Grenade grenadePrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private GameObject popupText;

    private int maxThrowDistance = 7;
    private int grenadeCount = 2;

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override List<GridPosition> GetAllValidGridPositionsListForThisAction()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (LevelGrid.Instance.GetWorldPosition(testGridPosition) - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;

                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, maxThrowDistance, obstaclesLayerMask))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            ActionValue = 0,
            GridPosition = gridPosition
        };
    }

    public override bool TryTakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (grenadeCount > 0)
        {
            grenadeCount--;
            Grenade grenade = Instantiate(grenadePrefab, unit.GetWorldPosition(), Quaternion.identity);
            grenade.Setup(gridPosition, OnGrenadeFinished);

            ActionStart(onActionComplete);
            return true;
        }

        Instantiate(popupText, transform.position, Quaternion.identity, transform);
        return false;
    }

    private void OnGrenadeFinished()
    {
        ActionComplete();
    }
}
