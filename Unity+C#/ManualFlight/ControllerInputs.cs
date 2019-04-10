using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

public class ControllerInputs : MonoBehaviour
{
    [SteamVR_DefaultAction("TrackpadTouch")]
    public SteamVR_Action_Vector2 trackpadAction;
    public Button CollidedButton;
    public GameObject CollidedObject;

    public Vector2 TrackpadValueRight;
    public Vector2 TrackpadValueLeft;
    public Vector2 TrackpackLeftChange;

    private SteamVR_TrackedObject controller;
    private Vector2 trackpadLastValueLeft;
    private Transform windowOriginalParent;
    private MediaWindow connectedMediaWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets input values based on trackpad inputs
        TrackpadValueRight = trackpadAction.GetAxis(SteamVR_Input_Sources.RightHand);
        TrackpadValueLeft = trackpadAction.GetAxis(SteamVR_Input_Sources.LeftHand);
        trackpadLastValueLeft = trackpadAction.GetLastAxis(SteamVR_Input_Sources.LeftHand);
        TrackpackLeftChange = trackpadAction.GetAxisDelta(SteamVR_Input_Sources.LeftHand);

        if (TrackpadValueLeft == Vector2.zero && trackpadLastValueLeft != Vector2.zero)
        {
            TrackpackLeftChange = Vector2.zero;
        }

        //Trigger pulled
        //TODO verify binding
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (CollidedButton)
            {
                CollidedButton.onClick.Invoke();
            }
            

            if (CollidedObject)
            {
                //Move window
                connectedMediaWindow = CollidedObject.gameObject.GetComponent<MediaWindow>();
                connectedMediaWindow.WindowPickup();
            }
        }

        if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.Any))
        {
            if (connectedMediaWindow)
            {
                Debug.Log("release");
                //Move window
                connectedMediaWindow.WindowRelease();
            }
        }
    }

}
