using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    //base script of all projectile behaviours
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    //current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCoolDownDuration;
    protected float currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCoolDownDuration = weaponData.CoolDownDuration;
        currentPierce = weaponData.Pierce;
    }
    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirX= direction.x;
        float dirY= direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation =transform.rotation.eulerAngles;

        if(dirX < 0 && dirY==0)//left
        {
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY <0)//down
        {
            scale.y = scale.y * -1;
            rotation.z = 180f;
        }
        else if (dirX == 0 && dirY > 0)//up
        {
            rotation.z = 180f;
        }
        else if (dirX > 0 && dirY > 0)//right up
        {
            scale.y = scale.y * -1;
            rotation.z = -45f;
            
        }
        else if (dirX > 0 && dirY < 0)//right down
        {
            rotation.z = 45f;
        }
        else if (dirX < 0 && dirY > 0)//left up
        {
            scale.y =scale.y * -1;
            rotation.z = 45f;
        }
        else if (dirX < 0 && dirY < 0)//left down
        {
            rotation.z = -45f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); // cant simply set the vector because cannot convert
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //refference the script from the collider and deal damge using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }
    void ReducePierce() //destroy once the pierce reaches 0
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
