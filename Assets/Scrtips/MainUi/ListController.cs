using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController<T> : MonoBehaviour
{
    // ȭ�鿡 ǥ�õ� ȭ��, ����ũó���Ǽ� �� ���̴� ȭ��
    public ScrollRect scrollRect;
    public RectTransform viewPortRect;
    // ���� ������ �� ������ ����Ʈ�� ��Ʈ Ʈ������
    public RectTransform ContentRt => scrollRect.content;
    // ������ ���� ���ӿ�����Ʈ
    GameObject firstItemCell;
    // ������ �� ����Ʈ
    List<ListItemCell<T>> itemCells;
    // ������(��ų, ������ ���)�� ����Ʈ
    List<T> data;
    // ��Ȱ��ȭ�� ������ �����ϴ� ť
    Queue<ListItemCell<T>> freeCells = new Queue<ListItemCell<T>>();
    // Ȱ��ȭ���Ѿ��ϴ� ������ �ε����� �����ϴ� ť
    // ��, ���� ���� �ε����� �ش��ϴ� ������ ȭ�鿡 ǥ�õǾ�� �Ѵٴ� ��.
    HashSet<int> needSetIndexies = new HashSet<int>();
    // ��ũ�ѷ�Ʈ�� ��Ʈ Ȯ�ο�
    public Rect sRect;

    float itemCellHeight;
    float scrollRectHeight;
    float viewPortHeight;
    (int, int) prevStartEndIdx;

    /// <summary>
    /// ������ �ʱ�ȭ(�Է�)
    /// </summary>
    /// <param name="list"></param>
    public void SetData(List<T> list )
    {
        data = list;
        
        // ���� ���־��� ���� ����.

        // ����Ʈ�� ũ�� ����
        // �� �� Vector2�� ��������. �׷��� ����x�� y�� �����غ��� ������ ������ ����.
        // ContentRt.sizeDelta = size �̰Ŵ� �ణ int a = 3;�̶� ���� ������.
        var size = ContentRt.sizeDelta;
        size.y = itemCellHeight * list.Count;
        ContentRt.sizeDelta = size;

        // ���ʷ� ǥ�õ� ���� ����, ���� �ε��� ���
        var startEndIdx = CalStartEndIdx();

        //�浹�˻�
        UpdateCellPositions(startEndIdx.Item1, startEndIdx.Item2);
    }

    /// <summary>
    /// ������ ��ġ�� ���������.
    /// ��, ��Ȱ��ȭ�� ������ ���ο� ��ġ�� �̵���Ŵ�� ���ÿ� ������ �Է�����.
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

        // �̹� Ȱ��ȭ�Ǿ��ִ� ���� ��� Ȱ��ȭ��ų �ε������� ����
        // 1, 2�� �̹� Ȱ��ȭ�� ���¿��� 1,2,3�� ����ؾ� �� ��� 1, 2�� Remove��.
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

        // ������ 1, 2�� Remove�Ǿ� ���� 3�� ����ϴ� ����.
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

        // ���� 1, 2, 3�� �ƴ� 0�� ��� ��Ȱ��ȭ.
        foreach (var cell in freeCells)
        {
            Debug.Log("FreeCellDeactive " + cell.idx);
            cell.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// �̷��� 2���� ��ȯ�ϰ� �� �� ����.
    /// ���͵� ������ ��������� �ڷ����ΰ�?
    /// </summary>
    /// <returns></returns>
    (int,int) CalStartEndIdx()
    {
        // ����Ʈ�� y��ǥ�� ������.
        float contentPosY = ContentRt.anchoredPosition.y;

        // �� �ε����� ȭ���� ���� ���ʿ� ���� ���� �ε����� �ǹ���.
        int lowestIdxInScreen = (int)(contentPosY / itemCellHeight);
        // �� �ε����� ���� ȭ�鿡 ���� �� ���� ������ �ǹ���.
        // ���� ���� ���� ���̸� ��Ʈ�� ���̿��� �� ���� ���� ���̷� ���� ����.
        // �׷��Ƿ� ���Ǵ� ������ �� ǥ�õ� ���� ���� -1����.
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

        // �Ʒ��� overlap�̶�°� �̿��ؼ� �浹ó���� �� �� ������ �˷���.
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

        // �����ۼ��� ȭ�鿡 ǥ�õ� �ִ� ������ ���ؼ�
        // �ش� ������ŭ �̸� �����ۼ��� ���� ������Ʈ Ǯ��ó�� ���
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
