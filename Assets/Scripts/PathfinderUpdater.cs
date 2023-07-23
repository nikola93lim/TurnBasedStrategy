using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyCrateDestroyed += DestructibleCrate_OnAnyCrateDestroyed;
    }

    private void DestructibleCrate_OnAnyCrateDestroyed(object sender, System.EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;

        GridPosition gridPosition = destructibleCrate.GetGridPosition();

        Pathfinding.Instance.SetGridPositionWalkable(gridPosition, true);
    }
}
