// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;


namespace Fungus
{
    /// <summary>
    /// NPCのmessageを編集
    /// </summary>
    [CommandInfo("Entity", 
                 "Set Position", 
                 "Set Position of Target GameObject")]
    [AddComponentMenu("")]
    public class SetPosition : Command
    {
        public enum TYPE {
            absolute,
            relative,
            basedOnPlayer
        }

        [Tooltip("Target we want to transport")]
        [SerializeField] protected GameObject targetObject;

        [Tooltip("Type of specification")]
        [SerializeField] protected TYPE type;

        [Tooltip("The Position")]
        [SerializeField] protected Vector2 position;

        private GameObject player;


        protected virtual void SetNewPosition()
        {
            targetObject.transform.position = GetNewPosition();
        }

        protected virtual Vector2 GetNewPosition()
        {
            Vector2 pos;

            if (type == TYPE.relative)
            {
                // 自身から見た相対座標を適用
                pos = targetObject.transform.position + new Vector3(position.x,position.y);

            }
            else if (type == TYPE.basedOnPlayer)
            {
                // プレイヤーに対しての相対座標を適用
                pos = player.transform.position + new Vector3(position.x,position.y);

            }
            else
            {
                // 絶対座標を適用
                pos = position;
            }
            
            return pos;
        }

        #region Public members

        public virtual void Start()
        {
            if (type == TYPE.basedOnPlayer)
            {
                player = GameObject.Find("Player");
            }
        }

        public override void OnEnter()
        {
            if (targetObject == null ||
                position == Vector2.zero)
            {
                Continue();
                return;
            }

            SetNewPosition();
            Continue();
        }

        public override string GetSummary()
        {
            if (targetObject == null)
            {
                return "Error: No target GameObject specified";
            }

            return "Set " + targetObject.name + " Position to " + position.ToString();
        }
        
        public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

        #endregion
    }
}