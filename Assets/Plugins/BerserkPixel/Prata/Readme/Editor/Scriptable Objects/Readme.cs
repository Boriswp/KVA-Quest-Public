using System;
using UnityEngine;

namespace BerserkPixel.Readme
{
    [CreateAssetMenu(menuName = "Readme/Readme", fileName = "Readme")]
    public class Readme : ScriptableObject
    {
        [NonSerialized] public string AssetVersion = "1.3.0";
        [NonSerialized] public bool checkUpdate = true;
        [NonSerialized] public bool documentation = true;
        [NonSerialized] public bool contact = true;
        [NonSerialized] public bool debug = true;
        [NonSerialized] public string AssetName = "Prata";
        [NonSerialized] public string PackageName = "Prata: Dialogues in seconds";
        [NonSerialized] public string FileType = "BerserkPixel.Prata";

        [DrawIf("ShowInInspector", true)]
        public BerserkURL urlsConfig = new BerserkURL();

        [NonSerialized] public string UnityVersion = Application.unityVersion;

        [HideInInspector]
        public bool ShowInInspector = false;

        private void OnValidate()
        {
            ShowInInspector = urlsConfig == null;
        }
    }
}