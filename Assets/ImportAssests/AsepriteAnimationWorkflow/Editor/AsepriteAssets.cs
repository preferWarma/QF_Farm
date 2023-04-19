using System.Collections.Generic;
using UnityEngine;


namespace APIShift.AsepriteAnimationWorkflow
{
  public class AsepriteAssets
  {
    private Texture2D _spritesheet;
    private IEnumerable<Sprite> _sprites;
    private IEnumerable<AnimationClip> _animations;

    public AsepriteAssets(
      Texture2D spritesheet,
      IEnumerable<Sprite> sprites,
      IEnumerable<AnimationClip> animations)
    {
      _spritesheet = spritesheet;
      _sprites = sprites;
      _animations = animations;
    }

    public void AddToContext(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
      ctx.AddObjectToAsset(_spritesheet.name, _spritesheet);
      foreach (var sprite in _sprites)
      {
        ctx.AddObjectToAsset(sprite.name, sprite);
      }
      foreach (var animation in _animations)
      {
        ctx.AddObjectToAsset(animation.name, animation);
      }
      ctx.SetMainObject(_spritesheet);
    }
  }
}