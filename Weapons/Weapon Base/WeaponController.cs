using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //base script for all weapon controllers
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    

    protected PlayerMovement pm;

    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CoolDownDuration;//at the start set the current cooldown to the cooldown duration
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        currentCooldown = weaponData.CoolDownDuration;
    }
}
