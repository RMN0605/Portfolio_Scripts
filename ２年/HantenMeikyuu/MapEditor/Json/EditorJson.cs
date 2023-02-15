using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EditorJson : MonoBehaviour
{
                      private MapData _mapData      = new MapData();
    [SerializeField]  private Button  _dataSave     = null; //データをセーブするボタン
    [SerializeField]  private Button  _dataLoad     = null; //データをロードするボタン
                      
    [HideInInspector] public  bool    overWriteSave = false; //保存時に上書きしてもいいか
    [HideInInspector] public  string  fileName      = "";    //保存、読み込みするファイルの名前
                      private string  _filePath     = "";    //保存されているファイルのパス
                      

    private void Awake()
    {
        Debug.Assert(null != _dataSave, "_dataOutPut ボタンが設定されていません");
        Debug.Assert(null != _dataLoad, "_LoadData ボタンが設定されていません");
        
        //ボタンにイベントを登録
        _dataSave.onClick.AddListener(OnClickSave);
        _dataLoad.onClick.AddListener(OnClickLoad);
    }

    #region セーブ関係
    
        /// <summary> 
        /// データをセーブ
        /// </summary>
        private void OnClickSave()
        {
            //ちゃんとステージ名は入力されてる？
            if (fileName == "")
            {
                Debug.Log($"<color=Red>ステージ名を入力してから押してね！</color>");
                return;
            }
            
            _filePath = Path.Combine(Application.dataPath, "Resources/MapData/" + fileName + ".json");
            
            #region ファイル名の被り確認
            
                //ファイル一覧から名前を全て取得
                string[] files = Directory.GetFiles("Assets/Resources/MapData/", "*.json", SearchOption.AllDirectories);
                //名前の確認
                foreach(var fileName in files)
                {
                    //名前の被りはある？
                    if (fileName != "Assets/Resources/MapData/" + fileName + ".json") continue;
                    
                    //上書きしてもいい？
                    if (overWriteSave)
                    {
                        Debug.Log("同じ名前のデータが存在しているが、上書き保存を選択しているため続行");
                        break;
                    }
                    else
                    {
                        Debug.Log("同じ名前のデータが存在しており、上書き保存しないを選択したため終了");
                        return;
                    }
                }
            
            #endregion

            SaveTileData();
            //まとめた情報をJsonに変換
            var Json = JsonUtility.ToJson(_mapData, false);
            //Jsonを保存
            File.WriteAllText(_filePath, Json);
            Debug.Log($"<color=blue>{_filePath} に保存したよ</color>");
        }
        
        /// <summary> 
        /// TileDataをJsonにする
        /// </summary>
        private void SaveTileData()
        {
            foreach (var map in _mapData.tileChips.Select((mapChip, index) => new { mapChip, index }))
            {
                var tileData = EditorManager.Instance.mapTiles[map.index].GetComponent<EditorMapTile>();
                
                map.mapChip.tileId = tileData.tileId;
                map.mapChip.isAdvance = tileData.isAdvance;
                map.mapChip.isInvert = tileData.isInvert;
                map.mapChip.isPlayer = tileData.isPlayer;
                map.mapChip.isRope = tileData.isRope;
                map.mapChip.isStone = tileData.isStone;
            }
        }
        
    #endregion

    #region ロード関係

        /// <summary> 
        /// データをロード
        /// </summary>
        private void OnClickLoad()
        {
            //ちゃんとステージ名は入力されてる？
            if (fileName == "")
            {
                Debug.Log($"<color=Red>ステージ名を入力してから押してね！</color>");
                return;
            }
            
            _filePath = Path.Combine(Application.dataPath, "Resources/MapData/" + fileName + ".json");
            //ファイルパスに指定した名前のデータはある？
            if (!File.Exists(_filePath))    
            {
                Debug.LogError($"<color=yellow>{_filePath} にJSONがないよ</color>");
                return;
            }
            
            // 指定したデータをJsonに戻す
            var json = File.ReadAllText(_filePath);
            //Jsonからデータを取り出す
            _mapData = JsonUtility.FromJson<MapData>(json);
            LoadTileData();
            Debug.Log($"<color=blue>{_filePath} をロードしたよ</color>");
        }

        /// <summary> 
        /// JsonからTileDataに戻す
        /// </summary>
        private void LoadTileData()
        {
            RestTiles();
            //タイルデータを読み込んだJsonのデータに書き換える
            foreach (var map in _mapData.tileChips.Select((mapChip, index) => new { mapChip, index }))
            {
                var tileData = EditorManager.Instance.mapTiles[map.index].GetComponent<EditorMapTile>();
                //Idからタイルの画像の名前を取得
                string spriteName = ((TileTypeId)map.mapChip.tileId).ToString();
                //Texturesにある取得した名前のスプライトを取得
                Sprite tileSprite = Resources.Load<Sprite>("Textures/" + spriteName) as Sprite;
                
                //差し替え
                tileData.spriteRenderer.sprite = tileSprite;
                tileData.tileId = map.mapChip.tileId;
                tileData.isAdvance = map.mapChip.isAdvance;
                tileData.isInvert = map.mapChip.isInvert;
                tileData.isPlayer = map.mapChip.isPlayer;
                tileData.isRope = map.mapChip.isRope;
                tileData.isStone = map.mapChip.isStone;
            }
        }

    #endregion

    /// <summary>
    /// TileDataを初期化
    /// </summary>
    private void RestTiles()
    {
        foreach (var tile in EditorManager.Instance.mapTiles)
        {
            var editorMapTile = tile.GetComponent<EditorMapTile>();
            editorMapTile.isAdvance = editorMapTile.isInvert = editorMapTile.isPlayer 
                                    = editorMapTile.isRope = editorMapTile.isStone = false;
        }
    }
    
    /// <summary>
    /// スクリプトが破棄されたときに登録したイベントを削除する
    /// </summary>
    private void OnDestroy()
    {
        _dataSave.onClick.RemoveAllListeners();
        _dataLoad.onClick.RemoveAllListeners();
    }
}
