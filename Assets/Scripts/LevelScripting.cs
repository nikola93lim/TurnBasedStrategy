using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [Header("Triggers")]
    [SerializeField] private Ramp firstRamp;
    [SerializeField] private Door secondDoor;

    [Header("Hiders")]
    [SerializeField] private List<GameObject> firstSectionHidersList;
    [SerializeField] private List<GameObject> secondSectionHidersList;
    [SerializeField] private List<GameObject> thirdSectionHidersList;
    [SerializeField] private List<GameObject> fourthSectionHidersList;
    [SerializeField] private List<GameObject> fifthSectionHidersList;

    [Header("Enemies")]
    [SerializeField] private List<GameObject> firstSectionEnemyList;
    [SerializeField] private List<GameObject> secondSectionEnemyList;
    [SerializeField] private List<GameObject> thirdSectionEnemyList;
    [SerializeField] private List<GameObject> fourthSectionEnemyList;
    [SerializeField] private List<GameObject> fifthSectionEnemyList;

    private void Start()
    {
        firstRamp.OnRampOpened += FirstRamp_OnRampOpened;
        secondDoor.OnDoorOpened += SecondDoor_OnDoorOpened;
    }

    private void SecondDoor_OnDoorOpened(object sender, System.EventArgs e)
    {
        SetActiveGameObjectList(secondSectionHidersList, false);
        SetActiveGameObjectList(secondSectionEnemyList, true);
    }

    private void FirstRamp_OnRampOpened(object sender, System.EventArgs e)
    {
        SetActiveGameObjectList(firstSectionHidersList, false);
        SetActiveGameObjectList(firstSectionEnemyList, true);
    }

    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }

}
