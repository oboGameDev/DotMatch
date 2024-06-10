using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using Sounds;
using UnityEngine;
using Utils;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO _audioClipRefsSo;
    [SerializeField] private AudioSource SoundPlayer;
    public Action VibrateOnButtonClick;

    public bool OnWin = true;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //PlaceManager.Instance.DotDestroyed += PLace_OnDotReachedUI;
        PlaceManager.Instance.Destroy += Place_Destroy;
        PlaceManager.Instance.OnFloatingWords += OnFloatingWords;
        DragController.Instance.OnPlacement += OnPlacement;
        GameLevelGoalUI.Instance.OnWinPanelShowSound += OnWinPanelShowSound;
        DragController.Instance.OnLost += OnLost;
        //PlaceManager.Instance.OnMoneyAdded += Place_MoneyAdded;
    }

    private void OnFloatingWords()
    {
        PlaySound(_audioClipRefsSo.floatingWords);
    }

    /*private void Place_MoneyAdded()
    {
        PlaySound(_audioClipRefsSo.moneyAdded);
    }*/

    private void OnPlacement()
    {
        PlaySound(_audioClipRefsSo.dotDrop);
    }

    private void OnLost()
    {
       PlaySound(_audioClipRefsSo.losePanel);
    }

    private void OnWinPanelShowSound()
    {
        PlaySound(_audioClipRefsSo.winPanel);
        OnWin = false;
    }
    private void Place_Destroy()
    {
        if (_audioClipRefsSo.dotDestroy != null )
        {
            PlaySound(_audioClipRefsSo.dotDestroy);
        }
    }

    private void PLace_OnDotReachedUI()
    {
        PlaySound(_audioClipRefsSo.moneyAdded);
    }

    private void PlaySound(AudioClip audioClip, float volumeMultiplier = 1f)
    {
        if (!DataSaver.Load("sound", true))
        {
            return;
        }
        SoundPlayer.PlayOneShot(audioClip, volumeMultiplier); 
    }

    public void ButtonSound()
    {
        PlaySound(_audioClipRefsSo.buttonClick);
        VibrateOnButtonClick?.Invoke();
    }
    
    public void SettingsChanged()
    {
        if (DataSaver.Load("sound" , true))
        {
            if (!SoundPlayer.isPlaying)
            {
                SoundPlayer.Play();
            }
        }
        else
        {
            if (SoundPlayer.isPlaying)
            {
                SoundPlayer.Stop();
            }
        }
    }
    
}