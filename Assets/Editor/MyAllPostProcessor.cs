using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyAllPostprocessor : AssetPostprocessor
{
    // AssetPostprocessor�� ��ӹ����� �Ʒ� �Լ��� ����� �� ����.
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {

        // �߰��ǰų� ������ ����� ���
        foreach (string str in importedAssets)
        {

            if (str == "Assets/xlsx/data.xlsx" || str == "Assets/xlsx/str.csv")
            {
                // csv���� ���� �о�ͼ� list<strUsage>�� ����
                DataConvertor.ReadStrUsageFromCsv();
                DataConvertor.ExcelToGameData();
                DataConvertor.UpdateCsv();
                // csv�� ����� ���� ����
            }
            else if (str == "Assets/xlsx/etcstr.csv")
                DataConvertor.MakeAssetForEtcStr();

            Debug.Log("Reimported Asset: " + str);
        }
        // ������ ���
        foreach (string str in deletedAssets)
        {
            Debug.Log("Deleted Asset: " + str);
        }
        // �̵��� ���
        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }
        // �������� �缳���� ���?
        if (didDomainReload)
        {
            Debug.Log("Domain has been reloaded");
        }
    }
}