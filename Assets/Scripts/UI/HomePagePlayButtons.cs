using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class HomePagePlayButtons : MonoBehaviour
{
    public void GoHomePage()
    {
        SceneManager.LoadScene(gameObject.scene.buildIndex);
        Time.timeScale = 1f;
        
    }
}
