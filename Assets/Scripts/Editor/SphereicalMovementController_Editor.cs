using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SphericalMovementController))]
public class SphereicalMovementController_Editor : Editor {

    bool _updatePosition;
    SphericalMovementController con;

    void OnEnable()
    {
        con = (SphericalMovementController)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button(new GUIContent("Move On Planet"))){
            con.SetPosition(con._positionOnPlane.x, con._positionOnPlane.y);
            con.RotateBy(con._rotationalAngle);
            con.Movement();
        }
        _updatePosition = GUILayout.Toggle(_updatePosition, new GUIContent("Update Position"));
        if(_updatePosition){
            con.SetPosition(con._positionOnPlane.x, con._positionOnPlane.y);
            con.RotateBy(con._rotationalAngle);
            con.Movement();
        }

    }

}
