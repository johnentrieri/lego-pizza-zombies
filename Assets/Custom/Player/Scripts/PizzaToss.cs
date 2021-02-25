using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;

public class PizzaToss : MonoBehaviour
{
    [SerializeField] GameObject pizzaPrefab;
    [SerializeField] int playerHealth = 10;

    private MinifigController minifigController;
    private Animator minifigAnimator;
    private float minifigForwardSpeed;
    private int pizzaTokens = 0;


    void Start()
    {
        minifigController = GetComponent<MinifigController>();
        minifigAnimator = GetComponent<Animator>();
        minifigForwardSpeed = minifigController.maxForwardSpeed;
    }

    void Update()
    {
        if ( !minifigController.airborne && Input.GetButtonDown("Toss")) {            
            minifigController.maxForwardSpeed = 0;
            minifigAnimator.SetTrigger("Toss");
            SpawnPizza();            
        }
    }
    public void InflictDamage(int dmg) {
        playerHealth -= dmg;
        if (playerHealth <= 0) { 
            playerHealth = 0;
            ProcessDeath(); 
        }
    }

    private void ProcessDeath() {
        minifigController.Explode();
    }
    
    private void TossComplete() {
        minifigController.maxForwardSpeed = minifigForwardSpeed;
    }

    private void SpawnPizza() {        
        Instantiate(pizzaPrefab,transform.position,transform.rotation);
    }
    public void AddPizzaTokens(int quantity) {        
        pizzaTokens += quantity;
    }
}
