using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
                      public static EditorManager Instance             = null;  //どこからでも参照できるようにする
                      
                      public        GameObject[]  mapTiles;
                      public        TileData[]    tileDatas;
    [HideInInspector] public        GameObject    selectedSampleObject = null;  //選択されたサンプルオブジェクト
    [HideInInspector] public        bool          isOpenedMenu         = false; //メニューが開かれているか
    [HideInInspector] public        GameObject    beforeTile;                   //ひとつ前に選択したタイル
    [HideInInspector] public        EditorMapTile player;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [System.Serializable]
    public class TileData
    {
        [Header("タイルの名前")] private string     _tileName;
        [Header("タイルID")]     public TileTypeId  tileId;
        [Header("前進できるか")] public bool        isAdvance;
        [Header("反転できるか")] public bool        isInvert;
    }
}
