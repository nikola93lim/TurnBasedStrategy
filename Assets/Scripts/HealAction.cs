using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    public event EventHandler OnHealActionStarted;
    public event EventHandler OnHealActionCompleted;

    private enum State
    {
        BeforeHeal,
        AfterHeal
    }

    private Unit targetUnit;

    private State state;
    private float stateTimer;

    private int maxHealDistance = 1;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.BeforeHeal:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);
                break;
            case State.AfterHeal:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.BeforeHeal:
                float afterHealStateTimer = 0.7f;
                stateTimer = afterHealStateTimer;
                state = State.AfterHeal;
                break;
            case State.AfterHeal:
                targetUnit.Heal(100);

                OnHealActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Heal";
    }

    public override List<GridPosition> GetAllValidGridPositionsListForThisAction()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxHealDistance; x <= maxHealDistance; x++)
        {
            for (int z = -maxHealDistance; z <= maxHealDistance; z++)
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

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // grid position is empty
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() != unit.IsEnemy())
                {
                    // both on the same team
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
            ActionValue = 5,
            GridPosition = gridPosition
        };
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.BeforeHeal;
        float beforeHealStateTimer = 0.7f;
        stateTimer = beforeHealStateTimer;

        OnHealActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }
}
