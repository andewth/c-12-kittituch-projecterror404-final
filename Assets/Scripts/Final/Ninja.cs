﻿using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Ninja : Character
{
    Animator animator;
    [SerializeField] TextMeshProUGUI healthTxt, appleTxt, cherryTxt;


    protected int apple = 0;
    public int Apple => apple;

    protected float cherry = 0;
    public float Cherry => (float)cherry;


    HashSet<GameObject> nearbyMonster = new();
    public GameObject AppleSpawn, CherrySpawn;


    void Start()
    {
        Damage = 10;
        Health = 100;
        UpdateHelthTxt();
        UpdateAppleTxt();
        UpdateCherryTxt();
        animator = GetComponent<Animator>();
    }


    // Keep Apple & Cherry
    public void KeepUp(int count)
    {
        // Keep Apple
        apple += count;
        Health += 20;
        UpdateAppleTxt();
        UpdateHelthTxt();
        Debug.Log($"+ Keep Apple [ Hp : {Health} ]");
    }

    public void KeepUp(float count)
    {
        // Keep Cherry
        cherry += count;
        Damage += 5;
        UpdateCherryTxt();
        Debug.Log($"+ Keep Cherry [ Damage : {Damage} ]");
    }


    // When Player Near Monster & Out Monster
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            nearbyMonster.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            nearbyMonster.Remove(collision.gameObject);
        }
    }


    // Player Left-Click to Attack
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AnimationAttack();
        }
    }


    public override void AnimationAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("NinjaAttack"))
        {
            animator.SetTrigger("Attack");
            Attack();
        }
    }


    void Attack()
    {
        List<GameObject> monstersToAttack = new List<GameObject>(nearbyMonster);
        foreach (GameObject ninjaObject in monstersToAttack)
        {
            Character monster = ninjaObject.GetComponent<Character>();

            if (monster != null)
            {
                monster.TakeDamage(Damage);

                if (monster.Health <= 0)
                {
                    Destroy(monster.gameObject);
                    SpawnNewObject(monster.transform.position);
                    nearbyMonster.Remove(ninjaObject);
                }
            }
        }
    }




    // Spwn Cherry When Monster Dead
    private void SpawnNewObject(Vector3 spawnPosition)
    {
        // Spawn Apple
        if (AppleSpawn != null)
        {
            spawnPosition.x += Random.Range(2f, 5f);
            spawnPosition.y += Random.Range(1.0f, 1.5f);
            Instantiate(AppleSpawn, spawnPosition, Quaternion.identity);
        }

        // Spawn Cherry
        if (CherrySpawn != null)
        {
            spawnPosition.x += Random.Range(2f, 5f);
            spawnPosition.y += Random.Range(1.0f, 1.5f);
            Instantiate(CherrySpawn, spawnPosition, Quaternion.identity);
        }
    }




    // Update User Interface
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateHelthTxt();
    }

    void UpdateHelthTxt()
    {
        healthTxt.text = $"{Health}";
    }


    void UpdateAppleTxt()
    {
        appleTxt.text = $"{Apple}";
    }

    void UpdateCherryTxt()
    {
        cherryTxt.text = $"{Cherry}";
    }

}
