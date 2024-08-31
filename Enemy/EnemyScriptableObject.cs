using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="EnemyScriptableObject", menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //base stats for enemy
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; set => damage = value; }
}
