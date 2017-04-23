using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectController : SphericalMovementController {

	void Update () {
        if (_isActive)
        {
            RotateBy(0);
            TranslateBy(0, 0);
            Movement();
        }
    }
}
