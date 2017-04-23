using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : PickupController
{
    protected override void OnPickup(Collider recipient)
    {
        PlayerShipController controller = recipient.transform.root.GetComponent<PlayerShipController>();
        if(controller != null)
        {
            controller.AddFuel();
            Destroy(gameObject);
        }
    }
}
