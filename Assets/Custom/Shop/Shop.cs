using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] float playerShopDistance = 5.0f;
    [SerializeField] GameObject shopWindow;
    [SerializeField] GameObject shopPrompt;    
    [SerializeField] GameObject defaultShopSelection;
    [SerializeField] string openShopPrompt = "Press Q to visit the Shop";
    [SerializeField] string closeShopPrompt = "Press SPACE to Purchase | Press Q to exit the Shop";
    
    private PizzaToss player;
    private float distanceToPlayer;
    private Text promptText;


    void Start()
    {
        promptText = shopPrompt.GetComponentInChildren<Text>();
        player = FindObjectOfType<PizzaToss>();
        CloseUpShop();
        shopPrompt.SetActive(false);        
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if ( distanceToPlayer <= playerShopDistance) {
            shopPrompt.SetActive(true);
            if (Input.GetButtonDown("Shop")) {
                if (!shopWindow.activeSelf) { OpenUpShop(); }
                else { CloseUpShop(); }
            } 
        } else {            
            shopPrompt.SetActive(false);
            CloseUpShop();            
        }
    }

    private void OpenUpShop() {
        shopWindow.SetActive(true);
        promptText.text = closeShopPrompt;
        player.SetPlayerInputEnabled(false);
        EventSystem.current.SetSelectedGameObject(defaultShopSelection);
    }

    private void CloseUpShop() {
        shopWindow.SetActive(false);
        promptText.text = openShopPrompt;
        player.SetPlayerInputEnabled(true);
    }
}
