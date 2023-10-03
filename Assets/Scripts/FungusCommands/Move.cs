// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections.Generic;


namespace Fungus
{
    /// <summary>
    /// NPCのmessageを編集
    /// </summary>
    [CommandInfo("Entity", 
                 "Move", 
                 "Move target object to spesific position")]
    [AddComponentMenu("")]
    public class Move : Command
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

        [Tooltip("How long does it take to move")]
        [SerializeField] protected float duration;

        private NPCAnimController npcAnimController;
        private GameObject player;

        private Vector3 startPosition;
        private Vector3 endPosition;
        private float startTime;
        private bool onEnable;


        protected virtual Vector3 GetNewPosition()
        {
            Vector3 pos;

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

        int GetFacingIdByVector2(Vector2 vector)
        {
            vector = SimplifyVector2(vector);

            Dictionary<Vector2, int> idList = new Dictionary<Vector2, int>() {
                {new Vector2(0, -1), 0},
                {new Vector2(-1, 0), 1},
                {new Vector2(1, 0), 2},
                {new Vector2(0, 1), 3}
            };

            return idList[vector];
        }

        private Vector2 SimplifyVector2(Vector2 pos)
        {
            if (pos.x != 0)
            {
                pos.x = Mathf.Sign(pos.x);
            }
            
            if (pos.y != 0)
            {
                pos.y = Mathf.Sign(pos.y);
            }
            
            return pos;
        }

        #region Public members

        public virtual void Start()
        {
            npcAnimController = targetObject.GetComponent<NPCAnimController>();

            if (npcAnimController == null)
            {
                Continue();
                return;
            }

            if (type == TYPE.basedOnPlayer)
            {
                player = GameObject.Find("Player");
            }
        }

        public virtual void Update()
        {
            if (!onEnable) return;
            float diff = Time.timeSinceLevelLoad - startTime;
            float rate = diff / duration;

            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, rate);

            if (rate >= 1.0f)
            {
                Continue();
                onEnable = false;
                npcAnimController.StopWalk();
            }
        }

        public override void OnEnter()
        {
            startTime = Time.timeSinceLevelLoad;

            startPosition = targetObject.transform.position;
            endPosition = GetNewPosition();

            onEnable = true;
            
            Vector2 relatedPosition = endPosition - startPosition;
            npcAnimController.StartWalk(GetFacingIdByVector2(relatedPosition));
            
            if (targetObject == null ||
                npcAnimController == null ||
                position == Vector2.zero)
            {
                Continue();
                return;
            }
        }

        public override string GetSummary()
        {
            if (targetObject == null)
            {
                return "Error: No target GameObject specified";
            }

            // 斜め移動は禁止
            if (position.x != 0.0f && position.y != 0.0f)
            {
                return "Error: x or y should be 0";
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