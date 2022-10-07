using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    private float idleTimer;

    private float idleDuration=10;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Debug.Log("idle");
        Idle(); 

        if(enemy.Target !=null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(UnityEngine.Collider2D other)
    {
        
        if(other.tag=="Knife")
        {
            enemy.Target = player.Instance.gameObject;
        }
    }
    private void Idle()
    {
        enemy.MyAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }

    }

}
