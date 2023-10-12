using UnityEditor;

namespace Fungus.EditorUtils
{
	[CustomPropertyDrawer(typeof(EnemyData))]
	public class EnemyDataDrawer : VariableDataDrawer<EnemyVariable> { }
}