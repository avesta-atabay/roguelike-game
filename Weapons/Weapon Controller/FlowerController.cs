using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerController : WeaponController
{
    
    protected override void Start()
    {
        base.Start();
    }

    
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedFlower = Instantiate(weaponData.Prefab);
        spawnedFlower.transform.position = transform.position; //assign the position to be the same as this object which is paranted to the player
        spawnedFlower.transform.parent = transform; //sp that is spawns bellow this object
    }
}
