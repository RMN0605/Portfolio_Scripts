using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(StageSetting))]
public class GeneralManager : MonoBehaviour
{
                      public static GeneralManager Instance = null;
    [HideInInspector] public        SoundManager   soundManager;
    [HideInInspector] public        StageSetting stageSetting;
    [HideInInspector] public        MapType        mapType;
    
                      public int    selectStageNum;                 //ステージ番号 :この番号に対応したステージを遊ぶ
                      public bool   isPlay;                         //行動してもよいか
                      
    private void Awake()
    {
        //FPSを60に固定
        Application.targetFrameRate =60;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
        
        soundManager = GetComponent<SoundManager>();
        stageSetting = GetComponent<StageSetting>();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        //ゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MaxcoffeeScene")
        {
            SceneManager.LoadScene("MaxcoffeeScene");
        }

    }

    public void OnDestroyThisObject()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}
