﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

public class Goblin : Character, IAttack
{
    Animator Animator;
    HashSet<GameObject> nearbyNinja = new();

    public float checkInterval { get; set; }
    public float timer { get; set; }

    

    private void Start()
    {
        Damage = 25;
        Health = 100;
        checkInterval = 2.0f;
        timer = 0f;
        Animator = GetComponent<Animator>();
    }


    // When Monster Near Player & Out Player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ninja"))
        {
            nearbyNinja.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ninja"))
        {
            nearbyNinja.Remove(collision.gameObject);
        }
    }


    // Attack to Player And TakeDamage
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            if (nearbyNinja.Count > 0)
            {
                AnimationAttack();
                timer = 0f;
            }
        }
    }

    public override void AnimationAttack()
    {
        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("GoblinAttack"))
        {
            Animator.SetTrigger("GoblinAttack");
            Attack();
        }
    }


    public void Attack()
    {
        List<GameObject> monstersToAttack = new List<GameObject>(nearbyNinja);
        foreach (GameObject ninjaObject in monstersToAttack)
        {
            Ninja ninja = ninjaObject.GetComponent<Ninja>();

            if (ninja != null)
            {
                ninja.TakeDamage(Damage);
                if (ninja.Health <= 0)
                {
                    Destroy(ninja.gameObject);
                }
            }
        }
    }
}
