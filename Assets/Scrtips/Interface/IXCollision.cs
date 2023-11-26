using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IXCollision
{
    // �浹 �˻��ϴ� �Լ�
    public bool IsCollide(IXCollision target)
    {
        var dist = Mathf.Abs(CenterX - target.CenterX);
        
        return dist < (Width + target.Width)*0.5f;
    }

    public float CenterX { get; }
    public float Width { get; }
}
