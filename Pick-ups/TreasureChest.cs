using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InvantoryManager inventory;
    void Start()
    {
        inventory = FindObjectOfType<InvantoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        OpenTreasureChest();
        Destroy(gameObject);
    }

    public void OpenTreasureChest()
    {
        if(inventory.GetPossibleEvolutions().Count <= 0)
        {
            Debug.LogWarning("no available evolutions");
        }

        WeaponEvolutionBuleprint toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
