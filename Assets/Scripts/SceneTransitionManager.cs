using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public float instantTransitionFadeDelay = 0.5f;
    public float instantTransitionDelay = 2.0f;
    public static SceneTransitionManager singleton;

    private void Awake()
    {
        if (singleton && singleton != this)
            Destroy(singleton);

        singleton = this;
    }

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    public void GoToSceneInstant(int sceneIndex)
    {
        // 1. Okamžitě nastavíme plnou barvu (alpha = 1)
        fadeScreen.SetAlpha(1.0f);

        // 2. Okamžitě načteme scénu
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneInstantWithDelay(int sceneIndex)
    {
        StartCoroutine(CombinedTransitionRoutine(sceneIndex));
    }

    private IEnumerator CombinedTransitionRoutine(int sceneIndex)
    {
        // 1. Čekáme na první krok (zatmění)
        yield return new WaitForSeconds(instantTransitionFadeDelay);
        fadeScreen.SetAlpha(1.0f);

        // 2. Čekáme na druhý krok (načtení scény)
        // POZOR: Tady čekáme DALŠÍ čas, nebo odečteme ten první.
        // Pokud chceš, aby scéna proběhla celkem za 2s od začátku:
        yield return new WaitForSeconds(instantTransitionDelay);

        SceneManager.LoadScene(sceneIndex);
    }
}
