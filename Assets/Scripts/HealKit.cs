using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealKit : MonoBehaviour, IInteractable
{
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material glowMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform healKitPrefab;

    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private float lerpSpeed = 1f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        meshRenderer.material = baseMaterial;

        originalScale = healKitPrefab.transform.localScale;
        targetScale = originalScale - Vector3.one * 0.1f;
    }

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, lerpSpeed) / lerpSpeed;
        meshRenderer.material.Lerp(baseMaterial, glowMaterial, lerp);
        healKitPrefab.localScale = Vector3.Lerp(originalScale, targetScale, lerp);

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
}
