using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    public List<FlightPath> AvailableFlightPaths;
    private GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePad()
    {
        indicator.SetActive(true);
        gameObject.GetComponent<Animator>().SetBool("isActive", true);
    }

    public void DeactivatePad()
    {
        indicator.SetActive(false);
        gameObject.GetComponent<Animator>().SetBool("isActive", false);
    }
}
