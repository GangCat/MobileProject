using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class BackendLogin
{
    private static BackendLogin _instance = null;

    public static BackendLogin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendLogin();
            }

            return _instance;
        }
    }

    public void CustomSignUp(string id, string pw)
    {
        //Debug.Log("ȸ�������� ��û�մϴ�.");

        //var bro = Backend.BMember.CustomSignUp(id, pw);

        //if (bro.IsSuccess())
        //{
        //    Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        //}
        //else
        //{
        //    Debug.LogError("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        //}
    }

    public void CustomLogin(string id, string pw)
    {
        // Step 3. �α��� �����ϱ� ����
    }

    public void UpdateNickname(string nickname)
    {
        // Step 4. �г��� ���� �����ϱ� ����
    }
}