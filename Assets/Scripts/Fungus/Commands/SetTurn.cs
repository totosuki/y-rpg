// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus {
    /// <summary>
    /// ターンを設定
    /// </summary>
    [CommandInfo("Turn", "Set Turn", "Set turn")]
    [AddComponentMenu("")]
    public class SetTurn : Command {
        [Tooltip("Turn")]
        [SerializeField] protected int turn;

        private GameObject obj;
        private TurnManager turnManager;
        
        #region Public members

        void Start()
        {
            obj = GameObject.Find("TurnManager");
            turnManager = obj.GetComponent<TurnManager>();
        }

        public override void OnEnter() {
            turnManager.SetTurnTo(turn);
            Continue();
        }

        public override string GetSummary() {
            return $"Set Turn to {turn}";
        }

        public override Color GetButtonColor() {
            return new Color32(253, 253, 150, 255);
        }

        #endregion
    }
}