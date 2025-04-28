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

    private InkStory? story = null;

    public override void _Ready()
    {
        editor.CodeCompletionRequested += OnCodeCompletionRequested;
        editor.TextChanged += OnTextChanged;

        fileDialog = new EditorFileDialog()
        {
            FileMode = EditorFileDialog.FileModeEnum.OpenFile,
            Access = EditorFileDialog.AccessEnum.Resources
        };
        
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
        try
        {
            story = ResourceLoader.Load<InkStory>(path, null, ResourceLoader.CacheMode.Ignore);
            FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                throw new InvalidCastException();
            }

            editor.Text = file.GetAsText();
        }
        catch (InvalidCastException)
        {
            GD.PrintErr($"{path} is not a valid ink story. "
                        + "Please make sure it was imported with `is_main_file` set to `true`.");
        }
    }

    private void SaveStory()
    {
        if (story != null)
        {
            FileAccess file = FileAccess.Open(story.ResourcePath, FileAccess.ModeFlags.Write);
            
            if (file.GetError() != Error.Ok)
            {
                GD.PushError("Unable to access story file: "+ file.GetError());
                return;
            }

            file.StoreString(editor.Text);
            file.Close();
            ResourceSaver.Save(story);
            
            EditorInterface.Singleton.GetResourceFilesystem().ReimportFiles([ story.ResourcePath ]);
            EditorInterface.Singleton.EditResource(story);
            
            GD.Print("Saved story");
        }
    }
}

#endif
