using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDataBase : ScriptableObject
{
    public List<EnemyStatusInformation> EnemyStatusList = new List<EnemyStatusInformation>();
}

//敵の名前（種類）
public enum EnemyId
{
    Robot = 0,
    Drone,
}
