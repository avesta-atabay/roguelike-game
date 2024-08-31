using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //references
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    
    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        //0-not moving !0-moving
        if(pm.moveDir.x !=0 || pm.moveDir.y != 0)
        {
            am.SetBool("Move",true);
            SpriteDirectionChecker();
        }else
        {
            am.SetBool("Move",false);
        }
    }
    void SpriteDirectionChecker()
    {
        //x>0-right/no flip x<00left/flip
        if(pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
