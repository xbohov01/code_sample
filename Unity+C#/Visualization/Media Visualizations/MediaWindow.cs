using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaWindow : MonoBehaviour
{
    private Transform originalParent;
    private ControllerHolder controller;

    // Start is called before the first frame update
    void Start()
    {
        originalParent = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "VRController")
        {
            //Set button
            ControllerInputs inputObject = GameObject.FindGameObjectWithTag("InputController").GetComponent<ControllerInputs>();
            inputObject.CollidedObject = this.gameObject;

            controller = col.transform.parent.gameObject.GetComponent<ControllerHolder>();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "VRController")
        {
            //Unset button
            ControllerInputs inputObject = GameObject.FindGameObjectWithTag("InputController").GetComponent<ControllerInputs>();
            inputObject.CollidedObject = null;

            //controller = null;
        }
    }

    public void WindowPickup()
    {
        //Switches parent to controller
        if (controller)
        {
            this.transform.SetParent(controller.transform);
        }
    }

    public void WindowRelease()
    {
        Debug.Log("release 2");
        //Switch parent back
        this.transform.SetParent(originalParent);
    }
}
