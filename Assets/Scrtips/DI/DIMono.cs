using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DI
{
    public class DIMono :MonoBehaviour
    {

        InjectObj InjectObj = new InjectObj();

        public void CheckAndInject()
        {
            InjectObj.CheckAndInject(this);
        }

        private void Start()
        {
            CheckAndInject();
            Init();
        }

        public virtual void Init()
        {

        }
    }
}
