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
        EventBus.Subscribe<EnterToDungeon>(EnterToDungeon);
        EventBus.Subscribe<ReturnToLastNormalStage>(ReturnToLastNormalStage);
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

    // ���̵���, �ƿ�
    //void FadeIn()
    //{
    //    StartCoroutine(FadeAlphaChange(1f, 0f));
    //}

    //void FadeOut()
    //{
    //    StartCoroutine(FadeAlphaChange(0f, 1f));
    //}

    /// <summary>
    /// ���ø����̼� ����� ȣ��Ǵ� �Լ�.
    /// �� Ÿ�ֿ̹� ���� �� �ϸ� ������
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
        // ���� ���������� ã�Ƴ�.
        // ������ 1, 2, 3 ó���Ǽ� +1�� ã���� ��.
        // ���߿� 101, 102�� �ؾ� ��.
        playData.currentKilledEnemyCount = 0;
        var nextStage = gameData.stages.Find(l => l.code == playData.currentStage.code + 1);
        StartCoroutine(ChangeSceneProcessCoroutine(nextStage));
    }

    public void RestartCurrentStage(RestartCurrentStage _obj)
    {
        StartCoroutine(ChangeSceneProcessCoroutine(playData.currentStage));
    }

    public void EnterToDungeon(EnterToDungeon _enterDungeon)
    {
        var dungeonStage = gameData.stages.Find(l => l.code == _enterDungeon.dungeon.stageCode);
        StartCoroutine(ChangeSceneProcessCoroutine(dungeonStage));
    }

    public void ReturnToLastNormalStage(ReturnToLastNormalStage _obj)
    {
        StartCoroutine(ChangeSceneProcessCoroutine(playData.lastNormalStage));
    }

    IEnumerator ChangeSceneProcessCoroutine(Stage _stage, bool _isBossStage = false)
    {
        // ���̵� �� ���
        yield return FadeAlphaChange(0,1);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GameScene");

        // �񵿱� �۾��� �Ϸ�� ������ ���.
        while (!unloadOperation.isDone)
            yield return null;

        if (_stage.type == SpaceType.Dungeon && playData.currentStage.type == SpaceType.Stage)
            playData.lastNormalStage = playData.currentStage;

        playData.currentStage = _stage;
        playData.isBossStage = _isBossStage;


        var loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        // �񵿱� �۾��� �Ϸ�� ������ ���.
        while (!loadOperation.isDone)
            yield return null;

        // ���̵� �ƿ� ���
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
        EventBus.Unsubscribe<EnterToDungeon>(EnterToDungeon);
        EventBus.Unsubscribe<ReturnToLastNormalStage>(ReturnToLastNormalStage);
    }
}
