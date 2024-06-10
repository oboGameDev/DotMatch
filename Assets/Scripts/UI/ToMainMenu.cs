using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ToMainMenu : MonoBehaviour
{
    [SerializeField] private Button GoToMainMenu;

    private void Awake()
    {
        GoToMainMenu.onClick.AddListener((() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);   
            PlaceManager.Instance.SaveLevel();
            /*foreach (var item in GameLevelGoalUI.Instance.Containers)
            {
                if (item.gameObject.activeSelf)
                {
                 
                }
            }*/
        }));
    }
}