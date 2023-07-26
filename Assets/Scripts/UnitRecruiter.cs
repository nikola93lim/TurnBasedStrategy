using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRecruiter : MonoBehaviour, IInteractable
{
    [SerializeField] private Unit unitPrefab;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        Pathfinding.Instance.SetGridPositionWalkable(gridPosition, false);
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
            Destroy(gameObject);
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        timer = 1f;
        isActive = true;

        RecruitUnit();
    }

    private void RecruitUnit()
    {
        Instantiate(unitPrefab, transform.position, Quaternion.identity);
        Pathfinding.Instance.SetGridPositionWalkable(gridPosition, true);
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        Interact(onInteractionComplete);
    }
}
