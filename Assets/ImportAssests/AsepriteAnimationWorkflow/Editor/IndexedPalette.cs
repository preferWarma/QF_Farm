using System.Collections.Generic;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public class IndexedPalette
  {
    private uint _startIndex;
    private uint _endIndex;
    private IList<Color> _colors;
    private byte _transparentIndex;

    public IndexedPalette(
      IList<Color> colors,
      uint startIndex,
      uint endIndex,
      byte transparentIndex)
    {
      _colors = colors;
      _startIndex = startIndex;
      _endIndex = endIndex;
      _transparentIndex = transparentIndex;
    }

    public Color GetColor(byte index)
        => (index != _transparentIndex && index >= _startIndex && index <= _endIndex)
          ? _colors[index]
          : Color.clear;
  }
}
