using NINESOFT.TUTORIAL_SYSTEM.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Assets.Scripts.UI
{
    public class RestartButton : MonoBehaviour
    {
        public void Restart()
        {
            /*
            SceneManager.LoadScene(gameObject.scene.buildIndex);
            */
            Loader.Load(Loader.Scene.MainMenuScene);
            PlayerPrefs.DeleteKey("places_with_dots");

            Time.timeScale = 1f;
       
        }
        public void Next()
        {
            var level = PlayerPrefs.GetInt("level", 1);
            PlayerPrefs.SetInt("level", level + 1);
            PlayerPrefs.DeleteKey("places_with_dots");
            DataSaver.Delete("losed_times");//inniki levellara gecen wagtlary , levely dowam etyan bolsa
            
            var actual = PlayerPrefs.GetInt("actual_level", 1);
            if (level + 1 > actual)
            {
                PlayerPrefs.SetInt("actual_level", level + 1);
            }

            Loader.Load(Loader.Scene.MainMenuScene);
            /*
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        */
        }
    }
}