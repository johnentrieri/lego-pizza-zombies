using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Behaviours.Actions;

public class PizzaToken : MonoBehaviour
{
    [SerializeField] int value = 100;
    private PickupAction pickupAction;
    private PizzaToss pizzaToss;
    private bool isCollected = false;

    void Start()
    {
        pizzaToss = FindObjectOfType<PizzaToss>();
        pickupAction = GetComponentInChildren<PickupAction>();
    }

    void Update()
    {
        if (!isCollected && pickupAction.m_Collected) { 
            isCollected = true;
            pizzaToss.AddPizzaTokens(value);
            Destroy(gameObject,0.5f);
        }
    }
}
