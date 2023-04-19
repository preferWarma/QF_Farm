using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public class Header
  {
    public Vector2Int FrameSize { get; private set; }
    public ColorDepth ColorDepth { get; private set; }
    public int FrameCount { get; private set; }
    public byte TransparentColorIndex { get; private set; }

    public Header(
      Vector2Int frameSize,
      ColorDepth colorDepth,
      int frameCount,
      byte transparentColorIndex)
    {
      FrameSize = frameSize;
      ColorDepth = colorDepth;
      FrameCount = frameCount;
      TransparentColorIndex = transparentColorIndex;
    }
  }
}
