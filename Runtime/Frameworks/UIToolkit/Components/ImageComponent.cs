using ReactUnity.Styling;
using ReactUnity.Styling.Converters;
using ReactUnity.Types;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReactUnity.UIToolkit
{
    public class ImageComponent : UIToolkitComponent<Image>
    {
        public ImageComponent(UIToolkitContext context, string tag) : base(context, tag)
        { }

        public override void SetProperty(string property, object value)
        {
            if (property == "source") SetSource(value);
            else base.SetProperty(property, value);
        }

        protected void SetSource(object value)
        {
            if (!AllConverters.ImageSourceConverter.TryGetConstantValue<ImageReference>(value, out var source))
                source = ImageReference.None;
            source.Get(Context, SetTexture);
        }

        protected void SetTexture(Texture2D texture)
        {
            Element.image = texture;
        }

        protected override void ApplyStylesSelf()
        {
            base.ApplyStylesSelf();

            if (ComputedStyle.HasValue(StyleProperties.color)) Element.tintColor = ComputedStyle.color;
            else Element.tintColor = Color.white;
        }
    }
}
