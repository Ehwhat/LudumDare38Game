using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupController : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        OnPickup(other);
    }

    protected abstract void OnPickup(Collider recipient);

}
