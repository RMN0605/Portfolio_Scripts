using UnityEngine;

public class EditorMouse : MonoBehaviour
{
                      private TileTypeId _tileId;
    [HideInInspector] public  bool       isPlayer; //岩を配置するか
    [HideInInspector] public  bool       isRope;   //プレイヤーを配置するか
    [HideInInspector] public  bool       isStone;  //ロープを配置するか
    
    private void Update()
    {
        //メニューは閉じてる？
        if (Input.GetMouseButton(0) && !EditorManager.Instance.isOpenedMenu)
        {
            //Rayの取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D _hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //Rayがオブジェクトに当たってる？ && サンプルオブジェクトは選択されている？ && そのオブジェクトのタグはMapTile？ && 変更したタイルとは違う？
            if (_hit2d && EditorManager.Instance.selectedSampleObject && _hit2d.transform.gameObject.CompareTag("MapTile") && EditorManager.Instance.beforeTile != _hit2d.transform.gameObject)
            {
                SetTileData(_hit2d);
                //変更したタイルを保持 :同じタイルに複数回変更されることを防ぐ
                EditorManager.Instance.beforeTile = _hit2d.transform.gameObject;
            }
        }
    }
    
    /// <summary>
    /// タイル情報の差し替え
    /// </summary>
    /// <param name="_hit2d">変更先のタイル</param>
    private void SetTileData(RaycastHit2D _hit2d)
    {
        EditorMapTile editorMapTile = _hit2d.transform.gameObject.GetComponent<EditorMapTile>();
        EditorSampleTile editorSampleTile = EditorManager.Instance.selectedSampleObject.GetComponent<EditorSampleTile>();
        var tileData = EditorManager.Instance.tileDatas[editorSampleTile.tileId];
        
        //差し替え
        _hit2d.transform.gameObject.GetComponent<SpriteRenderer>().sprite = editorSampleTile.spriteRenderer.sprite;
        editorMapTile.tileId    = tileData.tileId;
        //プレイヤーを配置する？ && タイルはasisle_01かasisle_02かasisle_03？
        editorMapTile.isPlayer  = isPlayer && (editorMapTile.tileId == TileTypeId.aisle_01 || editorMapTile.tileId == TileTypeId.aisle_02 || editorMapTile.tileId == TileTypeId.aisle_03) ? true : false;
        //ロープを配置する？ && タイルはasisle_02？ :ロープがおけるのはasisle_02のみ
        editorMapTile.isRope    = isRope && editorMapTile.tileId == TileTypeId.aisle_02 ? true: false;
        //岩を配置する？ && タイルはasisle_01かasisle_02かasisle_03？
        editorMapTile.isStone   = isStone && (editorMapTile.tileId == TileTypeId.aisle_01 || editorMapTile.tileId == TileTypeId.aisle_02 || editorMapTile.tileId == TileTypeId.aisle_03);
        //岩が配置されている？ : 配置されている場合は通れない
        editorMapTile.isAdvance = !isStone && tileData.isAdvance;
        editorMapTile.isInvert  = tileData.isInvert;
    }
}
