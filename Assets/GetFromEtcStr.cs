using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TMPro.TMP_Text))]
public class GetFromEtcStr : DIMono
{
    [Inject]
    EtcStrList etcStrList;

    public string code;

    TMPro.TMP_Text txt;
    public override void Init()
    {
        base.Init();
        txt=this.GetComponent<TMPro.TMP_Text>();
        txt.text = etcStrList.GetStr(code);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<LanguageChangeEvent>(OnLanguageChangeEvent);
    }

    private void OnLanguageChangeEvent(LanguageChangeEvent obj)
    {
        txt.text = etcStrList.GetStr(code);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LanguageChangeEvent>(OnLanguageChangeEvent);
    }


}
