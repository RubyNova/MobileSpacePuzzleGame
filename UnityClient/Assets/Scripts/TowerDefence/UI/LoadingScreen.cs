using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingUI;

    [SerializeField]
    private Slider _loadingBar;

    [SerializeField]
    private Text _loadingPercent;

    public void LoadLevel (string _levelName) => StartCoroutine(LoadAsynchronously(_levelName));

    IEnumerator LoadAsynchronously (string _levelName)
    {
        AsyncOperation _loading = SceneManager.LoadSceneAsync(_levelName);

        _loadingUI.SetActive(true);

        while(_loading.isDone == false)
        {
            float _progress = Mathf.Clamp01(_loading.progress / .9f) * 100f;

            _loadingBar.value = _progress;

            _loadingPercent.text = _progress.ToString("0") + "%";

            yield return null;
        }
    }
}
