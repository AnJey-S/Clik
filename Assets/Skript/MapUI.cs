using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform mapContainer;
    [SerializeField] private float horizontalSpacing = 150f;
    [SerializeField] private float verticalSpacing = 100f;

    private MapGenerator mapGenerator;
    private List<List<MapNode>> map;

    private void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        // Генерируем карту только если её ещё нет
        if (GameManager.Instance.currentMap == null)
        {
            map = mapGenerator.GenerateMap();
            GameManager.Instance.currentMap = map;
        }
        else
        {
            map = GameManager.Instance.currentMap;
        }

        DrawMap();
    }

    private void DrawMap()
    {
        foreach (List<MapNode> column in map)
        {
            foreach (MapNode node in column)
            {
                GameObject obj = Instantiate(nodePrefab, mapContainer);
                RectTransform rt = obj.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(
                    node.column * horizontalSpacing,
                    node.row * verticalSpacing
                );

                // Текст иконки комнаты
                TMP_Text label = obj.GetComponentInChildren<TMP_Text>();
                label.text = GetRoomIcon(node.roomType);

                // Кнопка
                Button btn = obj.GetComponent<Button>();
                MapNode captured = node;
                btn.onClick.AddListener(() => SelectNode(captured));

                // Недоступные комнаты затемняем
                bool isAccessible = IsAccessible(node);
                btn.interactable = isAccessible;

                Color color = obj.GetComponent<Image>().color;
                color.a = isAccessible ? 1f : 0.4f;
                obj.GetComponent<Image>().color = color;

                // Пройденные комнаты отмечаем
                if (node.isCompleted)
                {
                    color = Color.green;
                    color.a = 0.6f;
                    obj.GetComponent<Image>().color = color;
                }
            }
        }
    }

    private bool IsAccessible(MapNode node)
    {
        // Первая колонка всегда доступна
        if (node.column == 0) return !node.isCompleted;

        // Остальные — только если предыдущая пройдена и ведёт к этому узлу
        List<MapNode> previousColumn = map[node.column - 1];
        foreach (MapNode prevNode in previousColumn)
            if (prevNode.isCompleted && prevNode.nextNodes.Contains(node))
                return true;

        return false;
    }

    private void SelectNode(MapNode node)
    {
        GameManager.Instance.currentNode = node;

        switch (node.roomType)
        {
            case RoomType.Enemy:
            case RoomType.Elite:
            case RoomType.Boss:
                GameManager.Instance.LoadBattle();
                break;
            case RoomType.Heal:
                GameManager.Instance.HealPlayerPercent(0.25f);
                node.isCompleted = true;
                DrawMap(); // перерисовываем карту
                break;
        }
    }

    private string GetRoomIcon(RoomType type)
    {
        switch (type)
        {
            case RoomType.Enemy: return "⚔";
            case RoomType.Elite: return "💀";
            case RoomType.Heal: return "❤";
            case RoomType.Boss: return "👑";
            default: return "?";
        }
    }
}
