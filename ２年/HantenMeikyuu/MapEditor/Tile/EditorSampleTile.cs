using UnityEngine;

public class EditorSampleTile : MonoBehaviour
{
    [HideInInspector] public  GameObject     childObject    = null;
    [HideInInspector] public  SpriteRenderer spriteRenderer; 
    
                      public  int            tileId;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        childObject = transform.Find("SelectedObject").gameObject;
    }

    /// <summary>
    /// SampleTileが選択されたらEditorManagerに格納する
    /// </summary>
    public void OnClickSampleTile()
    {
        //メニューは閉じてる？ && 選択されてるタイルは自身とは違うタイル？
        if (!EditorManager.Instance.isOpenedMenu && EditorManager.Instance.selectedSampleObject != this)
        {
            //選択されているタイルはある？
            if (EditorManager.Instance.selectedSampleObject)
            {
                //選択されているタイルの選択表示をOff
                EditorManager.Instance.selectedSampleObject.GetComponent<EditorSampleTile>().childObject.SetActive(false);
            }
            //選択表示（SelectedObject）をOn
            childObject.SetActive(true);
            //EditorManagerに格納
            EditorManager.Instance.selectedSampleObject = gameObject;
            
            //変更したタイルはある？
            if (EditorManager.Instance.beforeTile != null)
                EditorManager.Instance.beforeTile = null;
        }
    }
}
