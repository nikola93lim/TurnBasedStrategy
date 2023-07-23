using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private bool isGreen;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private float timer;
    private bool isActive;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        if (isGreen)
        {
            SetGreenMaterial();
        }
        else
        {
            SetRedMaterial();
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
        timer = 0.5f;
        isActive = true;

        if (isGreen)
        {
            SetRedMaterial();
        }
        else
        {
            SetGreenMaterial();
        }
    }

    private void SetGreenMaterial()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetRedMaterial()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }
}
