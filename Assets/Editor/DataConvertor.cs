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

    // 엑셀 파일에 존재하는 내용을 읽어서 gamedata에 저장하는 함수
    public static void ExcelToGameData()
    {
        // 저장될 에셋의 경로 지정
        var assetPath = "Assets/Prefab/gamedata.asset";
        // 우선 해당 경로에 에셋을 가져옴
        var gameData = AssetDatabase.LoadAssetAtPath<GameData>(assetPath);

        // 에셋이 없어서 가져오지 못했을 경우
        if (gameData == null)
        {
            // 해당 에셋을 스크립터블 오브젝트로 생성? 인스턴스로 생성
            gameData = ScriptableObject.CreateInstance<GameData>();
            // 스크립터블 오브젝트를 해당 경로에 생성
            AssetDatabase.CreateAsset(gameData, assetPath);
        }

        //엑셀 정보를 데이터셋으로 가져옴.
        var dataset = GetDataSetFromExcel();

        // 현재 클래스의 타입을 가져옴.
        var classType = typeof(DataConvertor);
        // 타입의 메소드를 가져옴, 이 때 이름과 해당 클래스의 정보를 비트플래그형식으로 줘서 정확한 메소드를 가져오도록 도와줌.
        var methodInfo = classType.GetMethod("MakeListFromDataSet", BindingFlags.NonPublic | BindingFlags.Static);

        // 게임데이터 에셋의 타입을 가져옴.
        var gameDataType = gameData.GetType();

        // 게임 데이터 타입의 모든 필드에 접근함.
        foreach (var fieldInfo in gameDataType.GetFields())
        {
            // 필드의 타입이 어떤 형식인가 확인.
            // 아래 방법은 List<int> dictionary<string, int> 처럼 제너릭일 경우 사용하는 방법.
            // 이건 list인지 dictionary인지 확인
            if (fieldInfo.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                continue;

            // 메소드의 인스턴스를만드는 것?
            // 제너릭 함수의 T에 들어갈 자료형을 정해서 함수를 하나 만드는 것이라고 생각하면 됨.
            // 해당 제너릭 메소드의 제너릭 타입, 저장할 자료형의 타입<T>을 넣어서 해당 필드의 인스턴스를 만듬.
            // argument = 인자,[0]은 딕셔너리의 경우 0, 1 2개가 있음. 리스트는  1개  뿐이지만 0으로 만들었음.
            var makeListMethodInfo = methodInfo.MakeGenericMethod(fieldInfo.FieldType.GetGenericArguments()[0]);
            //위에서 만든 인스턴스, 메소드 인포를 사용해 값을 실제로 입력해줌.이 때 null은 static이기 때문에  사용하는  객체가  없어서 null임.
            // new object[] {} 이 안에매개변수를 넣어준느것임.
            fieldInfo.SetValue(gameData, makeListMethodInfo.Invoke(null, new object[2] { fieldInfo.Name, dataset }));
        }
        // 바뀐 값을 적용시켜줌.
        EditorUtility.SetDirty(gameData);
        // 적용된 에셋을 실제로 저장함.
        AssetDatabase.SaveAssets();
    }

    private static List<T> MakeListFromDataSet<T>(string sheetName, DataSet dataset) where T : new()
    {
        var type = typeof(T);

        // 각 구분의 이름(code, name, price 등등)이 어디에 위치하는지 상관없이 해당 이름의 칼럼 인덱스를 저장하기 위한딕셔너리
        Dictionary<string, int> namePerColIdx = new Dictionary<string, int>();
        // 결과로 반환할 리스트 생성
        var result = new List<T>();
        // 사용할 엑셀 시트의 이름
        var itemtable = dataset.Tables[sheetName];

        // 칼럼, 로우의 개수
        var col = itemtable.Columns.Count;
        var row = itemtable.Rows.Count;

        // 아이템 테이블에서 이름을 찾아 딕셔너리를 채워줌
        FillIdx(itemtable, namePerColIdx);

        for (int r = 1; r < row; ++r)
        {
            // 각 행의 객체 생성
            T tempItem = new T();

            foreach (var kp in namePerColIdx)
            {
                var name = kp.Key;
                var colIdx = kp.Value;

                // 현재입력할 셀의 정보
                var cellValue = itemtable.Rows[r][colIdx];
                // 셀이 비어있으면 컨티뉴
                if (cellValue is DBNull)
                    continue;

                // 제너릭에 설정된 자료형의 타입의 필드를 가져오는데 이 때 이름을 이용해서 이름과 동일한 필드의 인포를 가져옴.
                var fi = type.GetField(name);
                // 만약 없다면 컨티뉴
                if (fi == null)
                    continue;

                // 필드의 타입(자료형)을 미리 선언
                var fieldType = fi.FieldType;
                //  마지막에 저장할 값, SetValue할 때 들어갈 값
                object finalValue;

                // IParsable이라는 인터페이스를 지녔는지 확인함.
                // 해당 인터페이스는 Data에서 확인해볼 수 있음. 어떻게 선언하고 사용하는지
                if(fieldType.GetInterface("IParsable")!=null)
                {
                    // Activator.CreateInstance 메소드를 사용하면 해당 필드타입의 인스턴스, 객체를 생성함.
                    var obj = Activator.CreateInstance(fieldType);
                    // 생성한 객체의 타입을 인터페이스의 객체로 변환.
                    var parsable= obj as IParsable;
                    // 인터페이스의 함수를 호출
                    parsable.FillFromStr(cellValue.ToString());
                    // 값 갱신
                    finalValue = parsable;
                }
                // 혹시 Enum이라면
                else if (fieldType.IsEnum)
                {
                    // Enum을 파싱해서 값 저장.
                    finalValue = Enum.Parse(fieldType, (string)cellValue);
                }
                else
                {
                    // 컨버트를 쓰면변환할 수 있음.
                    // 하지만 이러면 기본 자료형만 가능함.
                    finalValue = Convert.ChangeType(cellValue, fieldType);
                }

                // 셋 벨류로 값을 저장.
                fi.SetValue(tempItem, finalValue);
                // 이를 반복
            }

            // 엑셀 시트의 구분(code, price 등)이 없다면 컨티뉴
            if (itemtable.Rows[r][0] is DBNull)
                continue;
            // 결과값에 아이템의 객체를 저장
            result.Add(tempItem);
        }
        
        return result;
    }

    private static void FillIdx(DataTable _dataTable, Dictionary<string, int> _tempDic)
    {
        int col = _dataTable.Columns.Count;

        for (int i = 0; i < col; ++i)
        {
            // 첫번째 행의 셀의 정보를 하나씩 가져옴.
            var cellObj = _dataTable.Rows[0][i];
            // cellObj의 타입이 string이 아니면 컨티뉴
            if (cellObj is string == false)
                continue;

            // string으로 바꾸고 딕셔너리에 저장
            var fielName = cellObj as string;
            _tempDic.Add(fielName, i);
        }

    }

    // 엑셀의 정보를 dataset으로 전환해주는 함수.
    // 이거는 에셋에서 가져온거로 기억함.
    private static DataSet GetDataSetFromExcel()
    {
        // 엑셀 파일의 경로
        string excelPath = "Assets/xlsx/data.xlsx";
        // 스트림을 정의함, 파일을 오픈, 접근을 읽기만 가능하게, fileshare는 왜있더라?
        // -> 파일을 다른 곳에서 쓰고 있어도 내가 열어서 작업을 하겠다라는 의미
        // 저게 없으면 엑셀이 열린채로 엑셀을 일어들일 때 에러가 발생함
        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            // 스트림을 변수로 주고 리더라는걸 만들어서 리더가 읽은 내용을 dataset으로 변환해줌.
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                return reader.AsDataSet();
            }
        }

    }
}
