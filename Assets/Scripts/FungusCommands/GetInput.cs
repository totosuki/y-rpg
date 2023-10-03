// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using TMPro;

namespace Fungus
{
    /// <summary>
    /// InputFieldから文字列を取得
    /// </summary>
    [CommandInfo("Scripting", 
                 "Get Input",
                 "Get an input in string and save it to specified variable")]
    [AddComponentMenu("")]
    public class GetInput : Command
    {
        [Tooltip("Variable whose input is to be saved")]
        [VariableProperty(typeof(StringVariable))]
        [SerializeField] protected StringVariable variable;

        [Tooltip("Placeholder on inputfield")]
        [SerializeField] protected string placeholder;

        [Tooltip("Send on end")]
        [SerializeField] protected string endMessage;
        
        private GameObject input;
        private TMP_InputField _inputField;
        private InputManager inputManager;

        protected virtual void SetVariable(string value)
        {
            variable.Value = value;
        }

        #region Public members

        public virtual void Start()
        {
            input = GameObject.Find("InputField");

            if (input == null)
            {
                Continue();
            }

            _inputField = input.GetComponent<TMP_InputField>();
            if (_inputField == null)
            {
                Continue();
            }

            inputManager = input.GetComponent<InputManager>();
            if (inputManager == null)
            {
                Continue();
            }
        }

        public override void OnEnter()
        {
            StartInput();
        }

        public void EndInput()
        {
            SetVariable(_inputField.text);
            Continue();
        }

        public override string GetSummary()
        {
            if (variable == null)
            {
                return "Error: Variable not selected";
            }

            return "Get input and save to " + variable.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        private void StartInput()
        {
            inputManager.SetPlaceholder(placeholder);
            inputManager.SetVarKey(variable.Key);
            inputManager.SetMessage(endMessage);
            inputManager.StartInput();
        }

        #endregion
    }
}