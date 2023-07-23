using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Transform bulletHitVFXPrefab;

    private Vector3 targetPoint;

    public void Setup(Vector3 targetPoint)
    {
        this.targetPoint = targetPoint;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPoint - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPoint);

        float moveSpeed = 200f;
        transform.position += moveSpeed * Time.deltaTime * moveDir;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPoint);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPoint;
            trail.transform.parent = null;
            Destroy(gameObject);

            Instantiate(bulletHitVFXPrefab, targetPoint, Quaternion.identity);
        }
    }
}
