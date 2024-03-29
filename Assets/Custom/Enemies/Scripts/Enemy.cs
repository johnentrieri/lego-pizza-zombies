﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.LEGO.Minifig;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;    
    [SerializeField] float turnSpeed = 30.0f;
    [SerializeField] int damage = 1;
    [SerializeField] float attackRange = 5.0f;
    [SerializeField] float attackSpeed = 1.0f;

    private PizzaToss target; 
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float distanceToTarget;
    private float attackTimer = 0.0f;
    public bool isAlive;
    

    void Start()
    {
        isAlive = true;
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<PizzaToss>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    void Update()
    {
        FaceTarget();

        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget <= attackRange) { AttackTarget(); } 
        else { ChaseTarget(); }
    }

    void OnTriggerEnter(Collider other) {
        switch(other.tag) {
            case "Pizza": 
                Destroy(other.gameObject);
                ProcessDeath();
                break;
            default:
                break;
        }            
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public void SetAttackSpeed(float newAttackSpeed) {
        attackSpeed = newAttackSpeed;
    }
    public void ProcessDeath() {
        isAlive = false;
        EnemyExplode();
        GetComponentInParent<EnemyManager>().EnemyDeathHandler(this);
        Destroy(gameObject, 5.0f);
    }

    private void ChaseTarget() {
        if (!isAlive) { return; }
        animator.SetBool("Attack",false);
        attackTimer = 0.0f;
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void AttackTarget() {
        if (!isAlive) { return; }
        animator.SetBool("Attack",true);
        navMeshAgent.SetDestination(transform.position);
        if (attackTimer > 0.0f) {
            attackTimer -= Time.deltaTime;
        } else {
            target.InflictDamage(damage);
            attackTimer = 1.0f / attackSpeed;
        }
    }

    private void FaceTarget () {
        if (!isAlive) { return; }
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Vector3 newLookRotation = new Vector3(targetDirection.x,0,targetDirection.z);
        Quaternion lookRotation = transform.rotation;
        if (newLookRotation != Vector3.zero) { lookRotation = Quaternion.LookRotation(newLookRotation); }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }    

    private void EnemyExplode() {
        animator.gameObject.GetComponent<CharacterController>().enabled = true;
        animator.gameObject.GetComponent<MinifigController>().enabled = true;       
        animator.gameObject.GetComponent<MinifigController>().SetInputEnabled(false);
        animator.gameObject.GetComponent<MinifigController>().Explode();

        Collider[] colliders = animator.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider c in colliders) {
            c.enabled = false;
        }

        animator.gameObject.GetComponent<CharacterController>().enabled = false;
        animator.gameObject.GetComponent<MinifigController>().enabled = false;        
    }
}
