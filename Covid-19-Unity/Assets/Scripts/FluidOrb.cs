using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidOrb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // refill fluid capsule
            FluidCapsule.instance.RefillFluid();
            Destroy(this.gameObject);
        }
    }
}
