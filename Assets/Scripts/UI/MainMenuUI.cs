using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
   [SerializeField] private Button playButton;

   private void Awake()
   {
      playButton.onClick.AddListener((() =>
      {
      }));
      
      
   }
}
