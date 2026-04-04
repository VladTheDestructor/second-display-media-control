using System;
using System.Collections.Generic;

namespace second_display_media_control
{
    [Serializable]
    public class ProjectData
    {
        public List<ProjectFileInfo> Files { get; set; } = new List<ProjectFileInfo>();
        public int CurrentPlayingIndex { get; set; } = -1;
        public bool AutoplayEnabled { get; set; } = false;
        public string CurrentPlayingUri { get; set; } = "";
        public bool SecondScreenEnabled { get; set; } = false;
        public int Volume { get; set; } = 50;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
    }

    [Serializable]
    public class ProjectFileInfo
    {
        public string FilePath { get; set; }
        public string BackgroundImagePath { get; set; }
    }
}