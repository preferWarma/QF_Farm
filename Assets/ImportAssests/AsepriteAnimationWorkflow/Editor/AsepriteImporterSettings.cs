using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace APIShift.AsepriteAnimationWorkflow
{
  [Serializable]
  public class AsepriteImporterSettings
  {
    const string CannotBeChanged = "(Cannot be changed for Aseprite imports)";

    private static Dictionary<SpriteAlignment, Vector2> _fixedPivots
      = new Dictionary<SpriteAlignment, Vector2>
      {
        [SpriteAlignment.Center] = new Vector2(0.5f, 0.5f),
        [SpriteAlignment.TopLeft] = new Vector2(0f, 1f),
        [SpriteAlignment.TopCenter] = new Vector2(0.5f, 1f),
        [SpriteAlignment.TopRight] = new Vector2(1f, 1f),
        [SpriteAlignment.LeftCenter] = new Vector2(0f, 0.5f),
        [SpriteAlignment.RightCenter] = new Vector2(1f, 0.5f),
        [SpriteAlignment.BottomLeft] = new Vector2(0f, 0f),
        [SpriteAlignment.BottomCenter] = new Vector2(0.5f, 0f),
        [SpriteAlignment.BottomRight] = new Vector2(1f, 0f)
      };

    [Tooltip(CannotBeChanged)]
    public TextureImporterType TextureType = TextureImporterType.Sprite;

    [Tooltip(CannotBeChanged)]
    public TextureImporterShape TextureShape = TextureImporterShape.Texture2D;

    [Tooltip(CannotBeChanged)]
    public SpriteImportMode SpriteMode = SpriteImportMode.Multiple;

    [Tooltip("How many pixels in the sprite correspond to one unit in the world.")]
    public int PixelsPerUnit = 100;

    [Tooltip("Type of sprite mesh to generate.")]
    public SpriteMeshType MeshType = SpriteMeshType.Tight;

    [Range(0, 32)]
    [Tooltip("How much empty area to leave around the sprite in the generated mesh.")]
    public uint ExtrudeEdges = 1;

    [Tooltip("Sprite pivot point in its localspace.")]
    public SpriteAlignment Pivot = SpriteAlignment.Center;

    [SerializeField]
    private Vector2 _pivot = new Vector2(0.5f, 0.5f);
    public Vector2 PivotValue
    {
      get => _fixedPivots.TryGetValue(Pivot, out Vector2 result) ? result : _pivot;
      set => _pivot = value;
    }

    [Tooltip("Generates a default physics shape from the outline of the sprite/s.")]
    public bool GeneratePhysicsShape = true;

    public TextureWrapMode WrapMode = TextureWrapMode.Clamp;

    public FilterMode FilterMode = FilterMode.Point;

    [Range(0, 32)]
    [Tooltip(CannotBeChanged)]
    public int AnisoLevel = 1;

    [Range(0, 32)]
    [Tooltip("Adds extra pixel space inside generated sprite/s edges. (Pivots are adjusted to accommodate)")]
    public int InnerPadding = 0;
  }
}