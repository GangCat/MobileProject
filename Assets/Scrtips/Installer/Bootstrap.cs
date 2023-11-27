using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bootstrap 
{
    // �� ���� �� ���÷��� ȭ���� ǥ���ϱ� ���� �ʱ�ȭ
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    // �ʱ�ȭ�Ҷ� �Ҹ��� �Լ�
    public static void Initialize()
    {
        var gamedata = Addressables.LoadAssetAsync<GameData>("Assets/Prefab/gamedata.asset").WaitForCompletion();

        gamedata.Init();
        DIContainer.Global.Regist(gamedata);

        DIContainer.Global.Regist(new UserData());


        // �۷ι��� �����̳ʸ� ������ִ°� �ƴ�.
        // ������ ȣ���ϸ� �������.

        // ���� ���� ȣ��Ǵ°� �� �Լ��ε� DIContainer�� ���������� ���� �� ������ Global�� �ִ� ����?
        // -> ���� �����ڴ� Ŭ������ ���ʷ� ȣ��� �� ȣ��ȴ�.
        DIContainer.Global.Regist(new PlayData()
        {
            currentStage= gamedata.stages[0]
        });
        Debug.Log("Bootstrap!!");
    }

}