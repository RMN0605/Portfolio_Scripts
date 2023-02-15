using TMPro;
using UnityEngine;

public class InputText : MonoBehaviour
{
    public TMP_InputField _field;
    public EditorJson     _editorJson;

    /// <summary>
    /// 入力した名前をEditorManagerのfileNameに送る
    /// </summary>
    public void OnEndEdit()
    {
        string input = _field.GetComponent<TMP_InputField>().text;
        _editorJson.fileName = input;
    }
}
