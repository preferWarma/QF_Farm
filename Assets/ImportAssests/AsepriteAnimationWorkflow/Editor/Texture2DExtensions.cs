using UnityEngine;
using System.Linq;

public static class Texture2DExtensions
{
  public static void Clear(this Texture2D texture, Color color)
  {
    var pixels = Enumerable
                .Range(0, texture.width * texture.height)
                .Select(i => Color.clear)
                .ToArray();
    texture.SetPixels(pixels);
    texture.Apply();
  }
}
