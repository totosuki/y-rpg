// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 指定されたGameObjectの視点にカメラを固定
    /// </summary>
    [CommandInfo("Player", 
                 "SetCanMove",
                 "Set Player.canMove")]
    [AddComponentMenu("")]
    public class SetCanMove : Command 
    {
        [Tooltip("canMove flag")]
        [SerializeField] protected bool canMove = false;

        private GameObject player;
        private PlayerController plc;

        protected virtual void UpdateCanMove()
        {
            plc.canMove = canMove;
        }

        #region Public members

        public virtual void Start() 
        {
            player = GameObject.Find("Player");

            if (player != null) {
                plc = player.GetComponent<PlayerController>();
            } else {
                Continue();
            }
        }

        public override void OnEnter()
        {
            UpdateCanMove();
            Continue();
        }

        public override string GetSummary()
        {
            return "canMove = " + canMove.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}