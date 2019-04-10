using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
   public enum WaypointType
   {
       Takeoff,
       Landing,
       CruiseRampUp,
       CruiseRampDown,
       Cruise,
       Pivot
   }

    public Vector3 Position;
    public Quaternion Rotation;
    public WaypointType Type;
    public bool WasPassed = false;

    void Start()
    {
        Position = transform.position;
        Rotation = transform.rotation;
    }

}
