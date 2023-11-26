using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DataConvertor
{

    public T aa<T>()
    {
        return default(T);
    }

    // ���� ���Ͽ� �����ϴ� ������ �о gamedata�� �����ϴ� �Լ�
    public static void ExcelToGameData()
    {
        // ����� ������ ��� ����
        var assetPath = "Assets/Prefab/gamedata.asset";
        // �켱 �ش� ��ο� ������ ������
        var gameData = AssetDatabase.LoadAssetAtPath<GameData>(assetPath);

        // ������ ��� �������� ������ ���
        if (gameData == null)
        {
            // �ش� ������ ��ũ���ͺ� ������Ʈ�� ����? �ν��Ͻ��� ����
            gameData = ScriptableObject.CreateInstance<GameData>();
            // ��ũ���ͺ� ������Ʈ�� �ش� ��ο� ����
            AssetDatabase.CreateAsset(gameData, assetPath);
        }

        //���� ������ �����ͼ����� ������.
        var dataset = GetDataSetFromExcel();

        // ���� Ŭ������ Ÿ���� ������.
        var classType = typeof(DataConvertor);
        // Ÿ���� �޼ҵ带 ������, �� �� �̸��� �ش� Ŭ������ ������ ��Ʈ�÷����������� �༭ ��Ȯ�� �޼ҵ带 ���������� ������.
        var methodInfo = classType.GetMethod("MakeListFromDataSet", BindingFlags.NonPublic | BindingFlags.Static);

        // ���ӵ����� ������ Ÿ���� ������.
        var gameDataType = gameData.GetType();

        // ���� ������ Ÿ���� ��� �ʵ忡 ������.
        foreach (var fieldInfo in gameDataType.GetFields())
        {
            // �ʵ��� Ÿ���� � �����ΰ� Ȯ��.
            // �Ʒ� ����� List<int> dictionary<string, int> ó�� ���ʸ��� ��� ����ϴ� ���.
            // �̰� list���� dictionary���� Ȯ��
            if (fieldInfo.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                continue;

            // �޼ҵ��� �ν��Ͻ�������� ��?
            // ���ʸ� �Լ��� T�� �� �ڷ����� ���ؼ� �Լ��� �ϳ� ����� ���̶�� �����ϸ� ��.
            // �ش� ���ʸ� �޼ҵ��� ���ʸ� Ÿ��, ������ �ڷ����� Ÿ��<T>�� �־ �ش� �ʵ��� �ν��Ͻ��� ����.
            // argument = ����,[0]�� ��ųʸ��� ��� 0, 1 2���� ����. ����Ʈ��  1��  �������� 0���� �������.
            var makeListMethodInfo = methodInfo.MakeGenericMethod(fieldInfo.FieldType.GetGenericArguments()[0]);
            //������ ���� �ν��Ͻ�, �޼ҵ� ������ ����� ���� ������ �Է�����.�� �� null�� static�̱� ������  ����ϴ�  ��ü��  ��� null��.
            // new object[] {} �� �ȿ��Ű������� �־��ش�����.
            fieldInfo.SetValue(gameData, makeListMethodInfo.Invoke(null, new object[2] { fieldInfo.Name, dataset }));
        }
        // �ٲ� ���� ���������.
        EditorUtility.SetDirty(gameData);
        // ����� ������ ������ ������.
        AssetDatabase.SaveAssets();
    }

    private static List<T> MakeListFromDataSet<T>(string sheetName, DataSet dataset) where T : new()
    {
        var type = typeof(T);

        // �� ������ �̸�(code, name, price ���)�� ��� ��ġ�ϴ��� ������� �ش� �̸��� Į�� �ε����� �����ϱ� ���ѵ�ųʸ�
        Dictionary<string, int> namePerColIdx = new Dictionary<string, int>();
        // ����� ��ȯ�� ����Ʈ ����
        var result = new List<T>();
        // ����� ���� ��Ʈ�� �̸�
        var itemtable = dataset.Tables[sheetName];

        // Į��, �ο��� ����
        var col = itemtable.Columns.Count;
        var row = itemtable.Rows.Count;

        // ������ ���̺��� �̸��� ã�� ��ųʸ��� ä����
        FillIdx(itemtable, namePerColIdx);

        for (int r = 1; r < row; ++r)
        {
            // �� ���� ��ü ����
            T tempItem = new T();

            foreach (var kp in namePerColIdx)
            {
                var name = kp.Key;
                var colIdx = kp.Value;

                // �����Է��� ���� ����
                var cellValue = itemtable.Rows[r][colIdx];
                // ���� ��������� ��Ƽ��
                if (cellValue is DBNull)
                    continue;

                // ���ʸ��� ������ �ڷ����� Ÿ���� �ʵ带 �������µ� �� �� �̸��� �̿��ؼ� �̸��� ������ �ʵ��� ������ ������.
                var fi = type.GetField(name);
                // ���� ���ٸ� ��Ƽ��
                if (fi == null)
                    continue;

                // �ʵ��� Ÿ��(�ڷ���)�� �̸� ����
                var fieldType = fi.FieldType;
                //  �������� ������ ��, SetValue�� �� �� ��
                object finalValue;

                // IParsable�̶�� �������̽��� ������� Ȯ����.
                // �ش� �������̽��� Data���� Ȯ���غ� �� ����. ��� �����ϰ� ����ϴ���
                if(fieldType.GetInterface("IParsable")!=null)
                {
                    // Activator.CreateInstance �޼ҵ带 ����ϸ� �ش� �ʵ�Ÿ���� �ν��Ͻ�, ��ü�� ������.
                    var obj = Activator.CreateInstance(fieldType);
                    // ������ ��ü�� Ÿ���� �������̽��� ��ü�� ��ȯ.
                    var parsable= obj as IParsable;
                    // �������̽��� �Լ��� ȣ��
                    parsable.FillFromStr(cellValue.ToString());
                    // �� ����
                    finalValue = parsable;
                }
                // Ȥ�� Enum�̶��
                else if (fieldType.IsEnum)
                {
                    // Enum�� �Ľ��ؼ� �� ����.
                    finalValue = Enum.Parse(fieldType, (string)cellValue);
                }
                else
                {
                    // ����Ʈ�� ���麯ȯ�� �� ����.
                    // ������ �̷��� �⺻ �ڷ����� ������.
                    finalValue = Convert.ChangeType(cellValue, fieldType);
                }

                // �� ������ ���� ����.
                fi.SetValue(tempItem, finalValue);
                // �̸� �ݺ�
            }

            // ���� ��Ʈ�� ����(code, price ��)�� ���ٸ� ��Ƽ��
            if (itemtable.Rows[r][0] is DBNull)
                continue;
            // ������� �������� ��ü�� ����
            result.Add(tempItem);
        }
        
        return result;
    }

    private static void FillIdx(DataTable _dataTable, Dictionary<string, int> _tempDic)
    {
        int col = _dataTable.Columns.Count;

        for (int i = 0; i < col; ++i)
        {
            // ù��° ���� ���� ������ �ϳ��� ������.
            var cellObj = _dataTable.Rows[0][i];
            // cellObj�� Ÿ���� string�� �ƴϸ� ��Ƽ��
            if (cellObj is string == false)
                continue;

            // string���� �ٲٰ� ��ųʸ��� ����
            var fielName = cellObj as string;
            _tempDic.Add(fielName, i);
        }

    }

    // ������ ������ dataset���� ��ȯ���ִ� �Լ�.
    // �̰Ŵ� ���¿��� �����°ŷ� �����.
    private static DataSet GetDataSetFromExcel()
    {
        // ���� ������ ���
        string excelPath = "Assets/xlsx/data.xlsx";
        // ��Ʈ���� ������, ������ ����, ������ �б⸸ �����ϰ�, fileshare�� ���ִ���?
        // -> ������ �ٸ� ������ ���� �־ ���� ��� �۾��� �ϰڴٶ�� �ǹ�
        // ���� ������ ������ ����ä�� ������ �Ͼ���� �� ������ �߻���
        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            // ��Ʈ���� ������ �ְ� ������°� ���� ������ ���� ������ dataset���� ��ȯ����.
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                return reader.AsDataSet();
            }
        }

    }
}
