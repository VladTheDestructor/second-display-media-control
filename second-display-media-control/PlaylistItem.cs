using System;
using System.Drawing;
using System.IO;

namespace second_display_media_control
{
    public class PlaylistItem
    {
        public string FilePath { get; set; }
        public string BackgroundImagePath { get; set; } // может быть null
        public Icon ThumbnailIcon { get; set; }         // для отображения в Preview

        // Вспомогательный метод для определения типа файла
        public bool IsAudioFile()
        {
            string ext = Path.GetExtension(FilePath)?.ToLower();
            return ext == ".mp3" || ext == ".wav" || ext == ".flac" || ext == ".ogg" || ext == ".m4a";
        }
    }
}