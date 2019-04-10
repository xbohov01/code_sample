using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject PlaneGameObject;
    public GameObject MenuParent;
    public List<FlightPath> DestinationList;
    private FlightComputer flightComputer;
    private NavigationComputer navigationComputer;
    private MediaWindowController mediaWindowController;
    private SoundController soundController;
    public Animator DestinationMenuAnimator;
    public Animator FlightSelectMenuAnimator;
    public Animator MediaMenuAnimator;

    //Animations
    private AnimationClip flightSelectAnimation;
    private AnimationClip destinationSelectAnimation;

    //Menu element groups
    public CanvasGroup FlightSelectUi;
    public CanvasGroup DestinationSelectUi;
    public CanvasGroup MediaMenuUi;

    //Menu Elements
    private Text destination1Text;
    private Text destination2Text;
    private Text destination3Text;
    private Text destination4Text;

    private CanvasGroup destination1Button;
    private CanvasGroup destination2Button;
    private CanvasGroup destination3Button;
    private CanvasGroup destination4Button;

    private int flightPathsIndexer = 0;

    // Start is called before the first frame update
    void Start()
    {
        flightComputer = PlaneGameObject.GetComponent<FlightComputer>();
        navigationComputer = PlaneGameObject.GetComponent<NavigationComputer>();
        mediaWindowController = gameObject.GetComponent<MediaWindowController>();
        soundController = transform.parent.GetComponentInChildren<SoundController>();

        DestinationMenuAnimator = GameObject.Find("Destination_Select_UI").GetComponent<Animator>();
        FlightSelectMenuAnimator = GameObject.Find("Flight_Select_UI").GetComponent<Animator>();
        MediaMenuAnimator = GameObject.Find("Media_UI").GetComponent<Animator>();

        //Create menu groups
        FlightSelectUi = GameObject.Find("Flight_Select_UI").GetComponent<CanvasGroup>();
        DestinationSelectUi = GameObject.Find("Destination_Select_UI").GetComponent<CanvasGroup>();
        MediaMenuUi = GameObject.Find("Media_UI").GetComponent<CanvasGroup>();

        //Retrieve elements
        destination1Text = GameObject.Find("destination1_button").GetComponentInChildren<Text>();
        destination2Text = GameObject.Find("destination2_button").GetComponentInChildren<Text>();
        destination3Text = GameObject.Find("destination3_button").GetComponentInChildren<Text>();
        destination4Text = GameObject.Find("destination4_button").GetComponentInChildren<Text>();

        destination1Button = GameObject.Find("destination1_button").GetComponentInChildren<CanvasGroup>();
        destination2Button = GameObject.Find("destination2_button").GetComponentInChildren<CanvasGroup>();
        destination3Button = GameObject.Find("destination3_button").GetComponentInChildren<CanvasGroup>();
        destination4Button = GameObject.Find("destination4_button").GetComponentInChildren<CanvasGroup>();

        FlightSelectMenuAnimator.SetBool("IsFlightSelectOpen", false);
        OpenDestinationSelect();
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Flight control selection
    /// Followed by Destination selection
    /// </summary>
    public void ManualFlightButtonClicked()
    {
        Debug.Log("Manual Flight selected");
        //Close menu
        StartCoroutine(CloseFlightSelectMenu());

        //Send signal to flight computer that manual flight was selected
        flightComputer.ManualFlightSelected();

        soundController.MenuConfirm();
    }

    public void AutomaticFlightButtonClicked()
    {
        Debug.Log("Automatic Flight selected");
        //Close menu
        StartCoroutine(CloseFlightSelectMenu());
        StartCoroutine(OpenMediaMenu());

        //Send signal to flight computer that automatic flight was selected
        flightComputer.AutomaticFlightSelected();

        soundController.MenuConfirm();
    }

    public void EngineButtonClicked()
    {
        Debug.Log("Engine button clicked");

        //TODO handle return
        flightComputer.EngineToggle();

        soundController.MenuConfirm();
    }

    public void FlightPrevButtonClicked()
    {
        StartCoroutine(CloseFlightSelectMenu());

        OpenDestinationSelect();

        soundController.MenuConfirm();
    }

    /// <summary>
    /// Destination Selection
    /// </summary>
    public void DestinationButtonClicked(int order)
    {

        StartCoroutine(CloseDestinationSelectMenuAndOpenFlightSelect());

        navigationComputer.SetFlightPath(flightPathsIndexer+order);

        soundController.MenuConfirm();
    }

    public void DestinationNextButtonClicked()
    {
        DestinationsNext();

        soundController.MenuConfirm();
    }

    public void DestinationPrevButtonClicked()
    {
        DestinationsPrev();

        soundController.MenuConfirm();
    }

    /// <summary>
    /// Media interactions
    /// </summary>
    public void PhoneButtonClicked()
    {
        mediaWindowController.PhoneClicked();

        soundController.MenuConfirm();
    }

    public void EmailButtonClicked()
    {
        mediaWindowController.EmailClicked();

        soundController.MenuConfirm();
    }

    public void MusicButtonClicked()
    {
        mediaWindowController.MusicClicked();

        soundController.MenuConfirm();
    }

    public void BrowserButtonClicked()
    {
        mediaWindowController.BrowserClicked();

        soundController.MenuConfirm();
    }

    //Animation interactions
    public void OpenFlightSelect()
    {
        Debug.Log("Opening flight select menu");
        //FlightSelectUi.alpha = 1f;
        FlightSelectMenuAnimator.SetBool("IsFlightSelectOpen", true);
    }

    public void OpenDestinationSelect()
    {
        //DestinationSelectUi.alpha = 1f;
        StartCoroutine(CloseFlightSelectMenu());
        DestinationMenuAnimator.SetBool("IsDestinationMenuOpen", true);

        //Get flight paths
        DestinationList = navigationComputer.GetAvailableFlightPaths();

        //Show first 3 options
        ShowCurrentDestinations();

    }

    //Menu control methods
    private void DestinationsNext()
    {
        if (flightPathsIndexer < DestinationList.Count)
        {
            flightPathsIndexer += 4;
            ShowCurrentDestinations();
        } 
    }

    private void DestinationsPrev()
    {
        if (flightPathsIndexer >= 4)
        {
            flightPathsIndexer -= 4;
            ShowCurrentDestinations();
        }
        
    }

    private void ShowCurrentDestinations()
    {
        if (DestinationList != null)
        {
            if (flightPathsIndexer < DestinationList.Count)
            {
                destination1Button.alpha = 1;
                destination1Button.interactable = true;
                destination1Text.text = DestinationList[flightPathsIndexer].DestinationPoint;
            }
            else
            {
                destination1Button.alpha = 1;
                destination1Button.interactable = false;
                destination1Text.text = "No destinations available";
            }

            if (flightPathsIndexer+1 < DestinationList.Count)
            {
                destination2Button.alpha = 1;
                destination2Text.text = DestinationList[flightPathsIndexer + 1].DestinationPoint;
            }
            else
            {
                destination2Button.alpha = 0;
            }

            if (flightPathsIndexer + 2 < DestinationList.Count)
            {
                destination3Button.alpha = 1;
                destination3Text.text = DestinationList[flightPathsIndexer + 2].DestinationPoint;
            }
            else
            {
                destination3Button.alpha = 0;
            }

            if (flightPathsIndexer + 3 < DestinationList.Count)
            {
                destination4Button.alpha = 1;
                destination4Text.text = DestinationList[flightPathsIndexer + 3].DestinationPoint;
            }
            else
            {
                destination4Button.alpha = 0;
            }            
            
        }
    }

    //Animation control coroutines
    private IEnumerator CloseFlightSelectMenu()
    {
        FlightSelectMenuAnimator.SetBool("IsFlightSelectOpen", false);
        yield return new WaitForSeconds(1f);
        //FlightSelectUi.alpha = 0;
    }

    private IEnumerator CloseDestinationSelectMenuAndOpenFlightSelect()
    {
        DestinationMenuAnimator.SetBool("IsDestinationMenuOpen", false);
        yield return new WaitForSeconds(1f);
        //DestinationSelectUi.alpha = 0;
        
        FlightSelectMenuAnimator.SetBool("IsFlightSelectOpen", true);
        yield return new WaitForEndOfFrame();
        //FlightSelectUi.alpha = 1;
    }

    private IEnumerator OpenMediaMenu()
    {
        MediaMenuAnimator.SetBool("IsMediaMenuOpen", true);
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator CloseMediaMenu()
    {
        MediaMenuAnimator.SetBool("IsMediaMenuOpen", false);
        yield return new WaitForSeconds(1f);
    }
}
