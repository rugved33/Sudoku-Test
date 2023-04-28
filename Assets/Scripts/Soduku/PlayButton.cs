using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    public void ContinueNextScene()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
