using Assets.Editor;
using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
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

    public static void ReadStrUsageFromCsv()
    {
        var dataset = GetDataSetFromCsv();

        if (dataset == null)
        {
            strUsages = new List<StrUsage>();
            return;
        }

        strUsages = MakeListFromDataSet<StrUsage>(dataset.Tables[0].TableName, dataset);
    }

    public static void MakeAssetForEtcStr()
    {
        var dataset = GetEtcStrDataSetFromCsv();

        if (dataset == null)
        {
            return;
        }

        // ����� ������ ��� ����
        var assetPath = "Assets/Prefab/etcstr.asset";
        // �켱 �ش� ��ο� ������ ������
        var etcStrList = AssetDatabase.LoadAssetAtPath<EtcStrList>(assetPath);

        // ������ ��� �������� ������ ���
        if (etcStrList == null)
        {
            // �ش� ������ ��ũ���ͺ� ������Ʈ�� ����? �ν��Ͻ��� ����
            etcStrList = ScriptableObject.CreateInstance<EtcStrList>();
            // ��ũ���ͺ� ������Ʈ�� �ش� ��ο� ����
            AssetDatabase.CreateAsset(etcStrList, assetPath);
        }

        etcStrList.etcStrList = MakeListFromDataSet<EtcStr>(dataset.Tables[0].TableName, dataset);

        // �ٲ� ���� ���������.
        EditorUtility.SetDirty(etcStrList);
        // ����� ������ ������ ������.
        AssetDatabase.SaveAssets();
    }


    public static void UpdateCsv()
    {
        DataTable dataTable = new DataTable();
        var filePath = "Assets/xlsx/str.csv";
        var classType = typeof(StrUsage);
        var fields = classType.GetFields();

        for (int i = 0; i < fields.Length; ++i)
            dataTable.Columns.Add(fields[i].Name, fields[i].FieldType);
        Debug.Log($"Count: {strUsages.Count}");
        Debug.Log($"kr: {strUsages[0].kr}");
        foreach(var strUsage in strUsages)
        {
            var row= dataTable.NewRow();

            for(int i = 0; i < fields.Length; ++i)
                row[i] = fields[i].GetValue(strUsage);

            dataTable.Rows.Add(row);
        }

        DataTableToCSV(dataTable, filePath);
    }

    static void DataTableToCSV(DataTable dataTable, string filePath)
    {
        // StreamWriter�� ����Ͽ� CSV ���Ͽ� ������ ����
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            // �� �̸� ��� ����
            foreach (DataColumn column in dataTable.Columns)
            {
                streamWriter.Write(column.ColumnName);
                streamWriter.Write(",");
            }
            streamWriter.WriteLine();

            // �� ���� �����͸� CSV ���Ͽ� ����
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    streamWriter.Write(item);
                    streamWriter.Write(",");
                }
                streamWriter.WriteLine();
            }
        }
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

                    if (fieldType == typeof(Str))
                    {
                        var str = parsable as Str;
                        // ���� ��Ʈ���� �ڵ�Į���� ����ִ��� Ȯ��
                        var codeIdx = namePerColIdx["code"];
                        // �ڵ� Į������ ���� �ο�� �����Ǵ� ��ġ�� Ȯ���ؼ� �� ���� �ڵ�� ���
                        var code = (int)Convert.ChangeType(itemtable.Rows[r][codeIdx],typeof(int));

                        var strUsage = strUsages.Find(l => l.hostName == itemtable.TableName && l.fieldName == name && l.code == code);
                        Debug.Log($"str.kor: {str.kor}");

                        if (strUsage == null)
                        {
                            strUsages.Add(new StrUsage()
                            {
                                code = code,
                                hostName = itemtable.TableName,
                                fieldName = name,
                                kr = str.kor
                            });
                        }
                        else if(strUsage.kr != str.kor)
                        {
                            strUsage.kr = str.kor;
                            strUsage.en = "";
                            strUsage.jp = "";
                        }
                        else
                        {
                            str.eng = strUsage.en;
                            str.jp = strUsage.jp;
                        }
                    }

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
            var firstVal = itemtable.Rows[r][0];
            if (firstVal is DBNull )
                continue;

            if (string.IsNullOrWhiteSpace(firstVal.ToString()))
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
            if (string.IsNullOrEmpty(fielName))
                continue;
            _tempDic.Add(fielName.Trim(), i);
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

    private static DataSet GetDataSetFromCsv()
    {
        // ���� ������ ���
        string csvPath = "Assets/xlsx/str.csv";
        // ��Ʈ���� ������, ������ ����, ������ �б⸸ �����ϰ�, fileshare�� ���ִ���?
        // -> ������ �ٸ� ������ ���� �־ ���� ��� �۾��� �ϰڴٶ�� �ǹ�
        // ���� ������ ������ ����ä�� ������ �Ͼ���� �� ������ �߻���


        if (!File.Exists(csvPath))
            return null;

        using (var stream = File.Open(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            // ��Ʈ���� ������ �ְ� ������°� ���� ������ ���� ������ dataset���� ��ȯ����.
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                return reader.AsDataSet();
            }
        }
    }

    private static DataSet GetEtcStrDataSetFromCsv()
    {
        // ���� ������ ���
        string csvPath = "Assets/xlsx/etcstr.csv";
        // ��Ʈ���� ������, ������ ����, ������ �б⸸ �����ϰ�, fileshare�� ���ִ���?
        // -> ������ �ٸ� ������ ���� �־ ���� ��� �۾��� �ϰڴٶ�� �ǹ�
        // ���� ������ ������ ����ä�� ������ �Ͼ���� �� ������ �߻���


        if (!File.Exists(csvPath))
            return null;

        using (var stream = File.Open(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            // ��Ʈ���� ������ �ְ� ������°� ���� ������ ���� ������ dataset���� ��ȯ����.
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                return reader.AsDataSet();
            }
        }
    }


    static List<StrUsage> strUsages = new List<StrUsage>();
}
