using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace DI
{


    public class InjectAttribute : Attribute
    {
        public readonly string key;
        public InjectAttribute( string key="")
        {
            this.key = key;
        }

    }


    public class InjectObj
    {
        bool isInjected;

        // 현재 객체가 이미 인젝트 되었는지 확인하고 그렇지 않다면 인젝트하도록 하는 함수.
        public void CheckAndInject(object o)
        {
            if (isInjected)
            {
                return;
            }
            DIContainer.Inject(o);
            isInjected = true;
        }

    }

    


   
    public class DIContainer
    {
        // 이건 씬 바뀌어도 사용할 컨테이너
        static DIContainer _global;
        public static DIContainer Global=>_global;
        // 이건 씬마다 사용할 컨테이너
        public static DIContainer Local { get; set; }

        // 생성자로 생성
        static DIContainer()
        {
            _global = new DIContainer();
        }

        public static T GetObj<T>(string key="")
        {
            var diKey = GetKey(typeof(T), key);
            object value;
            if (_global.classDictionary.TryGetValue(diKey, out value))
            {
                // 딕셔너리에 해당 키가 있으면 들어옴
                // 그런데 그 값이 null이면, 예를들어 해당 오브젝트(변수에 들어가야했던)가 파괴되었다면 예외처리
                if (value == null)
                    throw new Exception("Unity Object Destroyed");             
            }
            // 글로벌에 넣을 값이 없으면 로컬에 넣을 값이 있는지 확인
            // 이 때 우선 Local이 널인지부터 확인
            else if (Local != null && Local.classDictionary.TryGetValue(diKey, out value))
            {
                if (value == null)
                    throw new Exception("Unity Object Destroyed");
            }

            return (T)value ;

        }

        // Inject하는 함수.
        // 즉, 내가 가지고 있는 변수중에 Inject해야할 변수들을 얘가 찾아서 알아서 값을 넣어줌.
        public static void Inject(object o)
        {
            // 가져올 대상은 모든 Instance
            FieldInfo[] fieldInfos = o.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fi in fieldInfos)
            {
                // InjectAttribute가 있는지 확인
                var injectInstance = fi.GetAttribute<InjectAttribute>();
                if (injectInstance == null)
                    continue;

                // 있으면 해당 Inject의 키 가져옴. [Inject("1")]에서 1
                var key = injectInstance.key;
                // 가져온 key를 가지고 딕셔너리의 키를 만들어 저장
                key = GetKey(fi.FieldType, key);
                // 딕셔너리에서 가져올 값을 저장할 변수
                object value;

                // 우선 글로벌에 넣을 값을 확인
                if(_global.classDictionary.TryGetValue(key, out value))
                {
                    // 딕셔너리에 해당 키가 있으면 들어옴
                    // 그런데 그 값이 null이면, 예를들어 해당 오브젝트(변수에 들어가야했던)가 파괴되었다면 예외처리
                    if(value == null)
                        throw new Exception("Unity Object Destroyed");

                    // 아니라면 값 설정
                    fi.SetValue(o, value);
                }
                // 글로벌에 넣을 값이 없으면 로컬에 넣을 값이 있는지 확인
                // 이 때 우선 Local이 널인지부터 확인
                else if (Local != null && Local.classDictionary.TryGetValue(key, out value))
                {
                    if (value == null)
                        throw new Exception("Unity Object Destroyed");

                    fi.SetValue(o, value);
                }
                // 로컬이 null이거나 Type이 Regist되지 않아 저장된 값이 없다면 여기로
                else
                {
                    if(Local == null)
                        throw new Exception("Local DIContainer Not Instantiated");

                    throw new Exception("Type Not Registered "+key);
                }
            }
        }

        // 딕셔너리에 값을 넣는 함수
        public void Regist<T>( T t, string key ="")
        {
            // 키 값을 가져옴.
            var diKey = GetKey(t.GetType(), key);
            // 이미 저장된 값이 있다면 예외처리
            if (classDictionary.ContainsKey(diKey))
            {
                throw new Exception("Same Key registered");
            }

            // 값 추가
            classDictionary.Add(diKey, t);
        }

        // 딕셔너리에 사용할 키를 만드는 함수
        public  static  string GetKey(Type t ,string key)
        {
            return t.FullName +"@"+ key;
        }

       
        //public void _Inject(object obj)
        //{
        //    FieldInfo[] fieldInfos =  obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    foreach(var fi in fieldInfos)
        //    {
        //        var injectInstance = fi.GetAttribute<InjectAttribute>();
        //        if (injectInstance == null)
        //            continue;

        //        var key = injectInstance.key;
        //        key = GetKey(fi.FieldType, key);
        //        object value;
        //        if (classDictionary.TryGetValue(key, out value))
        //        {
        //            fi.SetValue(obj, value);
        //        }
        //    }

        //}

        private Dictionary<string, object> classDictionary = new Dictionary<string, object>();
    }
}
