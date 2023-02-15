using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Create EnemyData")]
public class Enemy_Data : ScriptableObject
{
    public enum Type
    {
        attack_type,
        passive_type,
        Item
    }

    [SerializeField, Header("飛来物名")]
    private string enemy_name;

    [SerializeField, Header("飛来物タイプ")]
    private Type enemy_type;

    [SerializeField, Header("飛来物プレハブ")]
    private GameObject enemy_object;


    public string Enemy_name { get { return enemy_name; } private set { enemy_name = value; } }
    public Type Enemy_type { get { return enemy_type; } private set { enemy_type = value; } }
    public GameObject Enemy_object { get { return enemy_object; } private set { enemy_object = value; } }

}
