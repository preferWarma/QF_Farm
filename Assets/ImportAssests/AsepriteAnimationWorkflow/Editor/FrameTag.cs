namespace APIShift.AsepriteAnimationWorkflow
{
  public class FrameTag
  {
    public string Name { get; private set; }
    public int FrameFrom { get; private set; }
    public int FrameTo { get; private set; }
    public AnimationDirection Direction { get; private set; }

    public FrameTag(
      string name,
      int frameFrom,
      int frameTo,
      AnimationDirection direction)
    {
      Name = name;
      FrameFrom = frameFrom;
      FrameTo = frameTo;
      Direction = direction;
    }
  }
}