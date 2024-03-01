// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System;
using UnityEngine;

namespace Fungus {
    /// <summary>
    /// MainUI（MainUICanvasとSaveMenu）の表示非表示を制御
    /// </summary>
    [CommandInfo("UI", "Show MainUI", "Show MainUI")]
    [AddComponentMenu("")]
    public class ShowMainUI : Command {
        [Tooltip("true = Show, false = Hide")]
        [SerializeField] protected bool show;

        private GameObject mainUICanvas;
        private GameObject saveMenu;

        #region Public members

        public virtual void Start() {
            mainUICanvas = GameObject.Find("MainUICanvas");
            saveMenu = GameObject.Find("SaveMenu");

            if (mainUICanvas == null || saveMenu == null) {
                Continue();
                return;
            }
        }

        public override void OnEnter() {
            if (mainUICanvas != null && saveMenu != null) {
                mainUICanvas.SetActive(show);
                saveMenu.SetActive(show);
            }

            Continue();
        }

        public override string GetSummary() {
            return "MainUI.SetActive(" + show + ")";
        }

        public override Color GetButtonColor() {
            return new Color32(253, 253, 150, 255);
        }

        #endregion
    }
}