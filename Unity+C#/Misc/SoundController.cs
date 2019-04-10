using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip EngineSoundClip;
    public AudioClip OffCourseWarningClip;
    public AudioClip DescentWarningClip;
    public AudioClip BatteryWarningClip;
    public AudioClip MenuConfirmClip;

    public FlightComputer FlightComputerRef;

    private AudioSource singleSource;
    private AudioSource repeatSource;
    private AudioSource engineSource;

    private bool isEngineSoundOn = false;
    private bool isOffCourseWarningOn = false;
    private bool isDescentWarningOn = false;
    private bool isBatteryWarningOn = false;

    // Start is called before the first frame update
    void Start()
    {
        singleSource = GameObject.Find("SingleAudioSource").GetComponent<AudioSource>();
        repeatSource = GameObject.Find("RepeatAudioSource").GetComponent<AudioSource>();
        engineSource = GameObject.Find("EngineAudioSource").GetComponent<AudioSource>();

        singleSource.Stop();
        repeatSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuConfirm()
    {
        PlaySingleSound(MenuConfirmClip);
    }

    public void ToogleBatteryWarning()
    {
        isBatteryWarningOn = !isBatteryWarningOn;

        if (isBatteryWarningOn)
        {
            PlayRepeatSound(BatteryWarningClip);
        }
        else
        {
            StopRepeatSound();
        }
    }

    public void ToggleDescentWarning()
    {
        isDescentWarningOn = !isDescentWarningOn;

        if (isBatteryWarningOn)
        {
            PlayRepeatSound(DescentWarningClip);
        }
        else
        {
            StopRepeatSound();
        }
    }

    public void ToggleEngineSound()
    {
        if (!isEngineSoundOn)
        {
            engineSource.clip = EngineSoundClip;
            engineSource.loop = true;
            engineSource.Play();
            isEngineSoundOn = true;
        }
        else
        {
            engineSource.Pause();
            isEngineSoundOn = false;
        }
    }

    public void OffCourseWarning()
    {
        if (!isOffCourseWarningOn)
        {
            PlaySingleSound(OffCourseWarningClip);
        }
        
    }

    private void PlaySingleSound(AudioClip sound)
    {
        singleSource.clip = sound;
        StartCoroutine(PlaySingleSoundTimer());
    }

    private IEnumerator PlaySingleSoundTimer()
    {
        singleSource.Play();
        yield return new WaitForSeconds(singleSource.clip.length);
        singleSource.Stop();
        singleSource.clip = null;

        if (singleSource.clip == OffCourseWarningClip)
        {
            FlightComputerRef.UnsetOffCourseWarning();
        }
    }

    private void PlayRepeatSound(AudioClip sound)
    {
        repeatSource.clip = sound;
        repeatSource.Play();
    }

    private void StopRepeatSound()
    {
        repeatSource.Stop();
        repeatSource.clip = null;
    }
}
