using System;
using UnityEditor;
using UnityEngine;

namespace BerserkPixel.Readme
{
    public class UpdateCheck
    {
        private bool _showingVersionMessage;
        private string _versionLatest;

        private string _assetName;
        private string _assetVersion;
        private string _packageName;
        private BerserkURL _urls;

        public UpdateCheck(Readme readme)
        {
            Update(readme);

            _showingVersionMessage = false;
            _versionLatest = null;
        }

        public void Update(Readme readme)
        {
            _assetName = readme.AssetName;
            _assetVersion = readme.AssetVersion;
            _packageName = readme.PackageName;
            _urls = readme.urlsConfig;
        }

        public void DrawUpdateCheck()
        {
            if (_showingVersionMessage)
            {
                EditorGUILayout.Space(20);

                if (_versionLatest == null)
                {
                    EditorGUILayout.HelpBox($"Checking the latest version...", MessageType.None);
                }
                else
                {
                    var local = Version.Parse(_assetVersion);
                    var remote = Version.Parse(_versionLatest);
                    if (local >= remote)
                    {
                        EditorGUILayout.HelpBox($"You have the latest version! {_assetVersion}.",
                                                MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout
                            .HelpBox($"Update needed. " + $"The latest version is {_versionLatest}, but you have {_assetVersion}.",
                                     MessageType.Warning);
                        EditorGUILayout.Space(4);
                        if (GUILayout.Button("Open PackageManager"))
                        {
                            OpenPackageManager();
                        }
                        EditorGUILayout.Space(4);
                    }
                }
            }

            if (GUILayout.Button("Check for Updates"))
            {
                _showingVersionMessage = true;
                _versionLatest = null;
                CheckVersion(version => { _versionLatest = version; });
            }

            if (_showingVersionMessage)
            {
                EditorGUILayout.Space(20);
            }
        }

        private void CheckVersion(Action<string> callback)
        {
            NetworkManager.GetVersion(_assetName, _urls.URL_VERSION, callback);
        }

        private void OpenPackageManager()
        {
#if UNITY_2020_3_OR_NEWER
            UnityEditor.PackageManager.Client.Resolve();
#endif
            UnityEditor.PackageManager.UI.Window.Open(_packageName);
        }
    }
}