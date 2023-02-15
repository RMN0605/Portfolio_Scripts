using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StageObjectData
{
    [Serializable]
    public class ObjectChip
    {
        public Vector2Int objectPos;
        public Vector3 pos;
        //初期化
        public ObjectChip()
        {
            objectPos = new Vector2Int();
            pos = Vector3.zero;
        }
    }
    
    public List<ObjectChip> objectChips = new List<ObjectChip>();
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public StageObjectData()
    {
        //全てのタイル情報をTileChipsに書き込む
        foreach (var num in Enumerable.Range(0, 10))
        {
            objectChips.Add(new StageObjectData.ObjectChip());
        }
    }
}
