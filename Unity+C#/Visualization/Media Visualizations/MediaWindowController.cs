using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaWindowController : MonoBehaviour
{
    public GameObject EmailPrefab;
    public GameObject PhonePrefab;
    public GameObject MusicPrefab;
    public GameObject BrowserPrefab;

    private bool emailOpen = false;
    private bool phoneOpen = false;
    private bool musicOpen = false;
    private bool browserOpen = false;
    private Transform windowSpawnPoint;
    private GameObject emailWindow;
    private GameObject phoneWindow;
    private GameObject browserWindow;
    private GameObject musicWindow;

    // Start is called before the first frame update
    void Start()
    {
        windowSpawnPoint = GameObject.Find("media_window_origin").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseAllMediaWindows()
    {
        Destroy(emailWindow);
        Destroy(phoneWindow);
        Destroy(browserWindow);
        Destroy(musicWindow);
    }

    public void EmailClicked()
    {
        if (!emailOpen)
        {
            emailWindow = Instantiate(EmailPrefab, windowSpawnPoint);         
        }
        else
        {
            Destroy(emailWindow);
        }
        emailOpen = !emailOpen;
    }

    public void PhoneClicked()
    {
        if (!phoneOpen)
        {
            phoneWindow = Instantiate(PhonePrefab, windowSpawnPoint);
        }
        else
        {
            Destroy(phoneWindow);
        }
        phoneOpen = !phoneOpen;
    }

    public void BrowserClicked()
    {
        if (!browserOpen)
        {
            browserWindow = Instantiate(BrowserPrefab, windowSpawnPoint);
        }
        else
        {
            Destroy(browserWindow);
        }
        browserOpen = !browserOpen;
    }

    public void MusicClicked()
    {
        if (!musicOpen)
        {
            musicWindow = Instantiate(MusicPrefab, windowSpawnPoint);
        }
        else
        {
            Destroy(musicWindow);
        }
        musicOpen = !musicOpen;
    }
}
