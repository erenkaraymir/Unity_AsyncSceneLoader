using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsnycSceneLoader : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private  TMP_Text loadingText;
    [SerializeField] private SO_LevelIndex _levelData;
    [SerializeField] private float _loadTime = 6f;

    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        float targetProgress = 1f; 

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_levelData.Level);
        asyncOperation.allowSceneActivation = false;
        
        while (loadingSlider.value < targetProgress)
        {
            loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, targetProgress, Time.deltaTime / _loadTime);
            loadingText.text = (loadingSlider.value * 100f).ToString("F0") + "%";

            yield return null;
        }

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingSlider.value = progress;
            loadingText.text = (progress * 100f).ToString("F0") + "%";

            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingText.text = "100%";
                asyncOperation.allowSceneActivation = true; 
            }

            yield return null;
        }
    }
}
