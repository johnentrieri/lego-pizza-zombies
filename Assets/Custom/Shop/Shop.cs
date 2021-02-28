using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] float playerShopDistance = 5.0f;
    [SerializeField] GameObject shopWindow;
    private PizzaToss player;
    private float distanceToPlayer;


    void Start()
    {
        player = FindObjectOfType<PizzaToss>();
        shopWindow.SetActive(false);
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log(distanceToPlayer);

        if ( distanceToPlayer <= playerShopDistance && Input.GetButtonDown("Shop")) {
            if (!shopWindow.activeSelf) {
                shopWindow.SetActive(true);
                player.SetPlayerInputEnabled(false);
            } else {
                shopWindow.SetActive(false);
                player.SetPlayerInputEnabled(true);
            }
        }

    }
}
