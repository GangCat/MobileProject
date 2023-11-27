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


        // 글로벌은 컨테이너를 만들어주는게 아님.
        // 생성자 호출하면 만들어짐.

        // 가장 먼저 호출되는게 이 함수인데 DIContainer를 생성한적이 없는 것 같은데 Global이 있는 이유?
        // -> 정적 생성자는 클래스가 최초로 호출될 때 호출된다.
        container.Regist(new PlayData()
        {
            currentStage= gamedata.stages[0]
        });
        Debug.Log("Bootstrap!!");
    }

}
