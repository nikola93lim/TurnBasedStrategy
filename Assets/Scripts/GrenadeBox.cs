using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBox : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject grenade;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
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
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        Interact(onInteractionComplete);

        unit.GetAction<GrenadeAction>().RefillGrenades();
        grenade.SetActive(false);
    }
}
