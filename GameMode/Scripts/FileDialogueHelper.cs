using System;
using System.Windows.Forms;

public static class FileDialogHelper
{
    public static string ShowOpenFileDialog(string filter = "All files (*.*)|*.*")
    {
        string selectedPath = null;

        using (OpenFileDialog dialog = new OpenFileDialog())
        {
            dialog.Filter = filter;
            dialog.Title = "Select a file";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedPath = dialog.FileName;
            }
        }

        return selectedPath;
    }
}