﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.LEGO.Minifig;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;    
    private PizzaToss target; 
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<PizzaToss>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Pizza") {
            Destroy(other.gameObject);
            ProcessDeath();
        }
    }

    private void ProcessDeath() {
        EnemyExplode();
        Destroy(gameObject, 5.0f);
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
