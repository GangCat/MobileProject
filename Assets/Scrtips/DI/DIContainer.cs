using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.Rendering.DebugUI;

namespace DI
{
    /// <summary>
    /// [Inject("키")]에서 키를 설정할 수 있음. 
    /// 같은 GameObject를 받아야할 때 키로 구분 가능.
    /// </summary>
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
            try
            {
                DIContainer.Inject(o);
            }catch(Exception e)
            {
                Debug.LogError("InjectObj Exception " + o.GetType());
                Debug.LogException(e);
            }
            isInjected = true;
        }

    }

    public class DIContainer
    {     
        // 컨테이너를 리스트로 관리하면 글로벌이 딱히 없어도 리스트[0]을 파괴안하면 글로벌이나 마찬가지임.
        private static List<DIContainer> containerList = new List<DIContainer>();

        public static void AddContainer(DIContainer container)
        {
            containerList.Add(container);
        }

        public static void RemoveContainer(DIContainer container)
        {
            containerList.Remove(container);
        }
      

        // 생성자로 생성
        static DIContainer()
        {
        }

        public static T GetObjT<T>(string key = "")
        {
            return (T)GetObj(typeof(T), key);
        }

        /// <summary>
        /// 컨테이너에 저장된 오브젝트를 타입과 키 값을 가지고 찾아냄.
        /// 모든 컨테이너를 순회하며 찾아냄
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object GetObj(Type type,string key="")
        {
            var diKey = GetKey(type, key);
            object value=null;
            for (int i = 0; i < containerList.Count; ++i)
            {
                if (containerList[i] != null && containerList[i].classDictionary.TryGetValue(diKey, out value))
                {
                    if (value == null)
                        continue;
                    break;
                }
                
            }

            if (value == null)
                throw new Exception(" 등록된 오브젝트를 찾지 못했습니다 Key:"+ diKey);

            return value ;

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
       
                // 딕셔너리에서 가져올 값을 저장할 변수
                object value = GetObj(fi.FieldType, key);

                fi.SetValue(o, value);
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
