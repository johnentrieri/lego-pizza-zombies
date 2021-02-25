using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;

public class Pizza : MonoBehaviour
{
    [SerializeField] float range = 10.0f;
    [SerializeField] float speed = 8.0f;
    private bool isFlying = false;
    private Animator pizzaAnimator;
    private Vector3 startingPosition;

void Start() {
    pizzaAnimator = GetComponent<Animator>();
    startingPosition = transform.position;
}
    void Update() {
        if ( Vector3.Distance(startingPosition,transform.position) >= range) {
            isFlying = false;
            Destroy(gameObject);
        }

        if (isFlying) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void StartProjectile() {
        startingPosition = transform.position;
        pizzaAnimator.enabled = false;
        isFlying = true;
    }

}
