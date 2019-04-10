using System.Collections;
using System.Collections.Generic;
using Assets.Source.FlightData;
using UnityEngine;

public class FlightComputer : MonoBehaviour
{
    //Flight computer settings
    public float MaxCourseDeviation = 40f;
    public bool IsLanded = true;

    //Flight computer internal components
    private FlightData currentFlightData;
    private Transform plane;
    private FlightDataVisualizer flightDataVisualizer;
    private EngineController engineController;
    private ControllerInputs controllerInputs;
    private PathNavigator pathNavigator;
    private NavigationComputer navigationComputer;
    private SoundController soundController;
    private bool manualFlight = false;
    private int throttleInputModifier = 3;

    //Warnings
    private bool offCourseWarning = false;
    private bool descentWarning = false;

    // Start is called before the first frame update
    void Start()
    {
        plane = transform;
        engineController = gameObject.GetComponent<EngineController>();
        flightDataVisualizer = gameObject.GetComponentInChildren<FlightDataVisualizer>();
        controllerInputs = GameObject.Find("VRActions").GetComponent<ControllerInputs>();
        pathNavigator = this.GetComponent<PathNavigator>();
        navigationComputer = this.GetComponent<NavigationComputer>();
        currentFlightData = new FlightData(plane, engineController, pathNavigator);
        flightDataVisualizer.CurrentFlightData = currentFlightData;
        soundController = GetComponentInChildren<SoundController>();
        soundController.FlightComputerRef = this;
    }

    // Update is called once per frame
    void Update()
    {
        currentFlightData.UpdateFlightData();
        if (manualFlight)
        {
            ProcessThrottleChange();
            ProcessDirectionChange();
            ProcessRudderChange();
            //CheckCourse();
        }
        
        //Check for warnings
        if (currentFlightData.VerticalSpeed < -3 && !descentWarning)
        {
            soundController.ToggleDescentWarning();
            descentWarning = true;
        }
        else if (currentFlightData.VerticalSpeed > -3 && descentWarning)
        {
            soundController.ToggleDescentWarning();
            descentWarning = false;
        }
    }

    //Trigger entry detection
    public void OnTriggerEnter(Collider col)
    {
        //Landing pad contact
        if (col.gameObject.tag == "Pad" && IsLanded == false)
        {
            Debug.Log("landing pad enter: " + col.gameObject.name);
            LandingPad lp = col.gameObject.GetComponentInChildren<LandingPad>();
            Debug.Log("ENTER: " + col.gameObject.name + "CURRENT: "+ navigationComputer.CurrentLandingPad.name);
            if (lp != navigationComputer.CurrentLandingPad)
            {
                //Debug.Log("VELOCITY: "+ plane.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
                //if (plane.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1f)
                //{
                    Debug.Log("CONTACT");
                    StartCoroutine(LandingPadContactHandler(lp));
                //}            
            }           
        }
        else if (col.gameObject.tag == "Waypoint")
        {
            col.GetComponent<Collider>().enabled = false;
            Debug.Log("waypoint enter: " + col.gameObject.name);
            IsLanded = false;
            pathNavigator.WaypointPassed();

        } else if (col.gameObject.tag == "Tunnel")
        {
            Debug.Log("course tunnel hit");
            soundController.OffCourseWarning();
        }

    }

    public void OnTriggerExit(Collider col)
    {
        //Landing pad take off
        if (col.gameObject.tag == "Pad" && IsLanded == true)
        {
            Debug.Log("landing pad exit: " + col.gameObject.name);
            LandingPad lp = col.gameObject.GetComponent<LandingPad>();
            if (lp == navigationComputer.CurrentLandingPad)
            {
                IsLanded = false;
            }
            
        }
    }

    public void ManualFlightSelected()
    {
        //Set up manual flight interface
        flightDataVisualizer.SetManualFlightInterface();
        plane.gameObject.GetComponent<Rigidbody>().useGravity = true;
        manualFlight = true;
    }

    public void AutomaticFlightSelected()
    {
        flightDataVisualizer.SetAutomaticFlightInterface();
        engineController.StartEngines();

        plane.gameObject.GetComponent<Rigidbody>().useGravity = false;

        pathNavigator.NavigatePath();
    }

    //Returns false if engines can't be turned off (vehicle is not grounded)
    public bool EngineToggle()
    {
        soundController.ToggleEngineSound();
        if (engineController.AreEnginesOn)
        {
            //flightDataVisualizer.SetEngineTextsOff();
            engineController.StopEngines();
            engineController.ThrottleBase = 0;
        }
        else
        {
            engineController.StartEngines();
        }
        //flightDataVisualizer.SetEngineTextsOn();
        return true;
    }

    //Handles control of warning
    public void OnOffCourse()
    {
        if (!offCourseWarning)
        {
            offCourseWarning = true;
        }
        else
        {
            offCourseWarning = false;
        }
    }

    public void UnsetOffCourseWarning()
    {
        offCourseWarning = false;
    }

    private IEnumerator LandingPadContactHandler(LandingPad landingPad)
    {
        yield return new WaitForSeconds(10);
        plane.gameObject.GetComponent<Rigidbody>().useGravity = true;
        engineController.StopEngines();
        IsLanded = true;
        navigationComputer.LandingPadContact(landingPad);
    }

    //Checks course and toggles warning
    private void CheckCourse()
    {
        if (pathNavigator.GetDistanceFromPath() > MaxCourseDeviation)
        {
            OnOffCourse();
        }
        else
        {
            if (offCourseWarning)
            {
                OnOffCourse();
            }
        }
    }

    private IEnumerator DebugResetFlightStatusToGroundAfterWait(float wait)
    {
        yield return new WaitForSeconds(wait);
        flightDataVisualizer.SetLandedInterface();

        //Engine shut-off
        engineController.StopEngines();
        manualFlight = false;
    }

    private void ProcessThrottleChange()
    {
        float modifiedChange = controllerInputs.TrackpackLeftChange.y * throttleInputModifier;

        //Rudder deadzone
        if (controllerInputs.TrackpadValueLeft.x > 0.3f || controllerInputs.TrackpadValueLeft.x < -0.3f)
        {
            modifiedChange = 0;
        }

        engineController.ThrottleBase += modifiedChange;
        if (engineController.ThrottleBase > 100)
        {
            engineController.ThrottleBase = 100;
        }
        else if (engineController.ThrottleBase < 0)
        {
            engineController.ThrottleBase = 0;
        }
    }

    private void ProcessRudderChange()
    {
        float modifiedChange = controllerInputs.TrackpadValueLeft.x;
        
        //Creating a deadzone
        if (modifiedChange < 0.3f && modifiedChange > -0.3f)
        {
            modifiedChange = 0;
        }

        modifiedChange *= 20;
    
        engineController.RudderModifier = modifiedChange;

    }

    private void ProcessDirectionChange()
    {
        engineController.DirectionModifier = controllerInputs.TrackpadValueRight;
    }
}
