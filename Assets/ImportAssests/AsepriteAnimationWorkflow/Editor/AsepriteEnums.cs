namespace APIShift.AsepriteAnimationWorkflow
{
  public enum BlendMode : ushort
  {
    Normal = 0,
    Multiply = 1,
    Screen = 2,
    Overlay = 3,
    Darken = 4,
    Lighten = 5,
    ColorDodge = 6,
    ColorBurn = 7,
    HardLight = 8,
    SoftLight = 9,
    Difference = 10,
    Exclusion = 11,
    Hue = 12,
    Saturation = 13,
    Color = 14,
    Luminosity = 15,
    Addition = 16,
    Subtract = 17,
    Divide = 18
  }

  public enum CelType : ushort
  {
    RawCel = 0,
    LinkedCel = 1,
    CompressedImage = 2
  }

  public enum ChunkType : ushort
  {
    Layer = 0x2004,
    Cel = 0x2005,
    FrameTags = 0x2018,
    Palette = 0x2019
  }

  public enum ColorDepth : ushort
  {
    RGBA = 32,
    Grayscale = 16,
    Indexed = 8
  }

  public enum LayerType : ushort
  {
    Normal = 0,
    Group = 1
  }

  public enum AnimationDirection : byte
  {
    Forward = 0,
    Reverse = 1,
    PingPong = 2,
  }
}