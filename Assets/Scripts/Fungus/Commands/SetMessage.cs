// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;


namespace Fungus {
    /// <summary>
    /// NPCのmessageを編集
    /// </summary>
    [CommandInfo("NPC", "Set Message", "Set NPC Message")]
    [AddComponentMenu("")]
    public class SetMessage : Command {
        [Tooltip("Target monobehavior which contains the method we want to call")]
        [SerializeField] protected GameObject targetObject;

        [Tooltip("Name of the method to call")]
        [SerializeField] protected string newMessage = "";

        [Tooltip("Delay (in seconds) before the method will be called")]
        [SerializeField] protected float delay;

        // !Fungus
        private NPCController npcController;

        protected virtual void SetTheMessage() {
            npcController.message = newMessage;
        }

        #region Public members

        public override void OnEnter() {
            npcController = targetObject.GetComponent<NPCController>();

            if (targetObject == null || npcController == null || newMessage.Length == 0) {
                Continue();
                return;
            }

            if (Mathf.Approximately(delay, 0f)) {
                SetTheMessage();
            }
            else {
                Invoke("SetTheMessage", delay);
            }

            Continue();
        }

        public override string GetSummary() {
            if (targetObject == null) {
                return "Error: No target GameObject specified";
            }

            if (npcController == null) {
                return "Error: Target GameObject is not NPC";
            }

            if (newMessage.Length == 0) {
                return "Error: No message specified";
            }

            return targetObject.name + " : " + newMessage;
        }
        
        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}