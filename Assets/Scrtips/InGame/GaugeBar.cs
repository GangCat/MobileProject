using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeBar : MonoBehaviour
{
    public Transform gaugeFrontTr;
    public void UpdateLength(float _ratio)
    {
        gaugeFrontTr.localScale = new Vector3(Mathf.Max(0f,_ratio), 1f, 1f);
    }
}
