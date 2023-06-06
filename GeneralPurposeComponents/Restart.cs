using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField, Tooltip("UI Title")] GameObject uiLoadTitle;

    struct Scenes
    {
        public static string title = "Title";
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadTitle() { LoadTitleSceneAsynchronously(); }

    void LoadTitleSceneAsynchronously()
    {
        AsyncSceneLoader asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
        if (asyncSceneLoader)
        {
            if (uiLoadTitle)
            {
                asyncSceneLoader.Load(uiLoadTitle, Scenes.title);
            }
            else
            {
                SceneManager.LoadScene(Scenes.title);
            }
        }
        else
        {
            if (uiLoadTitle)
            {
                uiLoadTitle.SetActive(true);
                SceneManager.LoadScene(Scenes.title);
            }
            else
            {
                SceneManager.LoadScene(Scenes.title);
            }
        }
    }
}
