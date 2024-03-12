using UnityEngine;
using TMPro;
using Fungus;
using System.Collections;
using System;

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

    private string variableKey;

    private bool isInputFinished;

    void Start() {
        input = gameObject;
        _inputField = input.GetComponent<TMP_InputField>();
        placeholder = _inputField.GetComponentInChildren<TMP_Text>();
        
        InitInput();
        input.SetActive(false);
    }

    void InitInput() {
        _inputField.text = "";
        isInputFinished = false;
    }

    public IEnumerator StartInput(Action callback) {
        // 入力を開始
        InitInput();
        input.SetActive(true);

        // 入力が終わったらコールバック
        yield return new WaitUntil(() => isInputFinished);
        callback();
    }

    public void EndInput() {
        // Inputを非表示
        input.SetActive(false);
        
        if (variableKey != null) {
            string value = _inputField.text;
            flowchart.SetStringVariable(variableKey, value);
        }
        
        isInputFinished = true;
    }

    public void SetPlaceholder(string text) {
        placeholder.text = text;
    }

    public void SetVariableKey(string key) {
        variableKey = key;
    }
}
