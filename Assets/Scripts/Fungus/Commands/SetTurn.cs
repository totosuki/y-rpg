// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections.Generic;

namespace Fungus
{
    /// <summary>
    /// Sets a Boolean, Integer, Float or String variable to a new value using a simple arithmetic operation. The value can be a constant or reference another variable of the same type.
    /// </summary>
    [CommandInfo("Turn",
                 "Set Turn",
                 "Sets Turn. anyVar must be 'turn'")]
    [AddComponentMenu("")]
    public class SetTurn : Command, ISerializationCallbackReceiver
    {
        [SerializeField]protected AnyVariableAndDataPair anyVar = new AnyVariableAndDataPair();

        [SerializeField]protected bool isSavePoint = true;

        protected GameManager gameManager;

        private void Start() {
            GameObject gameManagerObject = GameObject.Find("GameManager");
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
               
        protected virtual void DoSetOperation()
        {
            if (anyVar.variable == null)
            {
                return;
            }

            anyVar.SetOp(SetOperator.Assign);
        }

        #region Public members

        public override void OnEnter()
        {
            if (gameManager == null)
            {
                Continue();
            }

            DoSetOperation();

            // == ターン更新時にまとめて行う処理 == //
            // デバッグ用
            print($"-- Turn 5 --");
            int turn = (int)anyVar.data.integerData;

            gameManager.SetTurn(turn);
            gameManager.OnSave();

            // セーブポイントを作成
            if (isSavePoint)
            {
                var saveManager = FungusManager.Instance.SaveManager;
                saveManager.AddSavePoint($"Turn_{turn}", $"Start of Turn {turn}");
            }

            gameManager.InvokeOnTurnUpdate();
            // == == //

            Continue();
        }

        public override string GetSummary()
        {
            if (anyVar.variable == null)
            {
                return "Error: no variable specified";
            }
            string description = $"Set turn to {anyVar.GetDataDescription()}";

            return description;
        }

        public override bool HasReference(Variable variable)
        {
            return anyVar.HasReference(variable);
        }

        public override Color GetButtonColor()
        {
            return new Color32(253, 253, 150, 255);
        }

        #endregion



        #region Editor caches
#if UNITY_EDITOR
        protected override void RefreshVariableCache()
        {
            base.RefreshVariableCache();

            if(anyVar != null)
                anyVar.RefreshVariableCacheHelper(GetFlowchart(), ref referencedVariables);
        }
#endif
        #endregion Editor caches

        #region backwards compat


        [Tooltip("Variable to use in expression")]
        [VariableProperty(AllVariableTypes.VariableAny.Any)]
        protected Variable variable;

        [Tooltip("Integer value to compare against")]
        protected IntegerData integerData;

        public void OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (variable == null)
            {
                return;
            }
            else
            {
                anyVar.variable = variable;
            }

            if (variable.GetType() == typeof(IntegerVariable) && !integerData.Equals(new IntegerData()))
            {
                anyVar.data.integerData = integerData;
                integerData = new IntegerData();
            }

            //now converted to new AnyVar storage, remove the legacy.
            variable = null;
        }
        #endregion
    }
}
