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


    // �������� ó������ ���ư���
    // ���� ����, �������� ����(����Ʈ ����) ����
    // ���� �������� ����
    // ������ƮǮ ����, ���� ��ȯ
    // ���� ���������� �̵�
    // ���� �������� ���� �ϰϿͼ� ����, ����(����Ʈ ����) ��ȯ

    public override void Init()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var nextStage = gameData.stages.Find(l => l.code == playData.currentStage.code + 1);
            StartCoroutine(ChangeSceneProcessCoroutine(nextStage));
        }
    }

    // ���̵���, �ƿ�
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
    }

    IEnumerator ChangeSceneProcessCoroutine(Stage _stage)
    {
        yield return FadeAlphaChange(0,1);
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GameScene");

        // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.
        while (!unloadOperation.isDone)
        {
            yield return null; // �� �������� ��ٸ��ϴ�.
        }

        playData.currentStage = _stage;

        var loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.
        while (!loadOperation.isDone)
        {
            yield return null; // �� �������� ��ٸ��ϴ�.
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

        // �� ��ε�
        // �� �ε�

    }
    // ���� �������� ���� ���
}
