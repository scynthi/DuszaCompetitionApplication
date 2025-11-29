using Godot;
using System;

public partial class FileDialogHelper : Node
{
    private FileDialog fileDialog;
    
    [Signal]
    public delegate void FileSelectedEventHandler(string path);
    
    [Signal]
    public delegate void FolderSelectedEventHandler(string path);
    
    public override void _Ready()
    {
        fileDialog = new FileDialog();
        fileDialog.Access = FileDialog.AccessEnum.Filesystem;
        fileDialog.FileSelected += (path) => EmitSignal(SignalName.FileSelected, path);
        fileDialog.DirSelected += (path) => EmitSignal(SignalName.FolderSelected, path);
        AddChild(fileDialog);
    }
    
    public void ShowFileDialog(string[] filters = null)
    {
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        if (filters != null)
            fileDialog.Filters = filters;
        fileDialog.PopupCentered(new Vector2I(800, 600));
    }
    
    public void ShowFolderDialog()
    {
        fileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
        fileDialog.PopupCentered(new Vector2I(800, 600));
    }
    
    public System.Threading.Tasks.Task<string> WaitForFolderSelection()
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<string>();
        FolderSelected += (path) => tcs.SetResult(path);
        ShowFolderDialog();
        return tcs.Task;
    }
    
    public System.Threading.Tasks.Task<string> WaitForFileSelection(string[] filters = null)
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<string>();
        FileSelected += (path) => tcs.SetResult(path);
        ShowFileDialog(filters);
        return tcs.Task;
    }
}