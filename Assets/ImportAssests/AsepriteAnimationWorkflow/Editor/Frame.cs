using System.Collections.Generic;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public class Frame
  {
    public IList<Cel> Cels { get; private set; }
    public float Duration { get; private set; }

    public Frame(IList<Cel> cels, float duration)
    {
      Cels = cels;
      Duration = duration;
    }

    public Color[] GetFramePixels(Vector2Int frameSize)
    {
      var pixels = new Color[frameSize.x * frameSize.y];
      foreach (var cel in Cels)
      {
        if (!cel.Layer.Visible || cel.Layer.LayerType == LayerType.Group)
          continue;

        var celPixels = cel.GetCelFramePixels(frameSize);
        var opacity = cel.Layer.Opacity * cel.Opacity;
        pixels = BlendFunc.Lookup[cel.Layer.BlendMode](pixels, celPixels, opacity);
      }
      return pixels;
    }
  }
}
