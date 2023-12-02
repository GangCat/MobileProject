using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPStrUGUI : TMPro.TextMeshProUGUI
{

    Str _str;
    public Str Str { get => _str;
        set
        {
            _str = value;
            text = _str.ToString();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<LanguageChangeEvent>(OnLanguageChangeEvent);
    }

    private void OnLanguageChangeEvent(LanguageChangeEvent obj)
    {
        text = Str.ToString();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<LanguageChangeEvent>(OnLanguageChangeEvent);
    }

}
