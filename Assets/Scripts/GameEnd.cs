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
        SceneManager.UnloadScene(2);
        SceneManager.UnloadScene(1);
        SceneManager.LoadScene(1);
        
        this.gameObject.SetActive(false);
    }
    
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
