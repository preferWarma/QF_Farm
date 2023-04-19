namespace APIShift.AsepriteAnimationWorkflow
{
  public class Layer
  {
    public LayerType LayerType { get; private set; }
    public BlendMode BlendMode { get; private set; }
    public float Opacity { get; private set; }
    public bool Visible { get; private set; }
    public int ChildLevel { get; private set; }

    public Layer(
      LayerType layerType,
      BlendMode blendMode,
      float opacity,
      bool visible,
      int childLevel)
    {
      LayerType = layerType;
      BlendMode = blendMode;
      Opacity = opacity;
      Visible = visible;
      ChildLevel = childLevel;
    }
  }
}
