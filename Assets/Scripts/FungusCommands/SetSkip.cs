// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// スキップボタンを設置
    /// </summary>
    [CommandInfo("UI", 
                 "Set Skip",
                 "Set skip button on the screen")]
    [AddComponentMenu("")]
    public class SetSkip : Command 
    {
        [Tooltip("Destination block")]
        [SerializeField] protected string nextBlockName;
        
        private GameObject button;
        private SkipButtonManager buttonManager;

        #region Public members

        public virtual void Awake()
        {
            button = GameObject.Find("SkipButton");
        }

        public virtual void Start() 
        {
            if (button == null)
            {
                Continue();
                return;
            }

            buttonManager = button.GetComponent<SkipButtonManager>();

            if (buttonManager == null)
            {
                Continue();
                return;
            }
        }

        public override void OnEnter()
        {
            if (button != null)
            {
                buttonManager.SetNextBlock(nextBlockName);
                buttonManager.Show();
            }

            Continue();

            // SkipButton側でOnClickが発動してSkipが発動する
        }

        public override string GetSummary()
        {
            if (nextBlockName == "")
            {
                return "Error: Next block does not specified";
            }

            return "Set Skip Button -> " + nextBlockName;
        }

        public override Color GetButtonColor()
        {
            return new Color32(253, 253, 150, 255);
        }

        #endregion
    }
}