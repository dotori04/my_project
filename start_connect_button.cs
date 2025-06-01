using UnityEngine;
using UnityEngine.SceneManagement;

public class start_connect_button : MonoBehaviour
{
    public void LoadSceneBasedOnSelectedStar()
    {
        string starName = star_line.selectedStarName; // ✅ static 변수 사용

        if (string.IsNullOrEmpty(starName))
        {
            Debug.LogWarning("선택된 별이 없습니다.");
            return;
        }

        switch (starName)
        {
            case "Star_A":
                SceneManager.LoadScene("Star1");
                break;
            case "Star_B":
                SceneManager.LoadScene("Scene_Star_B");
                break;
            default:
                Debug.LogWarning($"선택된 별 '{starName}'에 해당하는 씬이 없습니다.");
                break;
        }
    }
}
