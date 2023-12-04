using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyAllPostprocessor : AssetPostprocessor
{
    // AssetPostprocessor를 상속받으면 아래 함수를 사용할 수 있음.
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {

        // 추가되거나 내용이 변경된 경우
        foreach (string str in importedAssets)
        {

            if (str == "Assets/xlsx/data.xlsx" || str == "Assets/xlsx/str.csv")
            {
                // csv에서 파일 읽어와서 list<strUsage>에 저장
                DataConvertor.ReadStrUsageFromCsv();
                DataConvertor.ExcelToGameData();
                DataConvertor.UpdateCsv();
                // csv에 변경된 사항 적용
            }
            else if (str == "Assets/xlsx/etcstr.csv")
                DataConvertor.MakeAssetForEtcStr();

            Debug.Log("Reimported Asset: " + str);
        }
        // 삭제된 경우
        foreach (string str in deletedAssets)
        {
            Debug.Log("Deleted Asset: " + str);
        }
        // 이동한 경우
        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }
        // 도메인이 재설정된 경우?
        if (didDomainReload)
        {
            Debug.Log("Domain has been reloaded");
        }
    }
}