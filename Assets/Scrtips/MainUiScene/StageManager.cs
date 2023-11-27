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


    // 스테이지 처음으로 돌아가기
    // 몬스터 스폰, 스테이지 보스(엘리트 몬스터) 스폰
    // 보스 스테이지 입장
    // 오브젝트풀 해제, 보스 소환
    // 다음 스테이지로 이동
    // 다음 스테이지 정보 일겅와서 몬스터, 보스(엘리트 몬스터) 소환

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
    void FadeIn()
    {
        StartCoroutine(FadeAlphaChange(1f, 0f));
    }

    void FadeOut()
    {
        StartCoroutine(FadeAlphaChange(0f, 1f));
    }

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
        var nextStage = gameData.stages.Find(l => l.code == playData.currentStage.code + 1);
        StartCoroutine(ChangeSceneProcessCoroutine(nextStage));
    }

    public void RestartCurrentStage(RestartCurrentStage _obj)
    {
        StartCoroutine(ChangeSceneProcessCoroutine(playData.currentStage));
    }

    IEnumerator ChangeSceneProcessCoroutine(Stage _stage, bool _isBossStage = false)
    {
        yield return FadeAlphaChange(0,1);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GameScene");

        // 비동기 작업이 완료될 때까지 기다립니다.
        while (!unloadOperation.isDone)
        {
            yield return null; // 한 프레임을 기다립니다.
        }

        playData.currentStage = _stage;
        playData.isBossStage = _isBossStage;

        var loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        // 비동기 작업이 완료될 때까지 기다립니다.
        while (!loadOperation.isDone)
        {
            yield return null; // 한 프레임을 기다립니다.
        }


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

        // 씬 언로드
        // 씬 로드

    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ChallangeToBossStage>(ChangeToBossStage);
        EventBus.Unsubscribe<RestartCurrentStage>(RestartCurrentStage);
        EventBus.Unsubscribe<ChangeToNextStage>(ChangeToNextStage);
    }
}
