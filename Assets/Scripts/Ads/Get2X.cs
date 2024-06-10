using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Get2X : MonoBehaviour
{

    public void Get2XAndNext()
    {
        var level = PlayerPrefs.GetInt("level", 1);
        PlayerPrefs.SetInt("level", level + 1);
        
        PlayerPrefs.DeleteKey("places_with_dots");
        
        DataSaver.Delete("losed_times");
        var actual = PlayerPrefs.GetInt("actual_level", 1);
        if (level + 1 > actual)
        {
            PlayerPrefs.SetInt("actual_level", level + 1);
            
            MoneySystem.Instance.MultiplyMoney(2); //level gecende goshyan puly

        }
        Loader.Load(Loader.Scene.MainMenuScene);

    }
}
