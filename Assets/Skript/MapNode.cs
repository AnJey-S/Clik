using System.Collections.Generic;
public enum RoomType
{
    Enemy,
    Elite,
    Heal,
    Boss
}

public class MapNode
{
    public RoomType roomType;
    public EnemyData enemyData;
    public bool isCompleted;
    public List<MapNode> nextNodes = new List<MapNode>();
    public int column; // позиция на карте для UI
    public int row;
}
