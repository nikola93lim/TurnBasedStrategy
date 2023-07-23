using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string SHOOT = "Shoot";
    private const string SWORD_SLASH = "SwordSlash";
    private const string HEAL = "Heal";

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject bat;

    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }

        if (TryGetComponent(out HealAction healAction))
        {
            healAction.OnHealActionStarted += HealAction_OnHealActionStarted;
            healAction.OnHealActionCompleted += HealAction_OnHealActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void HealAction_OnHealActionCompleted(object sender, EventArgs e)
    {
        StartCoroutine(Delay());
    }

    private void HealAction_OnHealActionStarted(object sender, EventArgs e)
    {
        EquipHeal();
        animator.SetTrigger(HEAL);
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipBat();
        animator.SetTrigger(SWORD_SLASH);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger(SHOOT);

        Transform bulletTransform = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Bullet bullet = bulletTransform.GetComponent<Bullet>();

        Vector3 targetUnitPosition = e.targetUnit.GetWorldPosition();

        targetUnitPosition.y = shootPoint.position.y;

        bullet.Setup(targetUnitPosition);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, true);
    }

    private void EquipRifle()
    {
        rifle.SetActive(true);
        bat.SetActive(false);
    }

    private void EquipBat()
    {
        bat.SetActive(true);
        rifle.SetActive(false);
    }

    private void EquipHeal()
    {
        rifle.SetActive(false);
    }
}
