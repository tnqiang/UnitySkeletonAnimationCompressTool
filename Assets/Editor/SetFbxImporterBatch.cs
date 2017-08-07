using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SetFbxImporterBatch : EditorWindow {

    public enum AnimationCompressType
    {
        NONE,
        KEY_FRAME_REDUCTION,
        OPTIMAL
    }

    private float m_rotationErrorThreshold;
    private float m_positionErrorThreshold;
    private float m_scaleErrorThreshold;
    private AnimationCompressType m_compressType;

    [MenuItem("Tools/SetFbxImporterBatch")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SetFbxImporterBatch window = (SetFbxImporterBatch)EditorWindow.GetWindow(typeof(SetFbxImporterBatch));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        m_compressType = (AnimationCompressType)EditorGUILayout.EnumPopup(m_compressType);

        if (m_compressType != AnimationCompressType.NONE)
        {
            m_rotationErrorThreshold = EditorGUILayout.FloatField("Rotation Error: ", m_rotationErrorThreshold);
            m_positionErrorThreshold = EditorGUILayout.FloatField("Position Error: ", m_positionErrorThreshold);
            m_scaleErrorThreshold = EditorGUILayout.FloatField("Scale Error: ", m_scaleErrorThreshold);
        }

        if (GUILayout.Button("Apply"))
        {
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            for (int i = 0; i < allAssetPaths.Length; ++i)
            {
                ModelImporter importer = ModelImporter.GetAtPath(allAssetPaths[i]) as ModelImporter;
                if (importer != null)
                {
                    FormatModel(importer, m_compressType, m_rotationErrorThreshold, m_positionErrorThreshold, m_scaleErrorThreshold);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(m_compressType);
            Debug.Log(m_rotationErrorThreshold);
            Debug.Log(m_positionErrorThreshold);
            Debug.Log(m_scaleErrorThreshold);
        }
        EditorGUILayout.EndHorizontal();
    }

    void FormatModel(ModelImporter importer, AnimationCompressType type, float rotationErrorThreshold, float positionThreshold, float scaleThreshold)
    {
        if (type == AnimationCompressType.NONE)
        {
            importer.animationCompression = ModelImporterAnimationCompression.Off;
        }
        else if (type == AnimationCompressType.KEY_FRAME_REDUCTION)
        {
            importer.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
            importer.animationRotationError = rotationErrorThreshold;
            importer.animationPositionError = positionThreshold;
            importer.animationScaleError = scaleThreshold;
        }
        else
        {
            importer.animationCompression = ModelImporterAnimationCompression.Optimal;
            importer.animationRotationError = rotationErrorThreshold;
            importer.animationPositionError = positionThreshold;
            importer.animationScaleError = scaleThreshold;
        }
        importer.SaveAndReimport();
    }
}
