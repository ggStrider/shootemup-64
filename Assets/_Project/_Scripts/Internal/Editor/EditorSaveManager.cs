using System.Diagnostics;
using System.IO;
using Internal.Core.DataModel;
using UnityEditor;
using UnityEngine;

namespace Internal.Editor
{
    public class EditorSaveManager : EditorWindow
    {
        private bool _isFileExist;

        [MenuItem(StaticKeys.PROJECT_NAME + "/Menus/Save Manager")]
        private static void OpenWindow()
        {
            GetWindow<EditorSaveManager>(utility: false, title: "Save Manager");
        }

        private void OnBecameVisible()
        {
            InitializeIsSaveExist();
        }

        private void InitializeIsSaveExist()
        {
            _isFileExist = File.Exists(SaveSystem.SavePath);
        }

        private void OnGUI()
        {
            if (_isFileExist)
            {
                DrawCustomGUI();
            }
            else
            {
                ReadyEditorElements.DrawRichLabel("No Save Found!", Color.red,
                    fontStyle: FontStyle.Bold);

                ReadyEditorElements.DrawRichButton("<b>Check is save exist again</b>", Color.yellow,
                    InitializeIsSaveExist);
            }
        }

        private void DrawCustomGUI()
        {
            ReadyEditorElements.DrawRichButton("<b>Open Save File</b>", Color.green, OpenSaveFile);
            ReadyEditorElements.DrawRichButton("<b>Delete Save File</b>", Color.yellow, DeleteSave);
        }

        private void DeleteSave()
        {
            SaveSystem.DeleteSave_Editor();
            InitializeIsSaveExist();
        }

        private void OpenSaveFile()
        {
            InitializeIsSaveExist();
            if (!_isFileExist) return;
            
            var processStartInfo = new ProcessStartInfo
            {
                FileName = SaveSystem.SavePath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
    }
}