using UnityEngine;
using TMPro;
using Fungus;

/// <summary>
/// InputField・GetInputコマンドなどの補助を行う
/// </summary>
public class InputManager : MonoBehaviour {
    // fungus
    [SerializeField]
    private Flowchart flowchart;

    private GameObject input;
    private TMP_InputField _inputField;
    private TMP_Text placeholder;

    private string varKey;
    private string message;

    void Start() {
        input = gameObject;
        _inputField = input.GetComponent<TMP_InputField>();
        placeholder = _inputField.GetComponentInChildren<TMP_Text>();
        
        InitInput();
        input.SetActive(false);
    }

    void InitInput() {
        _inputField.text = "";
    }

    public void StartInput() {
        InitInput();
        // 表示
        input.SetActive(true);
    }

    public void EndInput() {
        // 非表示
        input.SetActive(false);
        
        if (varKey == null) {
            return;
        }

        string value = _inputField.text;
        flowchart.SetStringVariable(varKey, value);

        // メッセージで終了を通知
        flowchart.SendFungusMessage(message);
    }

    public void SetPlaceholder(string text) {
        placeholder.text = text;
    }

    public void SetVarKey(string key) {
        varKey = key;
    }

    public void SetMessage(string msg) {
        message = msg;
    }
}
