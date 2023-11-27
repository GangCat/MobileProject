using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventBus
{

    static Dictionary<Type, List<Delegate>> typeToHandlers = new Dictionary<Type, List<Delegate>>();


    static List<Delegate> _tempDelegateList=new List<Delegate>();

    public static void Publish<T>(T eventObj)
    {
        var type = typeof(T);
        if (typeToHandlers.TryGetValue(type, out var handlers) == false)
        {
            return;
        }

        _tempDelegateList.Clear();
        _tempDelegateList.AddRange(handlers);


        foreach (var h in _tempDelegateList)
        {
            (h as System.Action<T>)(eventObj);
        }

    }

    public static void Subscribe<T>(System.Action<T> onEventHandler)
    {
        var type = typeof(T);
        if(typeToHandlers.TryGetValue(type,out var handlers) ==false)
        {
            typeToHandlers.Add(type, new List<Delegate>());
        }

        typeToHandlers[type].Add(onEventHandler);
    }


    public static void Unsubscribe<T>(System.Action<T> onEventHandler)
    {
        var type = typeof(T);
        if (typeToHandlers.ContainsKey(type) == false)
            return;

        typeToHandlers[type].Remove(onEventHandler);
    }


}