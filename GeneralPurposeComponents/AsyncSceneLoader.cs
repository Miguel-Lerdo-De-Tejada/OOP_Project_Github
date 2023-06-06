using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    bool isLoading = false;
    const float c_loadingAmount = 0.9f;

    public void Load(GameObject uiLoadScene, string sceneToLoad)
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(AsyncLoad(uiLoadScene, sceneToLoad));
        }
    }

    public void LoadWithIndex(GameObject uiLoadScene, int sceneToLoad)
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(AsyncIndexLoad(uiLoadScene, sceneToLoad));
        }
    }

    IEnumerator AsyncLoad(GameObject uiLoadScene, string sceneToLoad)
    {
        uiLoadScene.SetActive(true);
        Slider progressBar = uiLoadScene.GetComponentInChildren<Slider>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);        

        while (!operation.isDone)
        {
            float progressPercent = Mathf.Clamp01(operation.progress / c_loadingAmount);            
            progressBar.value = progressPercent;
            yield return null;
        }        
    }

    IEnumerator AsyncIndexLoad(GameObject uiLoadScene, int sceneToLoad)
    {
        uiLoadScene.SetActive(true);
        Slider progressBar = uiLoadScene.GetComponentInChildren<Slider>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            float progressPercent = Mathf.Clamp01(operation.progress / c_loadingAmount);
            progressBar.value = progressPercent;
            yield return null;
        }
    }
}
