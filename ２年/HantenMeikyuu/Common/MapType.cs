using System.Collections.Generic;
using UnityEngine;

public enum TileTypeId //タイルの種類とそのID
{
    invisible = 0, //透明
    aisle_01,      //反転できる道
    aisle_02,      //反転できない道
    aisle_03,      //氷の道
    wall_01,       //反転できる壁
    wall_02,       //反転できない壁
    wall_03,       //氷の壁
    goal_01,       //ロープのついていないゴール
    goal_02,       //反転したゴール
    goal_03,       //ロープのついているゴール
    wall_99,       //外壁
    statue_01,     //石像１:上
    statue_02,     //石像２:下
    statue_03,     //石像３:右
    statue_04,     //石像４:左
    statue_11,     //壊れた石像１:上
    statue_12,     //壊れた石像２:下
    statue_13,     //壊れた石像３:右
    statue_14,     //壊れた石像４:左
}

public class MapType : MonoBehaviour
{
    [HideInInspector] public TileTypeId  tileType;
}
