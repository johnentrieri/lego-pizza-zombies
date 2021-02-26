using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;

public class PizzaToss : MonoBehaviour
{
    [SerializeField] GameObject pizzaPrefab;
    [SerializeField] int playerHealth = 10;
    [SerializeField] AudioClip hurtAudioClip;

    private MinifigController minifigController;
    private Animator minifigAnimator;
    private Renderer[] playerRenderers;
    private AudioSource audioSource;
    private float minifigForwardSpeed;
    private int pizzaTokens = 0;
    [SerializeField] float blinkTime = 0.5f;
    [SerializeField]  float blinkPeriod = 0.1f;
    private bool isBlinking = false;

    void Start()
    {
        minifigController = GetComponent<MinifigController>();
        minifigAnimator = GetComponent<Animator>();
        playerRenderers = GetComponentsInChildren<Renderer>();  
        audioSource = GetComponent<AudioSource>();      
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
        if (isBlinking) { return; }
        audioSource.PlayOneShot(hurtAudioClip);
        playerHealth -= dmg;
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
        Instantiate(pizzaPrefab,transform.position,transform.rotation);
    }
    public void AddPizzaTokens(int quantity) {        
        pizzaTokens += quantity;
    }
}
