using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public static class BlendFunc
  {
    public static Dictionary<BlendMode, Func<Color[], Color[], float, Color[]>> Lookup
      = new Dictionary<BlendMode, Func<Color[], Color[], float, Color[]>>
      {
        [BlendMode.Normal] = Normal,
        [BlendMode.Multiply] = Multiply,
        [BlendMode.Screen] = Screen,
        [BlendMode.Overlay] = Overlay,
        [BlendMode.Darken] = Darken,
        [BlendMode.Lighten] = Lighten,
        [BlendMode.ColorDodge] = ColorDodge,
        [BlendMode.ColorBurn] = ColorBurn,
        [BlendMode.HardLight] = HardLight,
        [BlendMode.SoftLight] = SoftLight,
        [BlendMode.Difference] = Difference,
        [BlendMode.Exclusion] = Exclusion,
        [BlendMode.Hue] = Hue,
        [BlendMode.Saturation] = Saturation,
        [BlendMode.Color] = Color,
        [BlendMode.Luminosity] = Luminosity,
        [BlendMode.Addition] = Addition,
        [BlendMode.Subtract] = Subtract,
        [BlendMode.Divide] = Divide
      };

    public static Color[] Normal(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src) => src);

    public static Color[] Multiply(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src) => tgt * src);

    private static float Screen(float tgt, float src) => tgt + src - (tgt * src);
    public static Color[] Screen(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, Screen);

    public static Color[] Overlay(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src)
          => (tgt < 0.5)
          ? 2f * tgt * src
          : 1f - 2f * (1f - src) * (1f - tgt));

    public static Color[] Darken(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, Mathf.Min);

    public static Color[] Lighten(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, Mathf.Max);

    public static Color[] ColorDodge(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src)
          => tgt <= 0 ? 0
          : tgt >= 1 - src ? 1
          : tgt / (1 - src));

    public static Color[] ColorBurn(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src)
          => tgt >= 1 ? 1
           : 1 - tgt >= src ? 0
           : 1 - (1 - tgt) / src);

    public static Color[] HardLight(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src)
          => src <= 0.5f
           ? tgt * 2 * src
           : Screen(tgt, 2 * src - 1));

    public static float SoftLight(float src, float s)
      => s <= 0.5
       ? src - (1 - 2 * s) * src * (1 - src)
       : src
        + (2 * s - 1)
        * ((src <= 0.25
          ? ((16 * src - 12) * src + 4) * src
          : Mathf.Sqrt(src))
        - src);

    public static Color[] SoftLight(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, SoftLight);

    public static Color[] Difference(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src) => Mathf.Abs(src - tgt));

    public static Color[] Exclusion(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src) => tgt + src - 2 * tgt * src);

    public static Color[] Addition(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src) => tgt + src);

    public static Color[] Subtract(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src) => tgt - src);

    public static Color[] Divide(Color[] target, Color[] source, float opacity)
      => BlendComponents(target, source, opacity, (tgt, src)
          => tgt <= 0 ? 0
           : tgt >= src ? 255
           : tgt / src);

    public static Color[] Hue(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src)
        => src.WithSaturation(tgt.Saturation())
              .WithLuminosity(tgt.Luminosity()));

    public static Color[] Saturation(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src)
        => tgt.WithSaturation(src.Saturation())
              .WithLuminosity(tgt.Luminosity()));

    public static Color[] Color(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src)
        => src.WithLuminosity(tgt.Luminosity()));

    public static Color[] Luminosity(Color[] target, Color[] source, float opacity)
      => BlendColors(target, source, opacity, (tgt, src)
        => tgt.WithLuminosity(src.Luminosity()));

    private static Color BlendColor(
      Color target,
      Color source,
      Func<Color, Color, Color> blend,
      float opacity)
    {
      var c = blend(target, source);
      source.a *= opacity;
      c = ((1f - source.a) * target) + (source.a * c);
      c.a = target.a + source.a * (1f - target.a);
      return c;
    }

    private static Color[] BlendComponents(
      Color[] target,
      Color[] source,
      float opacity,
      Func<float, float, float> blend)
      => BlendColors(target, source, opacity, (tgt, src)
        => new Color(
          blend(tgt.r, src.r),
          blend(tgt.g, src.g),
          blend(tgt.b, src.b)));

    private static Color[] BlendColors(
      Color[] target,
      Color[] source,
      float opacity,
      Func<Color, Color, Color> blend)
    => Enumerable
      .Range(0, target.Length)
      .Select((s, i) => BlendColor(
        target[i],
        source[i],
        blend,
        opacity
      ))
      .ToArray();
  }
}

