using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.LEGO.Minifig;

public class Enemy : MonoBehaviour
{
    private MinifigController target; 
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<MinifigController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
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
        Destroy(gameObject);
    }
}
