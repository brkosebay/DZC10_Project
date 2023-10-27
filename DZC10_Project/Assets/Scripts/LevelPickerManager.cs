using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPickerManager : MonoBehaviour
{
    public void SelectNormalDifficulty()
    {
        GameManager.Instance.difficultyLevel = 1;
        LoadMainGameScene();
    }

    public void SelectMediumDifficulty()
    {
        GameManager.Instance.difficultyLevel = 2;
        LoadMainGameScene();
    }

    public void SelectHardDifficulty()
    {
        GameManager.Instance.difficultyLevel = 3;
        LoadMainGameScene();
    }

    void LoadMainGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}
