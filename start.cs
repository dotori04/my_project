using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public string targetSceneName;

        public void ChangeScene()
        {
            Debug.Log("change scene");
            SceneManager.LoadScene(targetSceneName);
        }
}
