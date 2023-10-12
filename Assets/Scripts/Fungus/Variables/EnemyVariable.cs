using UnityEngine;

namespace Fungus
{
	[VariableInfo("Other", nameof(Enemy))]
	[AddComponentMenu("")]
	[System.Serializable]
	public class EnemyVariable : VariableBase<Enemy> { }

	[System.Serializable]
	public struct EnemyData
	{
		[SerializeField]
		[VariableProperty("<Value>", typeof(EnemyVariable))]
		public EnemyVariable enemyRef;

		[SerializeField]
		public Enemy enemyVal;

		public static implicit operator Enemy(EnemyData enemyData)
		{
			return enemyData.Value;
		}

		public EnemyData(Enemy v)
		{
			enemyVal = v;
			enemyRef = null;
		}

		public Enemy Value
		{
			get { return (enemyRef == null) ? enemyVal : enemyRef.Value; }
			set { if (enemyRef == null) { enemyVal = value; } else { enemyRef.Value = value; } }
		}

		public string GetDescription()
		{
			if (enemyRef == null)
			{
				return enemyVal != null ? enemyVal.ToString() : "Null";
			}
			else
			{
				return enemyRef.Key;
			}
		}
	}
}