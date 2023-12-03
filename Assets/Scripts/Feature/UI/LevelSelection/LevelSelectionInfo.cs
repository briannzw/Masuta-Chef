using System.Collections.Generic;
using UnityEngine;


namespace LevelSelection
{
    [System.Serializable]
    public class LevelSelectionInfo
    {
        public Sprite MapIcon;
        public string Region = "Praksha";
        public List<string> Objectives = new();
        public Sprite LevelImage;
        [TextArea] public string Instruction;
        public bool IsTutorial = false;
    }
}