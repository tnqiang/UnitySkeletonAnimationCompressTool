using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling;
using System.Reflection;
using System;

public class GetAnimClipSize 
{
    [MenuItem("Test/GetAllAnimSize")]
    public static void GetAnimSize()
    {
        Assembly asm = Assembly.GetAssembly(typeof(Editor));
        MethodInfo getAnimationClipStats = typeof(AnimationUtility).GetMethod("GetAnimationClipStats", BindingFlags.Static | BindingFlags.NonPublic);
        Type aniclipstats = asm.GetType("UnityEditor.AnimationClipStats");
        FieldInfo sizeInfo = aniclipstats.GetField("size", BindingFlags.Public | BindingFlags.Instance);
        int allSize = 0;

        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        for (int k = 0; k < allAssetPaths.Length; ++k)
        {
            if (allAssetPaths[k].ToLower().EndsWith(".fbx"))
            {
                UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(allAssetPaths[k]);
                for (int i = 0; i < objs.Length; ++i)
                {
                    if (objs[i] is AnimationClip)
                    {
                        AnimationClip aniClip = objs[i] as AnimationClip;
                        //Debug.Log(Profiler.GetRuntimeMemorySize(aniClip));//MemorySize

                        if (objs[i].name.StartsWith("__preview__Take"))
                        {
                            var stats = getAnimationClipStats.Invoke(null, new object[]{ aniClip });
                            allSize += (int)sizeInfo.GetValue(stats);
                        }
                        //var stats = getAnimationClipStats.Invoke(null, new object[]{ aniClip });
                        //Debug.Log(objs[i].name + " " + EditorUtility.FormatBytes((int)sizeInfo.GetValue(stats)));//BlobSize
                    }
                }
            }
        }
        Debug.Log("All Animation Size: " + EditorUtility.FormatBytes(allSize));
    }

    [MenuItem("Test/GetSelectedAnimSize")]
    public static void GetSelectedAnimSize()
    {
        Assembly asm = Assembly.GetAssembly(typeof(Editor));
        MethodInfo getAnimationClipStats = typeof(AnimationUtility).GetMethod("GetAnimationClipStats", BindingFlags.Static | BindingFlags.NonPublic);
        Type aniclipstats = asm.GetType("UnityEditor.AnimationClipStats");
        FieldInfo sizeInfo = aniclipstats.GetField ("size", BindingFlags.Public | BindingFlags.Instance);

        if (Selection.assetGUIDs.Length > 0)
        {
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
            Debug.Log(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
            for (int i = 0; i < objs.Length; ++i)
            {
                if (objs[i] is AnimationClip)
                {
                    AnimationClip aniClip = objs[i] as AnimationClip;
                    Debug.Log(Profiler.GetRuntimeMemorySize (aniClip));//MemorySize

                    var stats = getAnimationClipStats.Invoke(null, new object[]{aniClip});
                    Debug.Log(objs[i].name + " " + EditorUtility.FormatBytes((int)sizeInfo.GetValue(stats)));//BlobSize
                }
            }
        }

    }
}
