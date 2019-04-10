using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationComputer : MonoBehaviour
{
    public LandingPad CurrentLandingPad;

    private FlightComputer flightComputer;
    private FlightDataVisualizer flightDataVisualizer;
    private PathNavigator pathNavigator;

    // Start is called before the first frame update
    void Start()
    {
        flightComputer = gameObject.GetComponent<FlightComputer>();
        pathNavigator = gameObject.GetComponent<PathNavigator>();
        flightDataVisualizer = gameObject.GetComponentInChildren<FlightDataVisualizer>();

        //Check if landing pad is selected or find 
        if (CurrentLandingPad == null)
        {
            SetCurrentLandingPadToClosest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<FlightPath> GetAvailableFlightPaths()
    {
        if (CurrentLandingPad != null)
        {
            return CurrentLandingPad.AvailableFlightPaths;
        }
        else
        {
            return null;
        }
        
    }

    public void SetFlightPath(int index)
    {
        pathNavigator.SetPath(CurrentLandingPad.AvailableFlightPaths[index]);
        Debug.Log("Flight path set");
    }

    public void LandingPadContact(LandingPad landingPad)
    {
        CurrentLandingPad = landingPad;
        landingPad.DeactivatePad();
        
        //End flight
        flightDataVisualizer.SetLandedInterface();
        
        //Destroy flight path
        pathNavigator.UnsetPath();
    }

    public void SetCurrentLandingPadToClosest()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("LandingPad");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        CurrentLandingPad = closest.GetComponent<LandingPad>();
    }
}
