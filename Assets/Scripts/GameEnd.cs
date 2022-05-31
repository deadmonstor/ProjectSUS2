using System;
using System.Collections;
using System.Collections.Generic;
using LevelSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public GameObject quitTop;
    public Level level;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(quitTop);
    }

    public void Continue()
    {
        StartCoroutine(level.StartGame());
    }
    
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
