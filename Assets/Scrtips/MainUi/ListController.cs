using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController<T> : MonoBehaviour
{
    // 화면에 표시될 화면, 마스크처리되서 딱 보이는 화면
    public ScrollRect scrollRect;
    public RectTransform viewPortRect;
    // 실제 내용이 다 들어가있을 컨텐트의 렉트 트랜스폼
    public RectTransform ContentRt => scrollRect.content;
    // 복사할 셀의 게임오브젝트
    GameObject firstItemCell;
    // 아이템 셀 리스트
    List<ListItemCell<T>> itemCells;
    // 데이터(스킬, 아이템 등등)의 리스트
    List<T> data;
    // 비활성화된 셀들을 저장하는 큐
    Queue<ListItemCell<T>> freeCells = new Queue<ListItemCell<T>>();
    // 활성화시켜야하는 셀들의 인덱스를 저장하는 큐
    // 즉, 여기 들어온 인덱스에 해당하는 정보가 화면에 표시되어야 한다는 뜻.
    HashSet<int> needSetIndexies = new HashSet<int>();
    // 스크롤렉트의 렉트 확인용
    public Rect sRect;

    float itemCellHeight;
    float scrollRectHeight;
    float viewPortHeight;
    (int, int) prevStartEndIdx;

    /// <summary>
    /// 데이터 초기화(입력)
    /// </summary>
    /// <param name="list"></param>
    public void SetData(List<T> list )
    {
        data = list;
        
        // 각종 자주쓰일 변수 선언.

        // 컨텐트의 크기 수정
        // 이 때 Vector2는 값복사임. 그러니 값의x나 y에 접근해봤자 원본이 변하지 않음.
        // ContentRt.sizeDelta = size 이거는 약간 int a = 3;이랑 같은 느낌임.
        var size = ContentRt.sizeDelta;
        size.y = itemCellHeight * list.Count;
        ContentRt.sizeDelta = size;

        // 최초로 표시될 셀의 시작, 종료 인덱스 계산
        var startEndIdx = CalStartEndIdx();

        //충돌검사
        UpdateCellPositions(startEndIdx.Item1, startEndIdx.Item2);
    }

    /// <summary>
    /// 셀들의 위치를 변경시켜줌.
    /// 즉, 비활성화된 셀들을 새로운 위치로 이동시킴과 동시에 정보를 입력해줌.
    /// </summary>
    /// <param name="startIdx"></param>
    /// <param name="endIdx"></param>
    void UpdateCellPositions(int startIdx, int endIdx)
    {
        needSetIndexies.Clear();
        freeCells.Clear();

        for (int idx= startIdx; idx <= endIdx; idx++)
        {
            needSetIndexies.Add(idx);
        }

        // 이미 활성화되어있는 셀의 경우 활성화시킬 인덱스에서 제외
        // 1, 2가 이미 활성화된 상태에서 1,2,3을 출력해야 할 경우 1, 2를 Remove함.
        foreach (var cell in itemCells)
        {
            if (startIdx<= cell.idx && cell.idx <= endIdx)
            {
                cell.gameObject.SetActive(true);
                needSetIndexies.Remove(cell.idx);
                continue;
            }

            freeCells.Enqueue(cell);
        }

        // 위에서 1, 2가 Remove되어 남은 3을 출력하는 내용.
        Debug.Log("-----");
        foreach (var idx in needSetIndexies)
        {
            var cell = freeCells.Dequeue();
            var rt = (cell.transform as RectTransform);

            Debug.Log($"--{idx}---");
            var aPos= rt.anchoredPosition;
            aPos.y = idx * -itemCellHeight;
            rt.anchoredPosition = aPos;

            cell.SetData(data[idx], idx);
            cell.gameObject.SetActive(true);
        }
        Debug.Log("--End---");

        // 위의 1, 2, 3이 아닌 0의 경우 비활성화.
        foreach (var cell in freeCells)
        {
            Debug.Log("FreeCellDeactive " + cell.idx);
            cell.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 이렇게 2개를 반환하게 할 수 있음.
    /// 저것도 일종의 사용자정의 자료형인가?
    /// </summary>
    /// <returns></returns>
    (int,int) CalStartEndIdx()
    {
        // 컨텐트의 y좌표를 가져옴.
        float contentPosY = ContentRt.anchoredPosition.y;

        // 이 인덱스는 화면의 가장 위쪽에 보일 셀의 인덱스를 의미함.
        int lowestIdxInScreen = (int)(contentPosY / itemCellHeight);
        // 이 인덱스는 현재 화면에 보일 총 셀의 개수를 의미함.
        // 가장 위의 셀의 높이를 렉트의 높이에서 뺀 값을 셀의 높이로 나눈 몫임.
        // 그러므로 계산되는 개수는 총 표시될 셀의 개수 -1개임.
        //var cellCountInScreen = Mathf.CeilToInt( (scrollRectHeight - (itemCellHeight - (contentPosY % itemCellHeight))) / itemCellHeight);
        var cellCountInScreen = Mathf.CeilToInt( (viewPortHeight - (itemCellHeight - (contentPosY % itemCellHeight))) / itemCellHeight);

        var startIdx = lowestIdxInScreen;
        var endIdx = lowestIdxInScreen + cellCountInScreen;

        startIdx = Mathf.Max(0, startIdx);
        endIdx = Mathf.Min(endIdx, data.Count - 1);

        return (startIdx,endIdx);
    }

    private void Update()
    {
        var startEndIdx = CalStartEndIdx();
        if (prevStartEndIdx == startEndIdx)
            return;

        Debug.Log($"startIdx: {startEndIdx.Item1} / endIdx: {startEndIdx.Item2} ");
        UpdateCellPositions(startEndIdx.Item1, startEndIdx.Item2);

        prevStartEndIdx = startEndIdx;

        // 아래는 overlap이라는걸 이용해서 충돌처리를 할 수 있음을 알려줌.
        //foreach (var cell in itemCells)
        //{
        //    var cellRt = (cell.transform as RectTransform);

        //    if (cellRt.rect.Overlaps(scrollRectRt.rect))
        //    {
        //        cell.name = "Overlap "+ cellRt.anchoredPosition.y;
        //    }
        //    else
        //    {
        //        cell.name = "None";
        //    }
        //}
    }

    protected virtual void Start()
    {
        firstItemCell = ContentRt.GetChild(0).gameObject;
        itemCells = new List<ListItemCell<T>>();

        scrollRectHeight = (scrollRect.transform as RectTransform).rect.height;
        viewPortHeight = viewPortRect.rect.height;
        itemCellHeight = (firstItemCell.transform as RectTransform).rect.height;
        sRect = (scrollRect.transform as RectTransform).rect;

        int maxCellCount = Mathf.RoundToInt(scrollRectHeight / itemCellHeight) + 1;
        Debug.Log($"maxCell{maxCellCount} {scrollRectHeight} {itemCellHeight}");

        // 아이템셀이 화면에 표시될 최대 개수를 정해서
        // 해당 개수만큼 미리 아이템셀을 만들어서 오브젝트 풀링처럼 사용
        GameObject copyGo = firstItemCell;

        var firstCell=firstItemCell.GetComponent<ListItemCell<T>>();
        firstCell.idx = -1;
        itemCells.Add(firstCell);
        for(int c = 1; c < maxCellCount; ++c)
        {          
            var cell = Instantiate(copyGo, ContentRt).GetComponent<ListItemCell<T>>();
            cell.idx = -1;
            itemCells.Add(cell);
        }

    }
}
