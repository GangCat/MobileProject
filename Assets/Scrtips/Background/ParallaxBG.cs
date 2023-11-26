using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBG : MonoBehaviour
{
    // 일종의 수치 모음집
    public MaterialPropertyBlock block;

    SpriteRenderer spRenderer;

    public float parallaxSpeed;

    public float xUnit;

    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();

        // 이렇게 하면 같은 머테리얼을 사용하고 있는 다른 오브젝트들에게 영향을 주지 않고 쉐이더 수치를 조절할 수 있음.
        block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", spRenderer.sprite.texture);

        var sp = spRenderer.sprite;

        // 스프라이트의 너비를 pixelperunit으로 나누면 총 몇 Unit인지 알 수 있음.
        // 픽셀 개수 / (픽셀 개수 / 1유닛) 이라서 그럼
        xUnit= sp.rect.width / sp.pixelsPerUnit;
    }

    void Update()
    {
//        spRenderer.sprite.tex

        block.SetFloat("_Strength", parallaxSpeed);
        // 위에서 구한 값으로 위치갱신을 조절해주면 캐릭터가 1유닛 움직일 때 배경도 1유닛 움직임(speed 1일 경우)
        block.SetFloat("_Offset", this.transform.position.x/ xUnit);
        // 마지막으로 이거 해줘야 수치조절한거 적용됨
        spRenderer.SetPropertyBlock(block);
    }
}
