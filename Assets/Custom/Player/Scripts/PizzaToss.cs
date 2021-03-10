using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;
using UnityEngine.UI;

public class PizzaToss : MonoBehaviour
{
    [SerializeField] GameObject pizzaPrefab;
    [SerializeField] GameObject enemyManager;
    [SerializeField] int playerHealth = 10;
    [SerializeField] int hotChocolateCost = 1000;
    [SerializeField] float playerSpeed = 10;
    [SerializeField] float maxSpeed = 20;
    [SerializeField] int speedPotionCost = 500;
    [SerializeField] float tossRate = 0.5f;
    [SerializeField] float maxTossRate = 3.0f;
    [SerializeField] float tossSpeed = 10.0f;
    [SerializeField] float maxTossSpeed = 20.0f;
    [SerializeField] float tossRange = 20.0f;
    [SerializeField] float maxTossRange = 20.0f;
    [SerializeField] int pizzaSlingerCost = 500;
    [SerializeField] int dynamiteCost = 10000;
    [SerializeField] AudioClip hurtAudioClip;
    [SerializeField] AudioClip newWaveAudioClip;
    [SerializeField] float blinkTime = 0.5f;
    [SerializeField] float blinkPeriod = 0.1f;
    [SerializeField] TMPro.TextMeshProUGUI tokenValueText;
    [SerializeField] Text speedPotionCostText;
    [SerializeField] Text pizzaSlingerCostText;
    [SerializeField] Text hotChocolateCostText;
    [SerializeField] Text dynamiteCostText;
    [SerializeField] Image healthBarImg;

    private MinifigController minifigController;
    private Animator minifigAnimator;
    private Renderer[] playerRenderers;
    private AudioSource audioSource;
    private Narrator narrator;
    private float tossTimer;
    private int pizzaTokens = 0;
    private bool isBlinking = false;
    private bool hasIssuedShopNarration = false;
    private int maxHP;

    void Start()
    {
        minifigController = GetComponent<MinifigController>();
        minifigAnimator = GetComponent<Animator>();
        playerRenderers = GetComponentsInChildren<Renderer>();  
        audioSource = GetComponent<AudioSource>();
        narrator = FindObjectOfType<Narrator>();

        minifigController.maxForwardSpeed = playerSpeed;
        tokenValueText.text = pizzaTokens.ToString();
        maxHP = playerHealth;
        UpdateGUI();
    }

    void Update()
    {
        // Decrement Toss Timer (Toss Rate)
        if ( tossTimer > 0 ) { tossTimer -= Time.deltaTime; }

        // Toss Pizza
        else if ( minifigController.GetInputEnabled() && !minifigController.airborne && Input.GetButtonDown("Toss")) {            
            minifigController.maxForwardSpeed = 0;
            minifigAnimator.SetTrigger("Toss");
            SpawnPizza();            
        }

        // Fall-off-Map Handler
        if (transform.position.y < -10) {
            InflictDamage(playerHealth);
        }

        // Shop Narration
        if ( !hasIssuedShopNarration ) {
            if (pizzaTokens >= PriceOfLowestCostItem()) {
                narrator.Narrate("Looks like you've collected a couple of PIZZA TOKENS", 5.0f, 0.5f );
                narrator.Narrate("Head back to the SHOP to trade them in for UPGRADES", 8.0f, 6.0f );
                hasIssuedShopNarration = true;
            }
        }
    }
    
    public void InflictDamage(int dmg) {
        if (isBlinking) { return; }
        audioSource.PlayOneShot(hurtAudioClip);
        playerHealth -= dmg;
        if (playerHealth <= 0) {  playerHealth = 0; }
        UpdateGUI();
        StartCoroutine( Blink() );
        if (playerHealth <= 0) { ProcessDeath(); }
    }

    public void PlayNewWaveAudio() {
        audioSource.PlayOneShot(newWaveAudioClip);
    }
    
    public void SetPlayerInputEnabled(bool enabled) {
        minifigController.SetInputEnabled(enabled);
    }

