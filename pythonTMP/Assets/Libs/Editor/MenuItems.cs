using UnityEditor;

namespace ZhuYuU3d.UnityEditor
{
    public class MenuItems
    {
        private const string ROOT_NAME = "UnityEditor";

        [MenuItem(ROOT_NAME + "/ProjectSettings/Include Built-in Shaders", priority = 9)]
        private static void IncludeBuiltinShaders()
        {
            GraphicsSettings.IncludeBuiltinShaders();
        }

    }
}