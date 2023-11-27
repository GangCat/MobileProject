using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : DIMono
{
    public Image fadeImage;
    public float fadeDuration;

    [Inject]
    PlayData playData;
    [Inject]
    GameData gameData;

    public override void Init()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        EventBus.Subscribe<ChallangeToBossStage>(ChangeToBossStage);
        EventBus.Subscribe<RestartCurrentStage>(RestartCurrentStage);
        EventBus.Subscribe<ChangeToNextStage>(ChangeToNextStage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var nextStage = gameData.stages.Find(l => l.code == playData.currentStage.code + 1);
            StartCoroutine(ChangeSceneProcessCoroutine(nextStage));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(ChangeSceneProcessCoroutine(playData.currentStage, true));
        }
    }

    // 페이드인, 아웃
    //void FadeIn()
    //{
    //    StartCoroutine(FadeAlphaChange(1f, 0f));
    //}

    //void FadeOut()
    //{
    //    StartCoroutine(FadeAlphaChange(0f, 1f));
    //}

    /// <summary>
    /// 어플리케이션 종료시 호출되는 함수.
    /// 이 타이밍에 해제 등 하면 좋을듯
    /// </summary>
    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        EventBus.Publish(new ApplicationQuitEvent());
        //EventBus.Subscribe<EnterToBossStage>();
    }

    public void ChangeToBossStage(ChallangeToBossStage _obj)
    {
        StartCoroutine(ChangeSceneProcessCoroutine(playData.currentStage, true));
    }

    public void ChangeToNextStage(ChangeToNextStage _obj)
    {
        // 다음 스테이지를 찾아냄.
        // 지금은 1, 2, 3 처럼되서 +1로 찾으면 됨.
        // 나중에 101, 102로 해야 함.
        var nextStage = gameData.stages.Find(l => l.code == playData.currentStage.code + 1);
        StartCoroutine(ChangeSceneProcessCoroutine(nextStage));
    }

    public void RestartCurrentStage(RestartCurrentStage _obj)
    {
        StartCoroutine(ChangeSceneProcessCoroutine(playData.currentStage));
    }

    IEnumerator ChangeSceneProcessCoroutine(Stage _stage, bool _isBossStage = false)
    {
        // 페이드 인 대기
        yield return FadeAlphaChange(0,1);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GameScene");

        // 비동기 작업이 완료될 때까지 대기.
        while (!unloadOperation.isDone)
            yield return null;

        playData.currentStage = _stage;
        playData.isBossStage = _isBossStage;

        var loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        // 비동기 작업이 완료될 때까지 대기.
        while (!loadOperation.isDone)
            yield return null;

        // 페이드 아웃 대기
        yield return FadeAlphaChange(1, 0);
    }

    IEnumerator FadeAlphaChange(float startAlpha, float endAlpha)
    {
        var color = fadeImage.color;
        float elapse = 0;
        while (elapse < fadeDuration)
        {
            var t = elapse / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            color.a = alpha;

            fadeImage.color = color;
            elapse += Time.deltaTime;
            yield return null;
        }
        color.a = endAlpha;
        fadeImage.color = color;

    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ChallangeToBossStage>(ChangeToBossStage);
        EventBus.Unsubscribe<RestartCurrentStage>(RestartCurrentStage);
        EventBus.Unsubscribe<ChangeToNextStage>(ChangeToNextStage);
    }
}
