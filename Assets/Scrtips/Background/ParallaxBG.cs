using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBG : MonoBehaviour
{
    // ������ ��ġ ������
    public MaterialPropertyBlock block;

    SpriteRenderer spRenderer;

    public float parallaxSpeed;

    public float xUnit;

    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();

        // �̷��� �ϸ� ���� ���׸����� ����ϰ� �ִ� �ٸ� ������Ʈ�鿡�� ������ ���� �ʰ� ���̴� ��ġ�� ������ �� ����.
        block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", spRenderer.sprite.texture);

        var sp = spRenderer.sprite;

        // ��������Ʈ�� �ʺ� pixelperunit���� ������ �� �� Unit���� �� �� ����.
        // �ȼ� ���� / (�ȼ� ���� / 1����) �̶� �׷�
        xUnit= sp.rect.width / sp.pixelsPerUnit;
    }

    void Update()
    {
//        spRenderer.sprite.tex

        block.SetFloat("_Strength", parallaxSpeed);
        // ������ ���� ������ ��ġ������ �������ָ� ĳ���Ͱ� 1���� ������ �� ��浵 1���� ������(speed 1�� ���)
        block.SetFloat("_Offset", this.transform.position.x/ xUnit);
        // ���������� �̰� ����� ��ġ�����Ѱ� �����
        spRenderer.SetPropertyBlock(block);
    }
}
