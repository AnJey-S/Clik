using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс отвечает за генерацию карты с узлами и назначение врагов для каждого узла. Он создает структуру карты с определенными типами комнат (обычные враги, элита, босс, лечение) и соединяет узлы между собой.
    // Он также назначает врагов для каждого узла на основе его типа, используя данные о врагах, заданные в инспекторе.
    // ----------------------------------------------------------------
    //
    // Методы:
    //    - GenerateMap(): Этот метод создает структуру карты, заполняя её узлами с определенными типами комнат и соединяя их между собой. Он также назначает врагов для каждого узла на основе его типа.
    //    - CreateColumn(int count, RoomType type, int col, bool withHeal = false): Этот метод создает колонку узлов с заданным количеством узлов, типом комнаты и номером колонки. Он также может случайным образом назначать одну из комнат в колонке как комнату лечения.
    //    - ConnectNodes(List<List<MapNode>> columns): Этот метод соединяет узлы между собой, создавая пути от узлов в одной колонке к узлам в следующей колонке.
    //    - AssignEnemies(List<List<MapNode>> columns): Этот метод назначает врагов для каждого узла на основе его типа комнаты, используя данные о врагах, заданные в инспекторе.
    // Поля:
    //    - regularEnemies: Список данных о обычных врагах, из которых будут случайным образом назначаться враги для узлов с типом комнаты "Enemy".
    //    - eliteEnemy: Данные об элитном враге, который будет назначаться для узлов с типом комнаты "Elite".
    //    - bossEnemy: Данные о боссе, который будет назначаться для узлов с типом комнаты "Boss".
    // ----------------------------------------------------------------
    [Header("Враги")]
    [SerializeField] private List<EnemyData> regularEnemies;
    [SerializeField] private EnemyData eliteEnemy;
    [SerializeField] private EnemyData bossEnemy;

    // Структура карты:
    // Колонка 0: старт (1 узел)
    // Колонки 1-3: обычные враги (2 пути)
    // Колонка 4: элита (1 узел)
    // Колонки 5-7: обычные враги (2 пути)
    // Колонка 8: элита (1 узел)
    // Колонка 9: босс (1 узел)

    public List<List<MapNode>> GenerateMap()
    {
        List<List<MapNode>> columns = new List<List<MapNode>>();

        // Колонка 0 — старт, одна комната обычного врага
        columns.Add(CreateColumn(1, RoomType.Enemy, 0));

        // Колонки 1-3 — два пути, обычные враги + возможно лечение
        for (int col = 1; col <= 3; col++)
            columns.Add(CreateColumn(2, RoomType.Enemy, col, withHeal: true));

        // Колонка 4 — элита
        columns.Add(CreateColumn(1, RoomType.Elite, 4));

        // Колонки 5-7 — два пути, обычные враги + возможно лечение
        for (int col = 5; col <= 7; col++)
            columns.Add(CreateColumn(2, RoomType.Enemy, col, withHeal: true));

        // Колонка 8 — элита
        columns.Add(CreateColumn(1, RoomType.Elite, 8));

        // Колонка 9 — босс
        columns.Add(CreateColumn(1, RoomType.Boss, 9));

        ConnectNodes(columns);
        AssignEnemies(columns);

        return columns;
    }

    private List<MapNode> CreateColumn(int count, RoomType type, int col, bool withHeal = false)
    {
        List<MapNode> nodes = new List<MapNode>();
        for (int i = 0; i < count; i++)
        {
            MapNode node = new MapNode();
            node.column = col;
            node.row = i;

            // Один из двух путей может быть комнатой лечения
            if (withHeal && count > 1 && i == 1 && Random.value > 0.5f)
                node.roomType = RoomType.Heal;
            else
                node.roomType = type;

            nodes.Add(node);
        }
        return nodes;
    }

    private void ConnectNodes(List<List<MapNode>> columns)
    {
        for (int col = 0; col < columns.Count - 1; col++)
        {
            List<MapNode> current = columns[col];
            List<MapNode> next = columns[col + 1];

            foreach (MapNode node in current)
                foreach (MapNode nextNode in next)
                    node.nextNodes.Add(nextNode);
        }
    }

    private void AssignEnemies(List<List<MapNode>> columns)
    {
        foreach (List<MapNode> column in columns)
        {
            foreach (MapNode node in column)
            {
                switch (node.roomType)
                {
                    case RoomType.Enemy:
                        node.enemyData = regularEnemies[Random.Range(0, regularEnemies.Count)];
                        break;
                    case RoomType.Elite:
                        node.enemyData = eliteEnemy;
                        break;
                    case RoomType.Boss:
                        node.enemyData = bossEnemy;
                        break;
                    case RoomType.Heal:
                        node.enemyData = null;
                        break;
                }
            }
        }
    }
}
