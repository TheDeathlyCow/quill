# Quill

An [Ink](https://github.com/inkle/ink) integration for [Godot Engine 4](https://github.com/godotengine/godot), based on [GodotInk](https://github.com/paulloz/godot-ink) by paulloz.  

If you use and like this project, please consider buying me a coffee.  
[![ko-fi](https://img.shields.io/badge/support_me_on_ko--fi-ff5e5b?style=for-the-badge&logo=kofi&logoColor=f5f5f5)](https://ko-fi.com/E1E53SKZF)

## Usage

You'll find a quick-start guide, and the general documentation on the project's
[wiki](./docs).

### Installation

The following instructions assume that you have a working Godot and .NET installation. If not, please refer to the
official [engine documentation](https://docs.godotengine.org/).

1. [Download](https://github.com/TheDeathlyCow/quill/releases/latest) and extract the code at the root of your project.
   You should see a new `addons/Quill/` folder in your project's directory.

2. Add the following line in your `.csproj` file (before the closing `</Project>` tag).
   ```xml
   <Import Project="addons\Quill\Quill.props" />
   ```
   If you don't have a `.csproj` file, click the **Create C# Solution** button in the editor's
   **Project/Tools** menu.

3. Build your project once.

4. Enable **Quill** in your project settings.

## License

*GodotInk* is released under MIT license (see the [LICENSE](./LICENSE) file for more information).
