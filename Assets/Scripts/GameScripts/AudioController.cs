using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource LevelMainAudio;
    [SerializeField] private AudioSource CalmBreathAudio;
    [SerializeField] private AudioSource FastBreathAudio;

    private float _levelGameOverValue;
    private Coroutine startinCoroutine;
    private Coroutine endingCoroutine;

    public void StopMenuAudio(AudioSource source)
    {
        StartCoroutine(StopMenuAudioCorutaine(source));
    }

    private void OnEnable()
    {
        GameStateController.OnGameStart += PlayMainBackLevelAudio;
        GameStateController.OnEnemyCountChanged += PlayBreathLevelAudio;
        GameStateController.OnWin += StopLevelSound;
        GameStateController.OnLoose += StopLevelSound;
    }

    private void StopLevelSound()
    {
        LevelMainAudio.Stop();
        CalmBreathAudio.Stop();
        FastBreathAudio.Stop();
    }

    private IEnumerator StopMenuAudioCorutaine(AudioSource source)
    {
        while(source.volume>0)
        {
            source.volume -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void PlayMainBackLevelAudio(float value)
    {
        LevelMainAudio.Play();
        _levelGameOverValue = value;
    }

    private void PlayBreathLevelAudio(float value)
    {
        if(startinCoroutine!=null)
            StopCoroutine(startinCoroutine);

        if (endingCoroutine != null)
            StopCoroutine(endingCoroutine);

        if (value > _levelGameOverValue*0.6f)
        {
            
            startinCoroutine=StartCoroutine(StopCalmSound(CalmBreathAudio, FastBreathAudio));
        }
        else
        {
            endingCoroutine=StartCoroutine(StopCalmSound(FastBreathAudio, CalmBreathAudio));
        }
    }

    private IEnumerator StopCalmSound(AudioSource endingAudio, AudioSource startingAudio)
    {
        while (startingAudio.volume > 0)
        {
            endingAudio.volume -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (startingAudio.isPlaying == false)
        {
            startingAudio.Play();

            while (startingAudio.volume < 1)
            {
                startingAudio.volume += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }    
    }
}
