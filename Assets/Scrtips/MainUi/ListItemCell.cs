using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListItemCell<T> : MonoBehaviour
{
    public TMPStrUGUI text;

    public int idx;

    public virtual void SetData(T _data,int _idx)
    {
        text.text = _data.ToString();
        idx = _idx;
    }


}
