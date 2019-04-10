using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Source.FlightData;
using Assets.Source.Visualization;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

public class FlightDataVisualizer : MonoBehaviour
{
    public FlightData CurrentFlightData;
    public GameObject PlaneGameObject;
    private float lastAltitude = 0;

    //Control properties
    public float CanopyOpacityMaximum = 0.8f;
    private bool manualFlight = false;

    //3D elements
    private IEnumerable<Transform> meshComponents;
    private Transform headingIndicator;
    private Transform elevationIndicator;
    private Transform artificialHorizon;
    private Transform altitudeIndicator;
    private List<Renderer> internalScreensRenderers;
    private Transform verticalSpeedIndicator;
    private Transform directionIndicator;

    //UI elements
    private IEnumerable<Text> textComponents;
    private CanvasGroup dataUI;
    private CanvasGroup menuUI;
    private CanvasGroup manualControlUI;
    private Text throttleText;
    private Text batteryText;
    private Text headingText;
    private Text elevationText;
    private Text airspeedText;
    private Text altitudeText;
    private Text verticalSpeedText;
    private UIBarController throttleBarController;
    private Text rpmText;
    private IEnumerable<Text> engineStatusTexts;
    private RectTransform throttleBar;

    private MenuController menuController;
    private bool isUiShown = false;
    private readonly float canopyOpacityModifier = 0.05f;

    // Update is called once per frame
    private void Update()
    {
        if (isUiShown)
        {
            UpdateTextData();
            UpdateMeshData();
            lastAltitude = CurrentFlightData.Altitude;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Get UI elements
        textComponents = gameObject.GetComponentsInChildren<Text>();
        throttleText = textComponents.First(x => x.gameObject.name == "throttle_text");
        batteryText = textComponents.First(x => x.gameObject.name == "battery_text");
        headingText = textComponents.First(x => x.gameObject.name == "heading_text");
        elevationText = textComponents.First(x => x.gameObject.name == "elevation_text");
        airspeedText = textComponents.First(x => x.gameObject.name == "airspeed_text");
        altitudeText = textComponents.First(x => x.gameObject.name == "altitude_text");
        rpmText = textComponents.First(x => x.gameObject.name == "rpm_text");
        engineStatusTexts = textComponents.ToList().FindAll(x => x.gameObject.name.Contains("engine_"));
        verticalSpeedText = textComponents.First(x => x.gameObject.name == "vsi_text");

        //Get throttle bar and create a controller
        throttleBar = GameObject.Find("throttle_bar").GetComponent<RectTransform>();
        throttleBarController = new UIBarController(throttleBar, 100, UIBarController.BarDirection.Vertical, 5);
        

        //Get internal screens and door panels
        internalScreensRenderers = gameObject.GetComponentsInChildren<Renderer>().ToList().FindAll(x => x.gameObject.name.Contains("canopy_int"));
        Renderer tmp = GameObject.Find("door_panel.001").GetComponent<Renderer>();
        internalScreensRenderers.Add(tmp);
        tmp = GameObject.Find("door_panel.002").GetComponent<Renderer>();
        internalScreensRenderers.Add(tmp);

        //Get mesh elements
        meshComponents = gameObject.GetComponentsInChildren<Transform>().ToList().FindAll(x => !x.gameObject.name.Contains("canopy"));
        artificialHorizon = meshComponents.First(x => x.gameObject.name == "artificial_horizon");
        headingIndicator = meshComponents.First(x => x.gameObject.name == "heading_indicator_slider");
        elevationIndicator = meshComponents.First(x => x.gameObject.name == "elevation_indicator_slider");
        altitudeIndicator = meshComponents.First(x => x.gameObject.name == "altitude_meter");
        verticalSpeedIndicator = meshComponents.First(x => x.gameObject.name == "vsi_indicator");
        directionIndicator = meshComponents.First(x => x.gameObject.name == "direction_arrow");

        menuController = gameObject.GetComponent<MenuController>();

        //Get canvas groups
        menuUI = GameObject.Find("Menu_UI").GetComponent<CanvasGroup>();
        dataUI = GameObject.Find("Data_UI").GetComponent<CanvasGroup>();
        manualControlUI = GameObject.Find("Manual_Control_UI").GetComponent<CanvasGroup>();

        //Hide UI before startup
        HideAllUI();
        SetStartInterface();
    }

    public void SetManualFlightInterface()
    {
        ShowAllUI();
        manualControlUI.alpha = 1;
        manualFlight = true;
        StartCoroutine(ChangeCanopyOpacity(0f));
    }

    public void SetAutomaticFlightInterface()
    {
        ShowAllUI();
        HideElement(airspeedText.gameObject);
        HideElement(throttleText.gameObject);
        HideElement(throttleBar.gameObject);

        manualFlight = false;
        StartCoroutine(ChangeCanopyOpacity(0.5f));
        //SetEngineTextsOn();
    }

    public void SetLandedInterface()
    {
        HideAllUI();
        StartCoroutine(ChangeCanopyOpacity(CanopyOpacityMaximum));
        menuController.OpenDestinationSelect();
        //SetEngineTextsOff();
    }

    private void SetStartInterface()
    {
        HideAllUI();
        StartCoroutine(ChangeCanopyOpacity(CanopyOpacityMaximum));
    }

    private void UpdateTextData()
    {
        throttleText.text = CurrentFlightData.ThrottleStatus.ToString("F1") + "%";
        batteryText.text = "Battery: " + CurrentFlightData.BatteryStatus + "%";
        headingText.text = CurrentFlightData.HeadingDegrees.ToString("F1");

        //Display correction
        if (CurrentFlightData.ElevationDegrees > 180)
        {
            elevationText.text = (-CurrentFlightData.ElevationDegrees + 360).ToString("F1");
        }
        else
        {
            elevationText.text = CurrentFlightData.ElevationDegrees.ToString("F1");
        }

        airspeedText.text = CurrentFlightData.AirSpeed.ToString("F1") + " kn";
        altitudeText.text = CurrentFlightData.Altitude.ToString("F1");
        throttleBarController.CurrentBarValue = (int)CurrentFlightData.ThrottleStatus;
        rpmText.text = CurrentFlightData.Rpm + " RPM";
        UpdateEngineStatuses();

        verticalSpeedText.text = CurrentFlightData.VerticalSpeed.ToString("F1");
    }

    private void UpdateEngineStatuses()
    {
        foreach (var engineStatus in CurrentFlightData.EngineStatuses)
        {
            SetEngineStatus(engineStatus.EngineId, engineStatus.EngineStatusString);
        }
    }

    private void UpdateMeshData()
    {
   
        //headingIndicator.localRotation = new Quaternion(CurrentFlightData.Elevation, CurrentFlightData.Heading, 0, 0);
        headingIndicator.localEulerAngles = new Vector3(0, -CurrentFlightData.HeadingDegrees, 0);
        
        //artificialHorizon.localRotation = new Quaternion(-CurrentFlightData.Elevation, 0, 0, 0);
        artificialHorizon.eulerAngles = new Vector3(-CurrentFlightData.ElevationDegrees, 0, 0);
        
        //elevationIndicator.localRotation = new Quaternion(-CurrentFlightData.Elevation, 0, 0, 0);
        elevationIndicator.eulerAngles = new Vector3(-CurrentFlightData.ElevationDegrees, CurrentFlightData.HeadingDegrees, 0);
        
        //Rotate altitude meter
        float altitudeDifference = CurrentFlightData.Altitude - lastAltitude;
        //altitudeIndicator.Rotate(altitudeIndicator.right, altitudeDifference);
        altitudeIndicator.localEulerAngles = new Vector3(altitudeIndicator.localEulerAngles.x + altitudeDifference, 0, 0);
        lastAltitude = CurrentFlightData.Altitude;

        //Direction arrow
        if (CurrentFlightData.NextWaypointTransform)
        {
            directionIndicator.LookAt(CurrentFlightData.NextWaypointTransform);
        }

        //VSI
        if (CurrentFlightData.VerticalSpeed <= 5f && CurrentFlightData.VerticalSpeed >= -5f)
        {
            verticalSpeedIndicator.localEulerAngles = new Vector3(-90 + (-CurrentFlightData.VerticalSpeed * 18), 0, 0);
        }
        else
        {
            verticalSpeedIndicator.localEulerAngles = CurrentFlightData.VerticalSpeed > 5f ? new Vector3(-180, 0, 0) : new Vector3(0, 0, 0);
        }
    }

    private void HideAllUI()
    {
        //Hide texts
        dataUI.alpha = 0;
        manualControlUI.alpha = 0;
        //Hide meshes
        foreach (var component in meshComponents)
        {
            var renderer = component.gameObject.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.enabled = false;
            }
        }

        isUiShown = false;
    }

