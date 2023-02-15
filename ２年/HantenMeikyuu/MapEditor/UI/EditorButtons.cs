using UnityEngine;

public class EditorButtons : MonoBehaviour
{
    private EditorJson  _editorJson;
    private EditorMouse _editorMouse;
    
    private bool        _isOverwriteSaveButton = false;　//上書きボタン
    private bool        _isPlayerButton = false; 　　　　//岩を配置するボタン
    private bool        _isRopeButton = false;   　　　　//プレイヤーを配置するボタン
    private bool        _isStoneButton = false;  　　　　//ロープを配置するボタン

    private void Awake()
    {
        _editorJson = GetComponent<EditorJson>();
        _editorMouse = GetComponent<EditorMouse>();
    }

    /// <summary>
    /// 選択されているSampleTileを解除するボタン
    /// </summary>
    public void CanselButton()
    {
        //選択状態になってる？
        if (EditorManager.Instance.selectedSampleObject != null)
        {
            //選択されているタイルの選択表示をOff
            EditorManager.Instance.selectedSampleObject.GetComponent<EditorSampleTile>().childObject.SetActive(false);
            //選択を解除
            ResetSelectTile();
        }
    }

    /// <summary>
    /// タイル消しゴムボタン
    /// </summary>
    public void EraserButton()
    {
        //一度選択を解除する(ひとつ前のタイルの情報も)
        CanselButton();
        //消しゴム探す
        var eraser = GameObject.Find("Eraser");
        //セット
        EditorManager.Instance.selectedSampleObject = eraser;
    }

    /// <summary>
    /// 上書きセーブボタン
    /// </summary>
    public void OverwriteSaveButton()
    {
        //Boolを入れ替える
        _isOverwriteSaveButton = !_isOverwriteSaveButton;
        _editorJson.overWriteSave = _isOverwriteSaveButton;
    }

    /// <summary>
    /// 岩を配置するボタン
    /// </summary>
    public void StoneButton()
    {
        //Boolを入れ替える
        _isStoneButton = !_isStoneButton;
        _editorMouse.isStone = _isStoneButton;
        ResetSelectTile(true);
    }

    /// <summary>
    /// プレイヤーを配置するボタン
    /// </summary>
    public void PlayerButton()
    {
        //Boolを入れ替える
        _isPlayerButton = !_isPlayerButton;
        _editorMouse.isPlayer = _isPlayerButton;
        ResetSelectTile(true);
    }

    /// <summary>
    /// ロープを配置するボタン
    /// </summary>
    public void RopeButton()
    {
        //Boolを入れ替える
        _isRopeButton = !_isRopeButton;
        _editorMouse.isRope = _isRopeButton;
        ResetSelectTile(true);
    }

    /// <summary>
    /// 選択解除
    /// </summary>
    private void ResetSelectTile(bool isCheckBoxButton = false)
    {
        //チェックボックスボタンじゃない？
        if (!isCheckBoxButton)
        {
            EditorManager.Instance.selectedSampleObject = null;
        }
        if(EditorManager.Instance.beforeTile != null) 
            EditorManager.Instance.beforeTile = null;
    }
}
