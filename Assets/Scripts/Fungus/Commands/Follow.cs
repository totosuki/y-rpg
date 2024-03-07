// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus {
    /// <summary>
    /// 指定されたGameObjectの視点にカメラを固定
    /// </summary>
    [CommandInfo("Camera", "Follow", "Follow the specified GameObject")]
    [AddComponentMenu("")]
    public class Follow : Command {
        [Tooltip("Target GameObject we want to follow")]
        [SerializeField] protected GameObject targetObject;

        [Tooltip("Camera that follows the GameObject")]
        [SerializeField] protected GameObject targetCamera;

        private CameraController cameraController;

        protected virtual void FollowTheObject() {
            cameraController.FollowObject(targetObject);
        }

        #region Public members

        public virtual void Start() {
            if (targetCamera != null) {
                cameraController = targetCamera.GetComponent<CameraController>();
            }
            else {
                // スキップ
                Continue();
            }
        }

        public override void OnEnter() {
            if (targetCamera == null || targetObject == null) {
                Continue();
                return;
            }

            FollowTheObject();
            Continue();
        }

        public override string GetSummary() {
            if (targetObject == null) {
                return "Error: No GameObject specified";
            }

            if (targetCamera == null) {
                return "Error: No Camera specified";
            }

            return targetCamera.name + " Follows " + targetObject.name;
        }

        public override Color GetButtonColor() {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}