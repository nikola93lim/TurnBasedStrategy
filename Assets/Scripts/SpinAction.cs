using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0f, spinAmount, 0f);

        totalSpinAmount += spinAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }

    public override bool TryTakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0f;

        ActionStart(onActionComplete);

        return true;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetAllValidGridPositionsListForThisAction()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0,
        };
    }

}
