using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EtcStr {

    public string code;
    public string kor, eng, jp;

    public override string ToString()
    {
        switch (Str.CurrentLanguage)
        {
            case Str.Language.Kor:
                return kor;
            case Str.Language.Eng:
                return eng;
            case Str.Language.Jap:
                return jp;
        }

        return kor;
    }

}

public class LanguageChangeEvent
{

}

[Serializable]
public class Str : IParsable
{
    public int code;

    public enum Language
    {
        Kor,Eng,Jap
    }

    private static Language _CurrentLanguage;
    public static Language CurrentLanguage
    {
        get
        {
            return _CurrentLanguage;
        }
        set
        {
            if (_CurrentLanguage == value)
            {
                return;
            }

            _CurrentLanguage = value;
            EventBus.Publish(new LanguageChangeEvent());
        }
    }

    public string kor, eng, jp;

    public override string ToString()
    {
        switch (CurrentLanguage)
        {
            case Language.Kor:
                return kor;
            case Language.Eng:
                return eng;
            case Language.Jap:
                return jp;
        }

        return kor;
    }
    public static implicit operator string(Str v)
    {
        return v.ToString();
    }

    public void FillFromStr(string str)
    {
        kor = str;
    }

}