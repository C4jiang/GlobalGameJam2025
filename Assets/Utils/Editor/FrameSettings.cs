using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class FrameSettings : MonoBehaviour
{
    [SerializeField] private int frame;

    [Button]
    public void SetFrames(int frame)
    {
        Application.targetFrameRate = frame;
    }
    public void OnGUI()
    {
        GUI.skin.button.wordWrap = true;
        if (GUILayout.Button("SetFrame"))
        {
            SetFrames(frame);
        }


    }
}



