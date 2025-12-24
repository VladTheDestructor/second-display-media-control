using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using second_display_media_control;

namespace second_display_media_control
{
    public class ProjectManager
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static bool SaveProject(string filePath, ProjectData project)
        {
            try
            {
                project.Modified = DateTime.Now;
                string json = JsonSerializer.Serialize(project, options);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving project: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static ProjectData LoadProject(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Project file not found", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                string json = File.ReadAllText(filePath);
                ProjectData project = JsonSerializer.Deserialize<ProjectData>(json, options);
                return project;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading project: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static string GetDefaultProjectPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "SDMC", "Projects");

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            return appFolder;
        }

        public static string GenerateProjectName()
        {
            return $"Project_{DateTime.Now:yyyyMMdd_HHmmss}.sdmc";
        }
    }
}