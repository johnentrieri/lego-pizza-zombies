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

        if ( distanceToPlayer <= playerShopDistance && Input.GetButtonDown("Shop")) {
            if (!shopWindow.activeSelf) {
                shopWindow.SetActive(true);
                player.SetPlayerInputEnabled(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                shopWindow.SetActive(false);
                player.SetPlayerInputEnabled(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;;
            }
        }

    }
}
