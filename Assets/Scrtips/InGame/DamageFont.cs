using DI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : DIMono
{

    [Inject]
    ObjectPoolManager poolManager;

    public AnimationCurve scalCurve;
    public TMP_Text text;

    public float duration = 2;
    public float YDist = 2;

    public void Show(float _dmg, Color _color)
    {
        // 이렇게하면 000,000이 되고 float도 정수로 표현됨.
        text.text = string.Format("{0:#,0}", _dmg);

        text.color = _color;

        StartCoroutine(AutoReturnCoroutine());
    }

    private IEnumerator AutoReturnCoroutine()
    {
        var startPos= this.transform.position;
        float elapse = 0;

        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            var t = elapse / duration;
            // 커브에서 현재 시각인 t일 때의 값을 가져옴.
            var scale= scalCurve.Evaluate(t);
            // 커브를 이용해 스케일을 조절해 느낌을 줌.
            this.transform.localScale = scale*Vector3.one;
            // 등속 상승
            this.transform.position= startPos + new Vector3(0,  Mathf.Lerp(0, YDist, t));
            yield return null;
        }

        poolManager.ReturnObj(this.gameObject);
        //this.transform.position = startPos + new Vector3(0, YDist);

    }

}
