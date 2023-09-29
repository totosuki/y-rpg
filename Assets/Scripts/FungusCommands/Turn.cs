// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    public enum Directions
    {
        right,
        left,
        up,
        down
    };

    /// <summary>
    /// 指定されたGameObjectの視点にカメラを固定
    /// </summary>
    [CommandInfo("Player", 
                 "Turn",
                 "Turn Player in the specified direction")]
    [AddComponentMenu("")]
    public class Turn : Command 
    {
        [Tooltip("The direction")]
        [SerializeField] Directions direction;

        [Tooltip("Player")]
        [SerializeField] protected GameObject player;

        private PlayerController playerController;

        protected virtual void TurnPlayer()
        {
            playerController.Turn(direction.ToString());
        }

        #region Public members

        public virtual void Start() 
        {
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }
            else
            {
                // スキップ
                Continue();
            }
        }

        public override void OnEnter()
        {
            if (player == null)
            {
                Continue();
                return;
            }

            TurnPlayer();
            
            Continue();
        }

        public override string GetSummary()
        {
            if (player == null)
            {
                return "Error: No Player specified";
            }

            return " Turn " + player.name + " in " + direction.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}