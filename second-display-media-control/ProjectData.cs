using System;
using System.Collections.Generic;

namespace second_display_media_control
{
    [Serializable]
    public class ProjectData
    {
        public List<string> FilePaths { get; set; } = new List<string>();
        public int CurrentPlayingIndex { get; set; } = -1;
        public bool AutoplayEnabled { get; set; } = false;
        public string CurrentPlayingUri { get; set; } = "";
        public bool SecondScreenEnabled { get; set; } = false;
        public int Volume { get; set; } = 50;

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
    }
}