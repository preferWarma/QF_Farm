using System;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public static class ColorExtensions
  {
    public static double Luminosity(this Color color)
      => (0.3 * color.r) + (0.59 * color.g) + (0.11 * color.b);

    public static Color WithLuminosity(this Color color, double l)
    {
      var d = l - color.Luminosity();
      color.r = (float)(color.r + d);
      color.g = (float)(color.g + d);
      color.b = (float)(color.b + d);
      return Clamp(color);
    }

    public static double Saturation(this Color color)
      => Math.Max(color.r, Math.Max(color.g, color.b))
       - Math.Min(color.r, Math.Min(color.g, color.b));

    private static float[] c = new float[3];
    private static int[] i = new int[] { 0, 1, 2 };
    private static void Swap(int a, int b)
    {
      var p = i[b];
      i[b] = i[a];
      i[a] = p;
    }

    public static Color WithSaturation(this Color color, double s)
    {
      if (color.r == color.g && color.r == color.b)
      {
        return new Color(0, 0, 0);
      }
      c[0] = color.r;
      c[1] = color.g;
      c[2] = color.b;
      if (c[i[0]] > c[i[1]]) Swap(0, 1);
      if (c[i[1]] > c[i[2]]) Swap(1, 2);
      if (c[i[0]] > c[i[1]]) Swap(0, 1);
      var min = c[i[0]];
      var mid = c[i[1]];
      var max = c[i[2]];
      c[i[0]] = 0;
      c[i[1]] = (float)(((mid - min) * s) / (max - min));
      c[i[2]] = (float)s;
      color.r = c[0];
      color.g = c[1];
      color.b = c[2];
      return Clamp(color);
    }

    private static Color Clamp(Color c)
    {
      var l = c.Luminosity();
      var n = Math.Min(c.r, Math.Min(c.g, c.b));
      var x = Math.Max(c.r, Math.Max(c.g, c.b));
      if (n < 0)
      {
        c.r = (float)(l + (((c.r - l) * l) / (l - n)));
        c.g = (float)(l + (((c.g - l) * l) / (l - n)));
        c.b = (float)(l + (((c.b - l) * l) / (l - n)));
      }
      if (x > 1)
      {
        c.r = (float)(l + (((c.r - l) * (1 - l)) / (x - l)));
        c.g = (float)(l + (((c.g - l) * (1 - l)) / (x - l)));
        c.b = (float)(l + (((c.b - l) * (1 - l)) / (x - l)));
      }
      return c;
    }
  }
}