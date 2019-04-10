using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;

public class PathNavigator : MonoBehaviour
{
    public GameObject PathGameObject;
    public GameObject NextWaypoint;
    public float CruiseSpeedMinimum = 10f;
    public int CruiseSpeedMultiplier = 20;
    public int ManeuverSpeedMax = 5;
    private FlightPath currentPath;
    private bool navigated = false;
    private float speed;
    private float totalJourneyDuration; //Would need to mark waypoints to differentiate main route and takeoff/landing

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Handles passing though a waypoint during manual flight
    public void WaypointPassed()
    {
        if (!currentPath.Waypoints[currentPath.WaypointIndexer].WasPassed)
        {
            currentPath.Waypoints[currentPath.WaypointIndexer].WasPassed = true;
            currentPath.WaypointIndexer++;
            currentPath.CurrentWaypoint = currentPath.NextWaypoint;
            currentPath.NextWaypoint = currentPath.Waypoints[currentPath.WaypointIndexer];
            NextWaypoint = currentPath.NextWaypoint.gameObject;
        }    
    }

    public void NavigatePath()
    {
        StartCoroutine(LinearMovementRotation());
    }

    public void SetPath(FlightPath path)
    {
        currentPath = path;
        currentPath.DrawFlightTunnel();
    }

    public void UnsetPath()
    {
        if (currentPath != null)
        {
            currentPath.DestroyFlightTunnel();
            currentPath = null;
        } 
    }

    public float GetDistanceFromPath()
    {
        //Get heading
        Vector3 heading = currentPath.NextWaypoint.Position - currentPath.CurrentWaypoint.Position;
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Point projection
        Vector3 lhs = this.transform.position - currentPath.CurrentWaypoint.Position;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        Vector3 closestPoint = currentPath.CurrentWaypoint.Position + heading * dotP;

        return Vector3.Distance(this.transform.position, closestPoint);
    }

    private IEnumerator LinearMovementRotation()
    {
        float t = 0;
        foreach (var waypoint in currentPath.Waypoints)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.Position;
            Quaternion startRotation = transform.rotation;

            //Calculate travel time
            float travelTime = 10f; //TODO
            float time = 0f;
            float distance;
            float distanceTraveled;

            distance = Mathf.Abs(Vector3.Distance(transform.position, endPosition));
            //TODO Use distance to calculate cruise multiplier
            distanceTraveled = 0f;
            time = 0f;

            while (transform.position != waypoint.Position)
            {
                speed = GetSpeedForWaypoint(waypoint.Type, distanceTraveled/distance);
                transform.position = Vector3.MoveTowards(transform.position, waypoint.Position, speed * Time.deltaTime);
                time += Time.deltaTime;
                distanceTraveled += speed * Time.deltaTime;

                //Flip rotation by 180 degrees on Y axis
                transform.rotation = Quaternion.Lerp(startRotation, waypoint.Rotation*Quaternion.AngleAxis(180, Vector3.up), distanceTraveled / distance);

                yield return new WaitForEndOfFrame();
            }

            //Cull float error
            transform.position = waypoint.Position;
            transform.rotation = waypoint.Rotation * Quaternion.AngleAxis(180, Vector3.up);
        }
        //Debug.Log(t);
        yield return null;
    }

    private float GetSpeedForWaypoint(Waypoint.WaypointType type, float fractionOfJourney)
    {
        if (type == Waypoint.WaypointType.Takeoff)
        {
            return getLinearRampUp(fractionOfJourney);
        }
        else if (type == Waypoint.WaypointType.CruiseRampUp)
        {
            return (CruiseSpeedMinimum - ManeuverSpeedMax) + getLinearRampUp(fractionOfJourney);
        }
        else if (type == Waypoint.WaypointType.Cruise)
        {
            return getParabolicSpeed(fractionOfJourney);
        }
        else if (type == Waypoint.WaypointType.CruiseRampDown)
        {
            return ManeuverSpeedMax + getLinearRampDown(fractionOfJourney);
        }
        else
        {
            //Landing
            return getLinearRampDown(fractionOfJourney);
        }
    }

    //For x = <0;1>
    private float getParabolicSpeed(float x)
    {
        //-10x^2 + 10x + offset
        return -CruiseSpeedMultiplier * Mathf.Pow(x, 2f) + CruiseSpeedMultiplier * x + CruiseSpeedMinimum;
    }

    private float getLinearRampUp(float x)
    {
        return ManeuverSpeedMax * x + 0.1f;
    }

    private float getLinearRampDown(float x)
    {
        return ManeuverSpeedMax - getLinearRampUp(x);
    }

}
