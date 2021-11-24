using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Image image = null;
    [SerializeField] private float minimumTime = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private float loadingProgress = 0f;
    private float timeLoading = 0f;

    #endregion

    #region PUBLIC_METHODS

    public void LoadScene(string scene)
    {
        IEnumerator LoadAssets()
        {
            loadingProgress = 0;
            timeLoading = 0;
            yield return null;

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                timeLoading += Time.deltaTime;
                loadingProgress = operation.progress + 0.1f;
                loadingProgress = loadingProgress * timeLoading / minimumTime;

                if (loadingProgress >= 1)
                {
                    operation.allowSceneActivation = true;
                }

                image.fillAmount = loadingProgress;

                yield return null;
            }
        }
        StartCoroutine(LoadAssets());
    }

    #endregion
}
