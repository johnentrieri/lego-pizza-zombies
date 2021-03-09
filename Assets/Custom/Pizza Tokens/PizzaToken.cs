using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Behaviours.Actions;

public class PizzaToken : MonoBehaviour
{
    [SerializeField] int startingValue = 100;
    [SerializeField] float upgradeScaleFactor = 0.1f;
    [SerializeField] float upgradeVerticalBias = 0.065f;
    private PickupAction pickupAction;
    private PizzaToss pizzaToss;
    private bool isCollected = false;
    private EnemyManager lootPoolManager;
    private int currentValue;
    private Vector3 startingScale = new Vector3(1.0f,1.0f,1.0f);
    private int worthMultiplier = 1;

    void Start()
    {
        pizzaToss = FindObjectOfType<PizzaToss>();
        lootPoolManager = FindObjectOfType<EnemyManager>();
        pickupAction = GetComponentInChildren<PickupAction>();
        currentValue = startingValue * worthMultiplier;
    }

    void Update()
    {
        if (!isCollected && pickupAction.m_Collected) { 
            isCollected = true;
            pizzaToss.AddPizzaTokens(currentValue);
            Recycle();
        }
    }

    public void UpgradeToken(int levels) {
        worthMultiplier += levels;
        currentValue = startingValue * worthMultiplier;
        float scaleMultiplier = 1.0f + (upgradeScaleFactor * worthMultiplier);
        transform.localScale = new Vector3(scaleMultiplier,scaleMultiplier,scaleMultiplier);
        float newYPos = transform.position.y - (worthMultiplier * upgradeVerticalBias);
        transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
    }

    public void Reanimate(Vector3 position) {
        transform.position = position;
    }

    private void Recycle() {
        worthMultiplier = 1;
        currentValue = startingValue * worthMultiplier;
        transform.localScale = startingScale;
        lootPoolManager.AddToLootPool(this);
        pickupAction.m_Collected = false;
        pickupAction.m_Initialised = false;
        isCollected = false;   
    }    
}
