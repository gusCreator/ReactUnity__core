using System;
using System.Collections.Generic;
using ReactUnity.Styling;
using ReactUnity.Styling.Computed;
using ReactUnity.Styling.Converters;
using UnityEngine;

namespace ReactUnity.Types
{
    public abstract class ImageDefinition
    {
        public class ResolvedImage
        {
            public static ResolvedImage Default = new ResolvedImage();

            public Texture2D Texture;
            public Vector2 IntrinsicSize = new Vector2(float.NaN, float.NaN);
            public float IntrinsicProportions = float.NaN;

            private Sprite sprite;
            public Sprite Sprite => Texture == null ? null :
                (sprite = sprite ?? Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), Vector2.one / 2));
        }

        public static ImageDefinition None { get; } = UrlImageDefinition.None;

        public virtual bool SizeUpdatesGraphic => false;

        internal virtual void ResolveImage(ReactContext context, Vector2 size, Action<ResolvedImage> callback) => callback(ResolvedImage.Default);

        internal virtual void ModifyMaterial(ReactContext context, Material material, Vector2 size) { }

        public class Converter : TypedStyleConverterBase<ImageDefinition>
        {
            private static HashSet<string> DefaultAllowedFunctions = new HashSet<string> {
                "linear-gradient",
                "repeating-linear-gradient",
                "radial-gradient",
                "repeating-radial-gradient",
                "conic-gradient",
                "repeating-conic-gradient",
            };
            private static StyleConverterBase ImageConverter = AllConverters.ImageReferenceConverter;

            public override bool CanHandleKeyword(CssKeyword keyword) => keyword == CssKeyword.None;

            protected override HashSet<string> AllowedFunctions => DefaultAllowedFunctions;

            protected override bool ConvertInternal(object value, out IComputedValue result)
            {
                return ComputedMapper.Create(out result, value, ImageConverter,
                    (object resolved, out IComputedValue rs) => {
                        if (resolved is ImageReference irr) return Constant(new UrlImageDefinition(irr), out rs);
                        return Fail(out rs);
                    });
            }

            protected override bool ParseInternal(string value, out IComputedValue result)
            {
                if (value == "none") return Constant(None, out result);
                if (CssFunctions.TryCall(value, out var gd, AllowedFunctions))
                {
                    if (gd is BaseGradient u) return Constant(new GradientImageDefinition(u), out result);
                }

                return ComputedMapper.Create(out result, value, ImageConverter,
                    (object resolved, out IComputedValue rs) => {
                        if (resolved is ImageReference irr) return Constant(new UrlImageDefinition(irr), out rs);
                        return Fail(out rs);
                    });
            }
        }
    }

    public class UrlImageDefinition : ImageDefinition
    {
        public static new UrlImageDefinition None { get; } = new UrlImageDefinition(ImageReference.None);
        public ImageReference Reference { get; }

        public UrlImageDefinition(ImageReference reference)
        {
            Reference = reference;
        }

        internal override void ResolveImage(ReactContext context, Vector2 size, Action<ResolvedImage> callback)
        {
            Reference.Get(context, tx => {
                callback(tx == null ? null : new ResolvedImage
                {
                    Texture = tx,
                    IntrinsicSize = new Vector2(tx.width, tx.height),
                    IntrinsicProportions = tx.width / tx.height,
                });
            });
        }
    }

    public class GradientImageDefinition : ImageDefinition
    {
        public BaseGradient Gradient { get; }
        public override bool SizeUpdatesGraphic => Gradient.SizeUpdatesGraphic;

        public GradientImageDefinition(BaseGradient gradient)
        {
            Gradient = gradient;
        }

        internal override void ResolveImage(ReactContext context, Vector2 size, Action<ResolvedImage> callback)
        {
            var calc = Gradient.GetRamp(size);
            callback(new ResolvedImage
            {
                Texture = calc.Texture,
            });
        }

        internal override void ModifyMaterial(ReactContext context, Material material, Vector2 size) => Gradient.ModifyMaterial(context, material, size);
    }
}
