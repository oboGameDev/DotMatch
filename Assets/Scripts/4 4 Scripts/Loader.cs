using UnityEngine.SceneManagement;
    public static class Loader
    {
        private static Scene targetScene;

        public enum Scene
        {
            MainMenuScene,
            LoadingScene , //this is not used yet
            GameScene,
        }



        public static void Load(Scene targetScene)
        {

            Loader.targetScene = targetScene;
            SceneManager.LoadScene(targetScene.ToString());
            
        }


        public static void LoaderCallback()
        {
            SceneManager.LoadScene(targetScene.ToString());//Load scene enum kabul edenok , that ' s why to string etmeli
        }
    }
