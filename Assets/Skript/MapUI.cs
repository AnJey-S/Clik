using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.U2D;

public class MapUI : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за отображение карты на экране и обработку взаимодействия игрока с узлами карты. Он создает визуальные представления узлов карты, отображает их типы и доступность, а также обрабатывает выбор узла игроком для начала боя или лечения.
    // ----------------------------------------------------------------
    // Методы:
    //    - Start(): Этот метод вызывается при загрузке сцены с картой. Он инициализирует MapGenerator, генерирует карту (или загружает её из GameManager, если она уже была сгенерирована) и вызывает метод DrawMap() для отображения карты на экране.
    //    - DrawMap(): Этот метод создает визуальные представления для каждого узла карты, устанавливает их позицию, отображает иконки типов комнат, настраивает кнопки для взаимодействия и изменяет внешний вид узлов в зависимости от их доступности и статуса прохождения.
    //    - IsAccessible(MapNode node): Этот метод проверяет, доступен ли узел для выбора игроком. Узел доступен, если он находится в первой колонке или если предыдущая колонка содержит пройденный узел, который ведет к этому узлу.
    //    - SelectNode(MapNode node): Этот метод вызывается, когда игрок выбирает узел на карте. Он устанавливает текущий узел в GameManager и загружает сцену боя или применяет лечение в зависимости от типа комнаты узла.
    //    - GetRoomIcon(RoomType type): Этот метод возвращает спрайт иконки, соответствующей типу комнаты, для отображения на узлах карты.
    // Поля:
    //    - nodePrefab: Префаб для визуального представления узла карты, который должен содержать компонент Button и Image для отображения иконки комнаты.
    //    - mapContainer: Трансформ, который будет служить родителем для всех узлов карты в иерархии сцены.
    //    - horizontalSpacing: Горизонтальное расстояние между узлами в разных колонках.
    //    - verticalSpacing: Вертикальное расстояние между узлами в одной колонке.
    //    - mapGenerator: Ссылка на компонент MapGenerator для генерации карты.
    //    - map: Структура данных, представляющая карту в виде списка списков узлов (колонок и строк).
    // ----------------------------------------------------------------
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform mapContainer;
    [SerializeField] private float horizontalSpacing = 150f;
    [SerializeField] private float verticalSpacing = 100f;

    private MapGenerator mapGenerator;
    private List<List<MapNode>> map;

    public SpriteAtlas UIIcons;

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
                //TMP_Text label = obj.GetComponentInChildren<TMP_Text>();
                //label.text = GetRoomIcon(node.roomType);

                // Иконка комнаты
                Button nodeButton = obj.GetComponent<Button>();
                Transform childTransform = nodeButton.transform.Find("NodeIcon");
                Image childImage = childTransform.GetComponent<Image>();
                childImage.sprite = GetRoomIcon(node.roomType);

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

    //private string GetRoomIcon(RoomType type)
    //{
    //    switch (type)
    //    {
    //        case RoomType.Enemy: return "⚔";
    //        case RoomType.Elite: return "💀";
    //        case RoomType.Heal: return "❤";
    //        case RoomType.Boss: return "👑";
    //        default: return "?";
    //    }
    //}

    private Sprite GetRoomIcon(RoomType type)
    {
        switch (type)
        {
            case RoomType.Enemy: return UIIcons.GetSprite("monster_black");
            case RoomType.Elite: return UIIcons.GetSprite("elite_black");
            case RoomType.Heal: return UIIcons.GetSprite("chest_black");
            case RoomType.Boss: return UIIcons.GetSprite("boss_black");
            default: return UIIcons.GetSprite("unknown_black");
        }
    }
}
