using Godot;
using System;

[Tool]
public partial class QuillPanel : Control
{
    [Export] private CodeEdit _editor;

    public override void _Ready()
    {
        if (_editor != null)
        {
            _editor.TextChanged += OnTextChanged;
        }
        else
        {
            GD.PrintErr("Code editor is not set!");
        }
    }

    private void OnTextChanged()
    {
        
    }
}
