using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventBus
{

    static Dictionary<Type, List<Delegate>> typeToHandlers = new Dictionary<Type, List<Delegate>>();


    static List<Delegate> _tempDelegateList=new List<Delegate>();

    /// <summary>
    /// 이벤트 버스를 통해 클래스를 던져 해당 클래스에 등록한 구독자들의 등록된 액션을 호출함.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventObj"></param>
    public static void Publish<T>(T eventObj)
    {
        var type = typeof(T);
        if (typeToHandlers.TryGetValue(type, out var handlers) == false)
        {
            return;
        }

        // 이렇게 따로 저장해주면 발행되는 과정에서 파괴되는 액션들이 있어도 에러가 발생하지 않음.
        _tempDelegateList.Clear();
        _tempDelegateList.AddRange(handlers);


        foreach (var h in _tempDelegateList)
        {
            (h as System.Action<T>)(eventObj);
        }

    }

    /// <summary>
    /// 내가 원하는 클래스에 내가 실행할 함수를 등록함.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onEventHandler"></param>
    public static void Subscribe<T>(System.Action<T> onEventHandler)
    {
        var type = typeof(T);
        if(typeToHandlers.TryGetValue(type,out var handlers) ==false)
        {
            typeToHandlers.Add(type, new List<Delegate>());
        }

        typeToHandlers[type].Add(onEventHandler);
    }

    /// <summary>
    /// 등록을 해제함.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onEventHandler"></param>
    public static void Unsubscribe<T>(System.Action<T> onEventHandler)
    {
        var type = typeof(T);
        if (typeToHandlers.ContainsKey(type) == false)
            return;

        typeToHandlers[type].Remove(onEventHandler);
    }


}