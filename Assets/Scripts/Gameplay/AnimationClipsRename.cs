using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AnimationClipsRename : MonoBehaviour
{
    [SerializeField] AnimationClip clip = null;
    [SerializeField] string oldName = String.Empty;
    [SerializeField] string rename = String.Empty;

    private void Awake()
    {
        if (clip != null)
        {
            for (int i = 0; i < AnimationUtility.GetCurveBindings(clip).Length; i++)
            {
                AnimationCurve animCurve = AnimationUtility.GetEditorCurve(clip, AnimationUtility.GetCurveBindings(clip)[i]);
                EditorCurveBinding CopyCurve = AnimationUtility.GetCurveBindings(clip)[i];
                CopyCurve.path = CopyCurve.path.Replace(oldName, rename);
                AnimationUtility.SetEditorCurve(clip, CopyCurve, animCurve);
                Debug.Log(AnimationUtility.GetCurveBindings(clip)[i].path);
            }
        }
    }

}
