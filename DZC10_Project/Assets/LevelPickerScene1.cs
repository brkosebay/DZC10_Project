using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPickerScene1 : MonoBehaviour
{
      public void PlayGame(){
        SceneManager.LoadSceneAsync("MainGameScene");
    }
}
