using System;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public class Cel
  {
    public Layer Layer { get; private set; }
    public float Opacity { get; private set; }

    private readonly Color[] _pixels;
    private readonly short _x;
    private readonly short _y;
    private readonly ushort _width;
    private readonly ushort _height;

    public Cel(
      Layer layer,
      Color[] pixels,
      short x,
      short y,
      ushort width,
      ushort height,
      float opacity)
    {
      Layer = layer;
      Opacity = opacity;

      _pixels = pixels;
      _x = x;
      _y = y;
      _width = width;
      _height = height;
    }

    public Color[] GetCelFramePixels(Vector2Int frameSize)
    {
      var target = new RectInt(_x, _y, _width, _height);
      target.xMax = Math.Min(target.xMax, frameSize.x);
      target.yMax = Math.Min(target.yMax, frameSize.y);
      target.xMin = Math.Max(target.xMin, 0);
      target.yMin = Math.Max(target.yMin, 0);
      var pixels = new Color[frameSize.x * frameSize.y];
      for (var y = target.yMin; y < target.yMax; y++)
      {
        for (var x = target.xMin; x < target.xMax; x++)
        {
          var srcIndex = (y - _y) * _width + (x - _x);
          var tgtIndex = (frameSize.y - y - 1) * frameSize.x + x;
          pixels[tgtIndex] = _pixels[srcIndex];
        }
      }
      return pixels;
    }
  }
}
