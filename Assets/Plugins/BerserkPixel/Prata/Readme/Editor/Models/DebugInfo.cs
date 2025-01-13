using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BerserkPixel.Readme
{
    public class DebugInfo
    {
        private string _assetName;
        private string _assetVersion;
        private string _unityVersion;

        public DebugInfo(Readme readme)
        {
            _assetName = readme.AssetName;
            _assetVersion = readme.AssetVersion;
            _unityVersion = readme.UnityVersion;
        }

        public void DrawDebug()
        {
            Utils.DrawUILine(Color.gray, 1, 20);
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Debug info", EditorStyles.miniBoldLabel);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Copy", EditorStyles.miniButtonLeft))
            {
                CopyDebugInfoToClipboard();
            }

            if (EditorGUIUtility.systemCopyBuffer == GetDebugInfoString())
            {
                EditorGUILayout.LabelField("Copied!", EditorStyles.miniLabel);
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            var debugInfo = GetDebugInfo();
            foreach (var s in debugInfo)
            {
                EditorGUILayout.LabelField($"    " + s, EditorStyles.miniLabel);
            }

            EditorGUILayout.Separator();
        }

        public void CopyDebugInfoToClipboard()
        {
            EditorGUIUtility.systemCopyBuffer = GetDebugInfoString();
        }

        public string GetDebugInfoString()
        {
            string[] info = GetDebugInfo();
            return String.Join("\n", info);
        }

        public string[] GetDebugInfo()
        {
            var info = new List<string> {
                $"{_assetName} version {_assetVersion}",
                $"Unity {_unityVersion}",
                $"Dev platform: {Application.platform}",
                $"Target platform: {EditorUserBuildSettings.activeBuildTarget}",
                $"Render pipeline: {Shader.globalRenderPipeline}"
            };

            return info.ToArray();
        }
    }
}