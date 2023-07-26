using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private Ramp firstRamp;
    [SerializeField] private List<GameObject> firstRampHidersList;
    [SerializeField] private List<GameObject> firstSectionEnemyList;

    private void Start()
    {
        firstRamp.OnRampOpened += FirstRamp_OnRampOpened;
    }

    private void FirstRamp_OnRampOpened(object sender, System.EventArgs e)
    {
        SetActiveGameObjectList(firstRampHidersList, false);
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
