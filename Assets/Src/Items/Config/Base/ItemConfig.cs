using UnityEditor;
using UnityEngine;

namespace Src.Items.Config.Base
{
    [CreateAssetMenu(menuName = "ItemConfig/" + nameof(ItemConfig), fileName = nameof(ItemConfig))]
    public abstract class ItemConfig : ScriptableObject
    {
        public Sprite sprite;
        public Color color;
        [HideInInspector] public bool canBeStacked = true;
        [HideInInspector] public float weight;
        [HideInInspector] public int stackSize;
    }

    [CustomEditor(typeof(ItemConfig), true)]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (ItemConfig)target;
            script.weight = EditorGUILayout.FloatField("Weight", script.weight);
            script.canBeStacked = EditorGUILayout.Toggle("Can be stacked", script.canBeStacked);

            if (script.canBeStacked == false)
                return;

            script.stackSize = EditorGUILayout.IntField("Stack Size", script.stackSize);
        }
    }
}