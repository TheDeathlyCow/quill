@tool
extends EditorPlugin


var _importer: EditorImportPlugin
var _main_panel: Control


func _enter_tree() -> void:
	var src_path : String = get_script().get_path().get_base_dir().path_join("Src")

	var importer_script : CSharpScript = load(src_path.path_join("InkStoryImporter.cs")) as CSharpScript
	if not importer_script.can_instantiate():
		return
	_importer = importer_script.new() as EditorImportPlugin
	add_import_plugin(_importer)

	var main_panel_scene: PackedScene = load(src_path.path_join("QuillPanel.tscn")) as PackedScene
	_main_panel = main_panel_scene.instantiate() as Control
	_make_visible(false)
	
	var fs : EditorFileSystem = get_editor_interface().get_resource_filesystem()
	if not fs.resources_reimported.is_connected(_when_resources_reimported):
		fs.resources_reimported.connect(_when_resources_reimported)


func _exit_tree() -> void:
	var fs : EditorFileSystem = get_editor_interface().get_resource_filesystem()
	if fs.resources_reimported.is_connected(_when_resources_reimported):
		fs.resources_reimported.disconnect(_when_resources_reimported)

	if _main_panel != null:
		_main_panel.queue_free()
		_main_panel = null

	if _importer != null:
		remove_import_plugin(_importer)
		_importer = null


func _notification(what : int) -> void:
	if what == NOTIFICATION_POST_ENTER_TREE:
		if _importer != null:
			return
		get_editor_interface().set_plugin_enabled("Quill", false)
		printerr(
			"Quill could not be loaded.\n" +
			"Make sure your .csproj file references GodotInk.props, and rebuild the solution.\n" +
			"See the installation guide for more information: https://github.com/paulloz/godot-ink/wiki\n" +
			"If you think this is not an issue on your end, please open a bug report: https://github.com/paulloz/godot-ink/issues/new/choose"
		)


func _when_resources_reimported(resources : PackedStringArray) -> void:
	pass


func _make_visible(visible):
	if _main_panel:
		_main_panel.visible = visible


# main screen handling
func _has_main_screen() -> bool:
	return true


func _get_plugin_name() -> String:
	return "Quill"


func _get_plugin_icon() -> Texture2D:
	var src_path : String = get_script().get_path().get_base_dir()

	return load(src_path.path_join("Quill.svg")) as Texture2D
