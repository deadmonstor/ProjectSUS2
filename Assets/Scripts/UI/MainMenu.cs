using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup controls;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject startButton;
    
    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }
    
    public void OnExit()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnControls()
    {
        EventSystem.current.SetSelectedGameObject(backButton);
        controls.interactable = true;
        controls.alpha = 1f;
        controls.blocksRaycasts = true;
    }

    public void ExitControls()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
        controls.interactable = false;
        controls.alpha = 0f;
        controls.blocksRaycasts = false;
    }
}
