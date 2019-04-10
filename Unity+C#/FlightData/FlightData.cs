using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Assets.Source.ManualFlight;
using UnityEngine;

namespace Assets.Source.FlightData
{
    public class FlightData
    {
        public float Altitude;
        public float Heading;
        public float HeadingDegrees;
        public float Elevation;
        public float ElevationDegrees;
        public float Roll;
        public float RollDegrees;
        public float AirSpeed = 0;
        public int BatteryStatus = 100;
        public float ThrottleStatus = 0;
        public int Rpm = 0;
        public List<EngineStatus> EngineStatuses;
        public float VerticalSpeed;
        public Transform NextWaypointTransform;
        private Vector3 lastPosition;
        private readonly Transform plane;
        private readonly float metersToKnotsCoefficient = 0.514f;
        private readonly EngineController engineController;
        private readonly PathNavigator pathNavigator;

        public FlightData(Transform plane, EngineController engineController, PathNavigator pathNavigator)
        {
            this.plane = plane;
            this.engineController = engineController;
            this.pathNavigator = pathNavigator;
        }

        public void UpdateFlightData()
        {
            Altitude = plane.position.y;
            Heading = plane.rotation.y;
            HeadingDegrees = plane.rotation.eulerAngles.y;
            Elevation = plane.rotation.x;
            ElevationDegrees = plane.rotation.eulerAngles.x;
            Roll = plane.rotation.z;
            RollDegrees = plane.rotation.eulerAngles.z;
            //Airspeed in knots
            AirSpeed = plane.GetComponent<Rigidbody>().velocity.magnitude * metersToKnotsCoefficient;
            
            EngineStatuses = engineController.GetEngineStatuses();
            Rpm = engineController.Rpm;
            ThrottleStatus = engineController.ThrottleBase;
            //VS in m/s
            VerticalSpeed = (plane.position.y - lastPosition.y) / Time.deltaTime;

            if (pathNavigator.NextWaypoint)
            {
                NextWaypointTransform = pathNavigator.NextWaypoint.transform;
            }

            //Update last position !HAS TO BE LAST!
            lastPosition = plane.position;
        }
    }
}
