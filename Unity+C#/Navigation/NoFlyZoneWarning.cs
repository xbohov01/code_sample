using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFlyZoneWarning : MonoBehaviour
{
    public float MinDistance = 10;
    public float MaxDistance = 80;
    private Material material;
    private Transform planeTransform;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
        ChangeOpacity(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (planeTransform)
        {
            float distance = Vector3.Distance(planeTransform.position, transform.position);
            //Check max
            if (distance > MaxDistance)
            {
                ChangeOpacity(0);
            } else if (distance < MinDistance)
            {
                //Check min
                ChangeOpacity(200);
            }
            else
            {
                //Check in between
                float ratio = (distance - MinDistance) / MaxDistance;
                float opacity = Mathf.Lerp(0, 200, ratio);
                ChangeOpacity(opacity);
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (planeTransform == null)
        {
            planeTransform = col.gameObject.transform;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (planeTransform)
        {
            ChangeOpacity(0);
            planeTransform = null;
        }
    }

    private void ChangeOpacity(float targetOpacity)
    {
        Color newColor = material.color;
        newColor.a = targetOpacity;
        material.color = newColor;
    }
}
