using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IXCollision
{
    // x축 요소만 가지고 충돌 검사하는 함수
    public bool IsCollide(IXCollision target)
    {
        var dist = Mathf.Abs(CenterX - target.CenterX);
        
        return dist < (Width + target.Width)*0.5f;
    }

    public float CenterX { get; }
    public float Width { get; }
}
