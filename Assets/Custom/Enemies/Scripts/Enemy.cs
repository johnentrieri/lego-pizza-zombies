using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.LEGO.Minifig;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 8.0f;    
    [SerializeField] float turnSpeed = 30.0f;  
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
        FaceTarget();
        navMeshAgent.SetDestination(target.transform.position);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Pizza") {
            Destroy(other.gameObject);
            ProcessDeath();
        }
    }
    private void FaceTarget () {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDirection.x,0,targetDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ProcessDeath() {
        EnemyExplode();
        GetComponentInParent<EnemyManager>().EnemyDeathHandler(gameObject);
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
