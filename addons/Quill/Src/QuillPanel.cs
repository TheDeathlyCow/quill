#if TOOLS
#nullable enable

using Godot;
using Quill;
using System;

[Tool]
public partial class QuillPanel : Control
{
    [Export] private CodeEdit editor = null!;

    [Export] private Button loadButton = null!;

    [Export] private Button saveButton = null!;

    private EditorFileDialog fileDialog = null!;

    private bool dirty = false;

    private string? storyPath = null;

    public override void _Ready()
    {
        editor.CodeCompletionRequested += OnCodeCompletionRequested;
        editor.TextChanged += OnTextChanged;

        fileDialog = new EditorFileDialog()
        {
            FileMode = EditorFileDialog.FileModeEnum.OpenFile, Access = EditorFileDialog.AccessEnum.Resources
        };
        fileDialog.AddFilter("*.ink", "Ink stories");
        fileDialog.FileSelected += LoadStory;

        AddChild(fileDialog);

        loadButton.Pressed += () => fileDialog.PopupCenteredClamped(new Vector2I(1050, 700), 0.8f);

        saveButton.Pressed += SaveStory;
    }

    private void OnCodeCompletionRequested()
    {
        GD.Print("Requesting code completion");
        editor.AddCodeCompletionOption(CodeEdit.CodeCompletionKind.Constant, "Preview Text", "Insert Text");
        editor.UpdateCodeCompletionOptions(true);
    }

    private void OnTextChanged()
    {
        dirty = true;
    }

    public void InkResourceImported(string resourcePath)
    {
    }

    private void LoadStory(string path)
    {
        storyPath = path;

        using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            GD.PushError(FileAccess.GetOpenError());
            return;
        }

        editor.Text = file.GetAsText();
    }

    private void SaveStory()
    {
        if (dirty && storyPath != null)
        {
            using (FileAccess? file = FileAccess.Open(storyPath, FileAccess.ModeFlags.Write))
            {
                if (file == null || file.GetError() != Error.Ok)
                {
                    GD.PushError(FileAccess.GetOpenError());
                    return;
                }

                file.StoreString(editor.Text);
            }
            
            EditorInterface.Singleton.GetResourceFilesystem().UpdateFile(storyPath);
            EditorInterface.Singleton.GetResourceFilesystem().ReimportFiles([storyPath]);
            
            //EditorInterface.Singleton.EditResource(story);
            dirty = false;
            GD.Print("Saved story");
        }
        else
        {
            GD.Print("Nothing to save");
        }
    }
}

#endif
