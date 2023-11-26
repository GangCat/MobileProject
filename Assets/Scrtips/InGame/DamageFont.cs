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
        // �̷����ϸ� 000,000�� �ǰ� float�� ������ ǥ����.
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
            // Ŀ�꿡�� ���� �ð��� t�� ���� ���� ������.
            var scale= scalCurve.Evaluate(t);
            // Ŀ�긦 �̿��� �������� ������ ������ ��.
            this.transform.localScale = scale*Vector3.one;
            // ��� ���
            this.transform.position= startPos + new Vector3(0,  Mathf.Lerp(0, YDist, t));
            yield return null;
        }

        poolManager.ReturnObj(this.gameObject);
        //this.transform.position = startPos + new Vector3(0, YDist);

    }

}
