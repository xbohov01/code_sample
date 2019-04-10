using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Source.ManualFlight;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public GameObject FrontRight;
    public GameObject FrontLeft;
    public GameObject RearRight;
    public GameObject RearLeft;
    public float ThrottleBase = 0;
    public bool AreEnginesOn = false;
    public int Rpm = 0;
    public Vector2 DirectionModifier;
    public float RudderModifier;
    public Animator EngineAnimator;

    private List<EngineStatus> engineStatuses = new List<EngineStatus>();
    private float startUpTime = 5;
    public float[] directionModifiers = {1,1,1,1};

    private Transform frontRight;
    private Transform frontLeft;
    private Transform rearRight;
    private Transform rearLeft;
    private Rigidbody planeRigidbody;
    private float rearEngineModifier = 0.715f;


    //TODO sound

    // Start is called before the first frame update
    void Start()
    {
        engineStatuses.Add(new EngineStatus(1, "FR"));
        engineStatuses.Add(new EngineStatus(2, "FL"));
        engineStatuses.Add(new EngineStatus(3, "RR"));
        engineStatuses.Add(new EngineStatus(4, "RL"));
        planeRigidbody = gameObject.GetComponent<Rigidbody>();
        frontRight = FrontRight.GetComponentInChildren<Transform>();
        frontLeft = FrontLeft.GetComponentInChildren<Transform>();
        rearRight = RearRight.GetComponentInChildren<Transform>();
        rearLeft = RearLeft.GetComponentInChildren<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AreEnginesOn)
        {
            ApplyDirectionModifier();
            ApplyThrottle();
            ApplyRudderModifier();
        }
        
    }

    public List<EngineStatus> GetEngineStatuses()
    {
        return engineStatuses;
    }

    public void StartEngines()
    {
        StartCoroutine(StartUpSequence());
        StartCoroutine(RpmMock());
        EngineAnimator.SetBool("IsEngineOn", true);
    }

    public void StopEngines()
    {
        AreEnginesOn = false;
        StopCoroutine(StartUpSequence());
        foreach (var engine in engineStatuses)
        {
            engine.EngineStatusString = "off";
        }

        Rpm = 0;
        EngineAnimator.SetBool("IsEngineOn", false);
    }

    public IEnumerator DelayedEngineStop()
    {
        while (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
        {
            yield return new WaitForSeconds(1);
        }
        StopEngines();
    }

    private IEnumerator StartUpSequence()
    {
        foreach (var engine in engineStatuses)
        {
            engine.EngineStatusString = "starting";
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(startUpTime);

        foreach (var engine in engineStatuses)
        {
            engine.EngineStatusString = "running";
            yield return new WaitForSeconds(0.1f);
        }
        AreEnginesOn = true;
    }

    private IEnumerator RpmMock()
    {
        while (Rpm < 1000)
        {
            Rpm++;
            yield return new WaitForEndOfFrame();
        }

    }

    private void ApplyDirectionModifier()
    {
        //Clear previous modifiers
        Array.Clear(directionModifiers, 0, 4);

        if (DirectionModifier != Vector2.zero)
        {
            DirectionModifier /= 8;
            float y = (float)Math.Round(DirectionModifier.y, 2);
            float x = (float)Math.Round(DirectionModifier.x, 2);
            //Pitch
            if (y >= 0)
            {
                //Front Right
                directionModifiers[2] += y;
                //Front Left
                directionModifiers[3] += y;
            }
            else
            {
                //Rear Right
                directionModifiers[0] += Mathf.Abs(y);
                //RearLeft
                directionModifiers[1] += Mathf.Abs(y);
            }

            
            //Roll
            if (DirectionModifier.x >= 0)
            {
                //Front Right
                directionModifiers[1] += x;
                //Rear Right
                directionModifiers[3] += x;
            }
            else
            {
                //Front Left
                directionModifiers[0] += Mathf.Abs(x);
                //RearLeft
                directionModifiers[2] += Mathf.Abs(x);
            } 
        }
        else
        {
            Array.Clear(directionModifiers, 0, 4);
        }
    }

    private void ApplyRudderModifier()
    {
        //Rotate plane
        planeRigidbody.AddTorque(transform.up * RudderModifier, ForceMode.Force);
    }

    private void ApplyThrottle()
    {
        //Debug.Log(ThrottleBase);
        if (AreEnginesOn)
        {
            Rpm = (int)(1000 + (ThrottleBase * 20));
        }     
        planeRigidbody.AddForceAtPosition(frontRight.forward * (directionModifiers[0] + ThrottleBase), frontRight.position, ForceMode.Impulse);
        planeRigidbody.AddForceAtPosition(frontLeft.forward * (directionModifiers[1] + ThrottleBase), frontLeft.position, ForceMode.Impulse);
        planeRigidbody.AddForceAtPosition(rearRight.forward * (directionModifiers[2] + ThrottleBase) * rearEngineModifier, rearRight.position, ForceMode.Impulse);
        planeRigidbody.AddForceAtPosition(rearLeft.forward * (directionModifiers[3] + ThrottleBase) * rearEngineModifier, rearLeft.position, ForceMode.Impulse);
 
    }
}
