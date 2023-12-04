using DI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAlert : DIMono
{
    public GameObject AlertObj;
    public TMP_Text text;
    public int autoHideDelay = 2;

    [Inject]
    EtcStrList etcStrList;

    public override void Init()
    {
        EventBus.Subscribe<ErrorMessageEvent>(OnAlert);
    }

    public void OnAlert(ErrorMessageEvent _obj)
    {
        AlertObj.SetActive(true);
        text.text = etcStrList.GetStr(_obj.etcStrCode);
        StartCoroutine(AutoHideAlertCoroutine());
    }

    private IEnumerator AutoHideAlertCoroutine()
    {
        yield return new WaitForSeconds(autoHideDelay);

        AlertObj.SetActive(false);
    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe<ErrorMessageEvent>(OnAlert);
    }


}
