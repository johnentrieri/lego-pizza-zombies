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
    private EnemyManager lootPoolManager;

    void Start()
    {
        pizzaToss = FindObjectOfType<PizzaToss>();
        lootPoolManager = FindObjectOfType<EnemyManager>();
        pickupAction = GetComponentInChildren<PickupAction>();
    }

    void Update()
    {
        if (!isCollected && pickupAction.m_Collected) { 
            isCollected = true;
            pizzaToss.AddPizzaTokens(value);
            Recycle();
            //Reanimate(posRNG);
        }
    }

    public void Reanimate(Vector3 position) {
        transform.position = position;
        //gameObject.SetActive(true);
    }

    private void Recycle() {
        lootPoolManager.AddToLootPool(this);
        pickupAction.m_Collected = false;
        pickupAction.m_Initialised = false;
        isCollected = false;
        //gameObject.SetActive(false);
        
    }

    
}
