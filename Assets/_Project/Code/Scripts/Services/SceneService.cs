using UnityEngine.SceneManagement;

namespace Project.Scripts.Scene
{
    public interface ISceneService
    {
        void LoadSceneByName(string sceneName);
    }

    public class SceneService : ISceneService
    {
        public void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}