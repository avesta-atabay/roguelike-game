using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : WeaponController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedArrow = Instantiate(weaponData.Prefab);
        spawnedArrow.transform.position = transform.position; //assign the position to be the same as this object which is parented to the player
        spawnedArrow.GetComponent<ArrowBehaviour>().DirectionChecker(pm.lastMovedVector); //reference and set the direction
    }
}
