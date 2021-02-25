using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.LEGO.Minifig;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;
    
    private MinifigController target; 
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<MinifigController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Projectile") {
            Destroy(other.gameObject);
            ProcessDeath();
        }
    }

    private void ProcessDeath() {
        animator.SetTrigger("Die");
        Destroy(gameObject,0.7f);
    }
}
