using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public  Image    ropeUI;
    [HideInInspector] public  HantenUI hantensUI;
    
    private void Awake()
    {
        ropeUI = GameObject.Find("RopeImage").GetComponent<Image>();
        hantensUI = GameObject.Find("HantenUI").GetComponent<HantenUI>();
    }

    /// <summary>
    /// ロープを所持している場合はalpha値を255に所持していない場合は50にする
    /// </summary>
    public void ChangeRopeUI()
    {
        ropeUI.color = StageManager.Instance.isHaveRope ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
    }
}
