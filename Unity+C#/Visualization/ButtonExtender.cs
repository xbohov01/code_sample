using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtender : MonoBehaviour
{
    private Button thisButton;

    // Start is called before the first frame update
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "VRController")
        {
            //Set button
            ControllerInputs inputObject = GameObject.FindGameObjectWithTag("InputController").GetComponent<ControllerInputs>();
            inputObject.CollidedButton = thisButton;
            
            //Highlight this button
            thisButton.Select();
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "VRController")
        {
            //Set button
            ControllerInputs inputObject = GameObject.FindGameObjectWithTag("InputController").GetComponent<ControllerInputs>();
            inputObject.CollidedButton = null;

            //Highlight this button
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
