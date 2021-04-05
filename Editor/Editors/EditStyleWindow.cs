using ReactUnity.Editor.Renderer;
using UnityEditor;
using UnityEngine;

namespace ReactUnity.Editor
{
    public class EditStyleWindow : ReactWindow
    {
        [MenuItem("React/Show Inspector")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<EditStyleWindow>();
            wnd.titleContent = new GUIContent("React Inspector");
        }


        protected override ReactScript GetScript()
        {
            var res = ReactScript.Resource("ReactUnity/editor/inspector/index");
            res.UseDevServer = false;
            return res;
        }
    }
}
