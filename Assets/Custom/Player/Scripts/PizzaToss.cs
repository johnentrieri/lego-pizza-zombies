using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;

public class PizzaToss : MonoBehaviour
{
    private MinifigController minifigController;
    private Animator minifigAnimator;
    private float minifigForwardSpeed;
    [SerializeField] GameObject pizzaPrefab;

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

    private void TossComplete() {
        minifigController.maxForwardSpeed = minifigForwardSpeed;
    }

    private void SpawnPizza() {        
        Instantiate(pizzaPrefab,transform.position,transform.rotation);
    }
}