    public void ApplySpeedPotion() {
        if (pizzaTokens >= speedPotionCost && playerSpeed < maxSpeed) {
            SubtractPizzaTokens(speedPotionCost);
            speedPotionCost *= 2;            
            playerSpeed += 2;
            if (playerSpeed > maxSpeed) { playerSpeed = maxSpeed; }
            minifigController.maxForwardSpeed = playerSpeed;
            UpdateGUI();
        }
    }
    public void ApplyPizzaSlinger() {
        if (pizzaTokens >= pizzaSlingerCost && tossRate < maxTossRate) {
            SubtractPizzaTokens(pizzaSlingerCost);
            pizzaSlingerCost *= 2;            
            tossRate += 0.5f;
            tossSpeed += 2.0f;
            tossRange += 2.0f;
            if (tossRate > maxTossRate) { tossRate = maxTossRate; }
            if (tossSpeed > maxTossSpeed) { tossSpeed = maxTossSpeed; }
            if (tossRange > maxTossRange) { tossRange = maxTossRange; }
            UpdateGUI();
        }
    }

    public void ApplyHotChocolate() {
        if (pizzaTokens >= hotChocolateCost && playerHealth < maxHP) {
            SubtractPizzaTokens(hotChocolateCost);
            hotChocolateCost *= 2;
            playerHealth = maxHP;
            healthBarImg.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            UpdateGUI();
        }
    }
    public void ApplyDynamite() {
        if (pizzaTokens < dynamiteCost) { return; }

        List<Enemy> activeEnemies = new List<Enemy>();
        foreach( Enemy e in enemyManager.GetComponentsInChildren<Enemy>() ) {
            if (e.gameObject.activeSelf && e.isAlive) { activeEnemies.Add(e); }
        }        
        
        if (activeEnemies.Count <= 0 ) { return; }

        foreach( Enemy e in activeEnemies ) { e.ProcessDeath(); }

        SubtractPizzaTokens(dynamiteCost);
        dynamiteCost *= 2;

        UpdateGUI();

    }
    public void AddPizzaTokens(int quantity) {        
        pizzaTokens += quantity;
        UpdateGUI();  
    }

    private void SubtractPizzaTokens(int quantity) {        
        pizzaTokens -= quantity;
        if (pizzaTokens < 0) { pizzaTokens = 0; }
        UpdateGUI();
    }    

    private IEnumerator Blink() {
        isBlinking = true;
        float blinkTimer = blinkTime;
        while ( blinkTimer > 0) {
            foreach(Renderer r in playerRenderers) { r.enabled = !r.enabled; }
            blinkTimer -= blinkPeriod;
            yield return new WaitForSeconds(blinkPeriod);
        }
        isBlinking = false;
    }

    private void ProcessDeath() {
        minifigController.Explode();
    }
    
    private void TossComplete() {
        // Triggered on Animation Event (Toss)
        minifigController.maxForwardSpeed = playerSpeed;
    }

    private void SpawnPizza() {     
        if (tossTimer > 0) { return; }   
        GameObject pizza = Instantiate(pizzaPrefab,transform.position,transform.rotation);
        pizza.GetComponent<Pizza>().SetSpeedandRange(tossSpeed,tossRange);
        tossTimer = 1.0f / tossRate;
    }
    
    private void UpdateGUI() {

        // Health
        float hpPercent = (1.0f * playerHealth) / (1.0f * maxHP);
        healthBarImg.transform.localScale = new Vector3(hpPercent,1.0f,1.0f);

        // Crystals
        tokenValueText.text = pizzaTokens.ToString();

        // Speed Potion Shop Price
        if (playerSpeed >= maxSpeed) { speedPotionCostText.text = "MAX"; }
        else { speedPotionCostText.text = speedPotionCost.ToString(); }

        // Speed Potion Shop Price
        if ( tossRate >= maxTossRate && tossSpeed >= maxTossSpeed && tossRange >= maxTossRange) { pizzaSlingerCostText.text = "MAX"; }
        else { pizzaSlingerCostText.text = pizzaSlingerCost.ToString(); }

        // Speed Potion Shop Price
        if (playerHealth >= maxHP) { hotChocolateCostText.text = "FULL"; }
        else { hotChocolateCostText.text = hotChocolateCost.ToString(); }

        // Dynamite Shop Price
        dynamiteCostText.text = dynamiteCost.ToString();
    }

    private int PriceOfLowestCostItem() {
        int minCost = 99999;
        if (speedPotionCost < minCost) { minCost = speedPotionCost; }
        if (pizzaSlingerCost < minCost) { minCost = pizzaSlingerCost; }
        if (hotChocolateCost < minCost) { minCost = hotChocolateCost; }
        if (dynamiteCost < minCost) { minCost = dynamiteCost; }
        return minCost;
    }
}
