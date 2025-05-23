#nullable enable

using Godot;

namespace Quill;

public partial class StubInkStory : InkStory
{
    internal override string RawStory
    {
        get => base.RawStory;
        set
        {
#if TOOLS
            if (Engine.IsEditorHint()) return;
#endif
            throw new InvalidInkException(
                "To load this story directly, please import it with 'is_main_file' set to true."
            );
        }
    }
}
