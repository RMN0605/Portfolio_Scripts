using UnityEngine;

public class GoBackTitle : MonoBehaviour
{
    public GameObject PopUpObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopUpObject.SetActive(!PopUpObject.activeSelf);
            if(PopUpObject.activeSelf)
                Time.timeScale = 0;
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
