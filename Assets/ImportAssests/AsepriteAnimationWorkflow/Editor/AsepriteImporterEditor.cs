using UnityEngine;
using UnityEditor;


namespace APIShift.AsepriteAnimationWorkflow
{
  [CustomEditor(typeof(AsepriteImporter)), CanEditMultipleObjects]
  public class AsepriteImporterEditor : UnityEditor.AssetImporters.ScriptedImporterEditor
  {
    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.Space();
      EditorGUI.BeginDisabledGroup(true);
      PropField(nameof(AsepriteImporterSettings.TextureType));
      PropField(nameof(AsepriteImporterSettings.TextureShape));
      EditorGUILayout.Space();
      PropField(nameof(AsepriteImporterSettings.SpriteMode));
      EditorGUI.EndDisabledGroup();
      ++EditorGUI.indentLevel;

      PropField(nameof(AsepriteImporterSettings.PixelsPerUnit));
      PropField(nameof(AsepriteImporterSettings.MeshType));
      PropField(nameof(AsepriteImporterSettings.ExtrudeEdges));
      PropField(nameof(AsepriteImporterSettings.GeneratePhysicsShape));
      EditorGUILayout.Space();
      EditorGUILayout.Space();

      --EditorGUI.indentLevel;
      PropField(nameof(AsepriteImporterSettings.WrapMode));
      PropField(nameof(AsepriteImporterSettings.FilterMode));

      EditorGUI.BeginDisabledGroup(true);
      PropField(nameof(AsepriteImporterSettings.AnisoLevel));
      EditorGUI.EndDisabledGroup();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Frame settings", EditorStyles.label);
      ++EditorGUI.indentLevel;
      PropField(nameof(AsepriteImporterSettings.InnerPadding));
      PivotPropField();

      serializedObject.ApplyModifiedProperties();
      base.ApplyRevertGUI();
    }

    private void PivotPropField()
    {
      var prop = GetProp(nameof(AsepriteImporterSettings.Pivot));
      EditorGUILayout.PropertyField(prop);
      if (((SpriteAlignment)prop.enumValueIndex) == SpriteAlignment.Custom)
      {
        NoLabelPropField("_pivot");
      }
    }

    private void NoLabelPropField(string propName)
      => EditorGUILayout.PropertyField(GetProp(propName), new GUIContent("  "));

    private void PropField(string propName)
      => EditorGUILayout.PropertyField(GetProp(propName));

    private SerializedProperty GetProp(string propName)
      => serializedObject.FindProperty($"{nameof(AsepriteImporter.Settings)}.{propName}");
  }
}
