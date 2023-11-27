using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bootstrap 
{
    // 앱 실행 후 스플래시 화면을 표시하기 전에 초기화
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    // 초기화할때 불리는 함수
    public static void Initialize()
    {
        var gamedata = Addressables.LoadAssetAsync<GameData>("Assets/Prefab/gamedata.asset").WaitForCompletion();

        gamedata.Init();
        DIContainer container = new DIContainer();
        DIContainer.AddContainer(container);
        container.Regist(gamedata);
        container.Regist(new UserData());

        container.Regist(new PlayData()
        {
            // 컨테이너 리스트의[0]이 글로벌임.
            currentStage= gamedata.stages[0]
        });
        Debug.Log("Bootstrap!!");
    }

}
