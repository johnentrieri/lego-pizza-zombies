using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;
using UnityEngine.UI;

public class PizzaToss : MonoBehaviour
{
    [SerializeField] GameObject pizzaPrefab;
    [SerializeField] int playerHealth = 10;
    [SerializeField] float tossRate = 2f;
    [SerializeField] AudioClip hurtAudioClip;
    [SerializeField] float blinkTime = 0.5f;
    [SerializeField] float blinkPeriod = 0.1f;
    [SerializeField] TMPro.TextMeshProUGUI tokenValueText;
    [SerializeField] Image healthBarImg;

    private MinifigController minifigController;
    private Animator minifigAnimator;
    private Renderer[] playerRenderers;
    private AudioSource audioSource;
    private float minifigForwardSpeed;
    private float tossTimer;
    private int pizzaTokens = 0;
    private bool isBlinking = false;
    private int maxHP;

    void Start()
    {
        minifigController = GetComponent<MinifigController>();
        minifigAnimator = GetComponent<Animator>();
        playerRenderers = GetComponentsInChildren<Renderer>();  
        audioSource = GetComponent<AudioSource>();      
        minifigForwardSpeed = minifigController.maxForwardSpeed;
        tokenValueText.text = pizzaTokens.ToString();
        maxHP = playerHealth;
        healthBarImg.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
    }

    void Update()
    {
        if ( tossTimer > 0 ) { tossTimer -= Time.deltaTime; }
        if ( !minifigController.airborne && Input.GetButtonDown("Toss")) {            
            minifigController.maxForwardSpeed = 0;
            minifigAnimator.SetTrigger("Toss");
            SpawnPizza();            
        }
    }
    public void InflictDamage(int dmg) {
        if (isBlinking) { return; }
        audioSource.PlayOneShot(hurtAudioClip);
        playerHealth -= dmg;
        float hpPercent = (1.0f * playerHealth) / (1.0f * maxHP);
        healthBarImg.transform.localScale = new Vector3(hpPercent,1.0f,1.0f);
        StartCoroutine( Blink() );        

        if (playerHealth <= 0) { 
            playerHealth = 0;
            ProcessDeath(); 
        }
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
        minifigController.maxForwardSpeed = minifigForwardSpeed;
    }

    private void SpawnPizza() {     
        if (tossTimer > 0) { return; }   
        Instantiate(pizzaPrefab,transform.position,transform.rotation);
        tossTimer = 1.0f / tossRate;
    }
    public void AddPizzaTokens(int quantity) {        
        pizzaTokens += quantity;
        tokenValueText.text = pizzaTokens.ToString();
    }
}
