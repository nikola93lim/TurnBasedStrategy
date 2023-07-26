using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ramp : MonoBehaviour, IInteractable
{
    public event EventHandler OnRampOpened;

    private Animator animator;
    private GridPosition[] gridPositionArray;
    private bool isOpen;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPositionArray = new GridPosition[] {
            LevelGrid.Instance.GetGridPosition(transform.position),
            LevelGrid.Instance.GetGridPosition(transform.position + transform.right * 2),
            //LevelGrid.Instance.GetGridPosition(transform.position + transform.right * -2)
        };

        foreach (GridPosition gridPosition in gridPositionArray)
        {
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        }

        if (isOpen)
        {
            OpenRamp();
        }
        else
        {
            CloseRamp();
        }
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        timer = 1f;
        isActive = true;

        if (isOpen)
        {
            CloseRamp();
        }
        else
        {
            OpenRamp();
        }
    }

    private void OpenRamp()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);

        foreach (GridPosition gridPosition in gridPositionArray)
        {
            Pathfinding.Instance.SetGridPositionWalkable(gridPosition, true);
        }

        OnRampOpened?.Invoke(this, EventArgs.Empty);
    }

    private void CloseRamp()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);

        foreach (GridPosition gridPosition in gridPositionArray)
        {
            Pathfinding.Instance.SetGridPositionWalkable(gridPosition, false);
        }
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        Interact(onInteractionComplete);
    }
}
