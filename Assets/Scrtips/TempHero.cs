using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TempHero : DIMono
{
    // �̷��� ���� �صθ� Inject�ȴ�
    [Inject]
    PlayData playData;

    [Inject]
    public Camera cam;

    GameObject tempPrefab;

    public List<GameObject> objs =new List<GameObject>();

    public override void Init()
    {
     
        // �̰� ������� ���� ����
        // awake���� �ν��緯�� �� �־��ְ� ���� �̰� �����ؾ���.


        // �̷��� ������ �����ؼ� �����Ҽ����ְ�
        // tempPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();

        //    for (int i = 0; i < 3; ++i)
        // Instantiate(tempPrefab, transform);
        //      Addressables.InstantiateAsync("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            // �̷��� �ٷ� �ν��Ͻ��� �����Ҽ��� �ִ�.
            // �ٵ� �̶��� ������Ʈ�� ������ ������Ѵ�.
            // �ٷ� �����Ҷ��� �ڷ��� �������� �ʾƵ� �˾Ƽ� GameObject�� ����ȴ�.
            var o=Addressables.InstantiateAsync("Assets/Prefab/P_TempImage.prefab").WaitForCompletion();

            objs.Add(o);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            var o =objs.FirstOrDefault();
            if (o != null)
            {
                // �̷������� ������������Ѵ�.
                Addressables.ReleaseInstance(o);
                objs.Remove(o);
            }
        }


    }

    private void OnDestroy()
    {
        // �̷��� �������������
        Addressables.Release(tempPrefab);

    }


}
