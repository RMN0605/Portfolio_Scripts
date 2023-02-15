using UnityEngine;

public class TrainingMenu : MonoBehaviour
{
    //�@���j���[���J���{�^��
    [SerializeField]
    private GameObject menuOpenButton;
    //�@�Q�[���ĊJ�{�^��
    [SerializeField]
    private GameObject menuCloseButton;
    //�@���j���[�p�l��
    [SerializeField]
    private GameObject menuPanel;

    private bool isStopScene = false;
    //private void Update()
    //{
    //    if(isStopScene && Time.timeScale == 1)
    //    {
    //        Time.timeScale = 0f;
    //    }
    //}

    public void StopGame()
    {
        Time.timeScale = 0f;
        isStopScene = true;
        menuOpenButton.SetActive(false);
        menuCloseButton.SetActive(true);
        menuPanel.SetActive(true);
    }

    public void ReStartGame()
    {
        isStopScene = false;
        menuOpenButton.SetActive(true);
        menuCloseButton.SetActive(false);
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
