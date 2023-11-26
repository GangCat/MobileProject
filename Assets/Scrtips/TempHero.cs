using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TempHero : DIMono
{
    // 이렇게 선언만 해두면 Inject된다
    [Inject]
    PlayData playData;

    [Inject]
    public Camera cam;

    GameObject tempPrefab;

    public List<GameObject> objs =new List<GameObject>();

    public override void Init()
    {
     
        // 이거 해줘야지 변수 들어옴
        // awake에서 인스톨러가 다 넣어주고 나서 이거 실행해야함.


        // 이렇게 변수에 저장해서 생성할수도있고
        // tempPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();

        //    for (int i = 0; i < 3; ++i)
        // Instantiate(tempPrefab, transform);
        //      Addressables.InstantiateAsync("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            // 이렇게 바로 인스턴스를 생성할수도 있다.
            // 근데 이때는 오브젝트를 릴리즈 해줘야한다.
            // 바로 생성할때는 자료형 지정하지 않아도 알아서 GameObject로 적용된다.
            var o=Addressables.InstantiateAsync("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();

            objs.Add(o);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            var o =objs.FirstOrDefault();
            if (o != null)
            {
                // 이런식으로 릴리즈해줘야한다.
                Addressables.ReleaseInstance(o);
                objs.Remove(o);
            }
        }


    }

    private void OnDestroy()
    {
        // 이렇게 릴리즈해줘야함
        Addressables.Release(tempPrefab);

    }


}
