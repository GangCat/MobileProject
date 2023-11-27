using UnityEngine;
using Unity;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class ObjectPoolManager :MonoBehaviour
{
    Dictionary<string, ObjectPool> assetPathToObjectPool = new Dictionary<string, ObjectPool>();
    Dictionary<GameObject, string> pathToObject = new();

    /// <summary>
    /// 오브젝트 풀을 생성
    /// </summary>
    /// <param name="path"></param>
    /// <param name="increaseCnt"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public ObjectPool PrepareObjects(string path,int increaseCnt=5)
    {
        if (assetPathToObjectPool.ContainsKey(path))
            throw new System.ArgumentException($"이미 풀이 등록되어있습니다 [{path}]");

        // 오브젝트 풀을 보관할 홀더 생성
        var poolGoHolder= new GameObject();
        // 오브젝트의 path를 확인하고 가장 마지막, 즉 ~~~.prefab을 이름으로 설정.
        var delimiterIdx= path.LastIndexOf("/");
        if (delimiterIdx == -1)
            poolGoHolder.name = path;
        else
            poolGoHolder.name = path.Substring(delimiterIdx+1);
        // 해당 홀더의 부모를 매니저로 생성
        poolGoHolder.transform.SetParent(this.transform);
        // path를 이용해 오브젝트를 찾아옴
        var poolObj=Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
        // 오브젝트를 아이템으로 하는 오브젝트풀 생성
        var objPool=new ObjectPool(poolObj, poolGoHolder.transform, increaseCnt);
        assetPathToObjectPool.Add(path, objPool);

        return objPool;
    }

    public GameObject GetObject(string path)
    {
        // 등록된 풀이 없으면 생성
        if (assetPathToObjectPool.TryGetValue(path, out var objectPool) == false)
            objectPool=PrepareObjects(path);

        var obj= objectPool.ActivatePoolItem();
        // 오브젝트를 키로 하는 딕셔너리에 추가
        pathToObject.Add(obj,path);

        return obj;
    }

    public void ReturnObj(GameObject obj)
    {
        // 오브젝트를 받으면 오브젝트를 키로 하는 딕셔너리에서 해당 오브젝트의 path를 찾아옴
        if(pathToObject.TryGetValue(obj,out var path) == false)
            throw new System.ArgumentException($"오브젝트가 풀에 등록되어있지 않습니다. [{obj.name}]");

        // 딕셔너리에서 오브젝트 제거
        pathToObject.Remove(obj);
        // 위에서 찾은 path를 가진 오브젝트 풀에게 오브젝트 반환
        assetPathToObjectPool[path].DeactivatePoolItem(obj);
    }

    public void OnDestroy()
    {
        // 파괴시 모두 해제
        ReleasePoolObjects();
    }

    public void ReleasePoolObjects()
    {
        foreach (ObjectPool op in assetPathToObjectPool.Values)
            Addressables.Release(op.PoolObject);
    }

}
