using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;

public class FlightPath : MonoBehaviour
{
    public List<Waypoint> Waypoints;
    public List<GameObject> FlightTunnel;
    public GameObject WaypointPrefab;
    public LandingPad DestinationLandingPad;
    public int PointDistanceDivider = 50;
    public Waypoint CurrentWaypoint;
    public Waypoint NextWaypoint;
    public string StartPoint;
    public string DestinationPoint;
    public int WaypointIndexer = 0;
    private float pointDistance;
    private int intermittentRings = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Retrieve all waypoints
        foreach (Transform child in this.transform)
        {
            if (!child.name.Contains("line"))
            {
                Waypoints.Add(child.gameObject.GetComponent<Waypoint>());
            }          
        }
    }

    public void DrawFlightTunnel()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            CurrentWaypoint = Waypoints[i];

            //Don't draw rings over landing pads
            if (CurrentWaypoint.Type == Waypoint.WaypointType.Landing)
            {
                DestinationLandingPad.ActivatePad();
                continue;
            }

            NextWaypoint = null;
            if (i < Waypoints.Count - 1)
            {
                NextWaypoint = Waypoints[i + 1];
            }

            //Spawn rings at point
            SpawnRing(Waypoints[i].Position, Waypoints[i].Rotation);
            Waypoints.Last().gameObject.tag = "Waypoint";

            //Spawn intermittent rings
            if (NextWaypoint != null)
            {
                pointDistance = Vector3.Distance(CurrentWaypoint.Position, NextWaypoint.Position);
                intermittentRings = (int) pointDistance / PointDistanceDivider;

                if (intermittentRings > 0)
                {
                    for (int j = 1; j < intermittentRings; j++)
                    {
                        Vector3 intermittentPosition = Vector3.Lerp(CurrentWaypoint.Position, NextWaypoint.Position, (1f / intermittentRings) * j);
                        Quaternion intermittentRotation = Quaternion.Lerp(CurrentWaypoint.Rotation, NextWaypoint.Rotation, (1f / intermittentRings) * j);
                        SpawnIntermittentRing(intermittentPosition, intermittentRotation);
                    }
                }
            }
            
        }

        CurrentWaypoint = Waypoints[WaypointIndexer];
        NextWaypoint = Waypoints[WaypointIndexer + 1];
    }

    public void DestroyFlightTunnel()
    {
        StartCoroutine(DestroyFlightTunnelRings());
    }

    public void ReversePath()
    {
        //Switch names
        string tmp = StartPoint;
        StartPoint = DestinationPoint;
        DestinationPoint = tmp;

        //Reverse waypoints
        //TODO
    }

    private void SpawnRing(Vector3 parentPosition, Quaternion parentRotation)
    {
        GameObject newRing = Instantiate(WaypointPrefab, parentPosition, parentRotation);
        newRing.transform.rotation = new Quaternion(0,0,0,0);
        newRing.transform.Rotate(new Vector3(90, 90 + parentRotation.eulerAngles.y, 0));
        FlightTunnel.Add(newRing);
    }

    private void SpawnIntermittentRing(Vector3 parentPosition, Quaternion parentRotation)
    {
        GameObject newRing = Instantiate(WaypointPrefab, parentPosition, parentRotation);
        newRing.transform.rotation = new Quaternion(0, 0, 0, 0);
        newRing.transform.Rotate(new Vector3(90, 90 + parentRotation.eulerAngles.y, 0));
        newRing.GetComponent<Collider>().enabled = false;
        FlightTunnel.Add(newRing);
    }

    public IEnumerator DestroyFlightTunnelRings()
    {
        foreach (GameObject ring in FlightTunnel)
        {
            Destroy(ring);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