    private void ShowAllUI()
    {
        //Show texts
        dataUI.alpha = 1;
        ShowElement(airspeedText.gameObject);
        ShowElement(throttleText.gameObject);
        ShowElement(throttleBar.gameObject);

        //Show meshes
        foreach (var component in meshComponents)
        {
            var renderer = component.gameObject.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.enabled = true;
            }
        }

        isUiShown = true;
    }

    private void HideElement(GameObject element)
    {
        var rendererComponent = element.GetComponent<Renderer>();
        var canvasGroupComponent = element.GetComponent<CanvasGroup>();
        if (rendererComponent)
        {
            rendererComponent.enabled = false;
        } 
        else if (canvasGroupComponent)
        {
            canvasGroupComponent.alpha = 0;
        }
        
    }

    private void ShowElement(GameObject element)
    {
        var rendererComponent = element.GetComponent<Renderer>();
        var canvasGroupComponent = element.GetComponent<CanvasGroup>();
        if (rendererComponent)
        {
            rendererComponent.enabled = true;
        }
        else if (canvasGroupComponent)
        {
            canvasGroupComponent.alpha = 1;
        }

    }

    private IEnumerator ChangeCanopyOpacity(float targetOpacity)
    {
        foreach (Renderer screenRenderer in internalScreensRenderers)
        {
            if (screenRenderer.material.color.a > targetOpacity)
            {
                //Decrease opacity
                while (screenRenderer.material.color.a > targetOpacity)
                {
                    Color currentColor = screenRenderer.material.color;                                  
                    if (currentColor.a - canopyOpacityModifier < 0)
                    {
                        currentColor.a = 0;
                    }
                    else
                    {
                        currentColor.a -= canopyOpacityModifier;
                    }
                    screenRenderer.material.color = currentColor;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                //Increase opacity
                while (screenRenderer.material.color.a < targetOpacity)
                {
                    Color currentColor = screenRenderer.material.color;                 
                    if (currentColor.a + canopyOpacityModifier > 1)
                    {
                        currentColor.a = 1;
                    }
                    else
                    {
                        currentColor.a += canopyOpacityModifier;
                    }
                    screenRenderer.material.color = currentColor;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        //yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Engine status texts
    /// </summary>
    public void SetEngineStatus(int engineId, string status)
    {
        Text engineText = engineStatusTexts.ToList().Find(x => x.gameObject.name == "engine_" + engineId);
        string currentText = engineText.text;
        string engine = currentText.Split(':').First();
        engineText.text = engine + ": " + status;
    }
}
