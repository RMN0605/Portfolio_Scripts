using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamesBackButton : MonoBehaviour
{
    public void ReturnHanten()
    {
        SceneManager.LoadScene("GameSelectScene");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        if(GameManager.Instance != null)
            GameManager.Instance.IsOperation = true;
        if(GeneralManager.Instance != null)
            GeneralManager.Instance.OnDestroyThisObject();
    }

    public void ReturnTunakaze()
    {
        SceneManager.LoadScene("GameSelectScene");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        if(GameManager.Instance != null)
            GameManager.Instance.IsOperation = true;
        
        if(GameObject.Find("NCMBSettings") != null)
            SceneManager.MoveGameObjectToScene(GameObject.Find("NCMBSettings"), SceneManager.GetActiveScene());
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
    
    public void NotReturn()
    {
        if (SceneManager.GetActiveScene().name == "GameSelectScene")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        gameObject.SetActive(false);
    }
}
