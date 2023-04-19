using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


namespace APIShift.AsepriteAnimationWorkflow
{
  public class Aseprite
  {
    private Vector2Int _frameSize;
    private IList<Frame> _frames;
    private IList<FrameTag> _frameTags;

    public Aseprite(
      Vector2Int frameSize,
      IList<Frame> frames,
      IList<FrameTag> frameTags)
    {
      _frameSize = frameSize;
      _frames = frames;
      _frameTags = frameTags;
    }

    public AsepriteAssets CreateAssets(AsepriteImporterSettings settings, string name)
    {
      var padding = settings.InnerPadding;
      var paddedFrameSize = new Vector2Int(
        _frameSize.x + padding * 2,
        _frameSize.y + padding * 2);

      var spritesheet = CreateBlankSpritesheet(
        settings,
        paddedFrameSize,
        _frames.Count,
        out int cols);

      var sprites = new List<Sprite>();
      for (var i = 0; i < _frames.Count; ++i)
      {
        var frame = _frames[i];
        var col = i % cols;
        var row = Mathf.FloorToInt(i / (float)cols) + 1;

        var spriteRect = new Rect(
            col * paddedFrameSize.x,
            spritesheet.height - (row * paddedFrameSize.y),
            paddedFrameSize.x,
            paddedFrameSize.y);

        var framePixels = frame.GetFramePixels(_frameSize);
        spritesheet.SetPixels(
            (int)spriteRect.x + padding,
            (int)spriteRect.y + padding,
            _frameSize.x,
            _frameSize.y,
            framePixels);

        var spriteData = new UnityEditor.AssetImporters.SpriteImportData
        {
          rect = spriteRect,
          pivot = settings.PivotValue
                + new Vector2(
                    padding / (float)paddedFrameSize.x,
                    padding / (float)paddedFrameSize.y),
          name = i.ToString()
        };

        var sprite = CreateNamedSprite(
            string.Format("{0}_{1}", name, spriteData.name),
            spritesheet,
            spriteData,
            settings);

        sprites.Add(sprite);
      }
      spritesheet.Apply();

      var animations = CreateAnimations(name, sprites, typeof(SpriteRenderer));
      var uiAnimName = $"UI_{name}";
      var uiAnimations = CreateAnimations(uiAnimName, sprites, typeof(Image));
      return new AsepriteAssets(
        spritesheet,
        sprites,
        animations.Concat(uiAnimations));
    }

    private List<AnimationClip> CreateAnimations(
      string name,
      List<Sprite> sprites,
      Type bindingType)
    {
      var frameTags = _frameTags.Count > 0
              ? _frameTags
              : CreateDefaultFrameTags(sprites.Count);

      var animations = new List<AnimationClip>();
      foreach (var frameTag in frameTags)
      {
        var animTimePos = 0f;
        var sourceFrameCount = (frameTag.FrameTo - frameTag.FrameFrom) + 1;
        var targetFrameCount = sourceFrameCount;
        var reversed = frameTag.Direction == AnimationDirection.Reverse;
        if (frameTag.Direction == AnimationDirection.PingPong && targetFrameCount > 2)
        {
          targetFrameCount = (targetFrameCount * 2) - 2;
        }

        var keyFrames = new List<ObjectReferenceKeyframe>();
        for (var i = 0; i < targetFrameCount; ++i)
        {
          var sourceFrame = i;
          if (sourceFrame >= sourceFrameCount)
          { // Only applies to ping pong animations
            sourceFrame -= sourceFrameCount - 1;
            reversed = true;
          }
          var frame = reversed ? frameTag.FrameTo - sourceFrame : frameTag.FrameFrom + sourceFrame;

          var sprite = sprites[frame];
          var keyFrame = CreateKeyFrame(animTimePos, sprite);
          animTimePos += _frames[frame].Duration;
          keyFrames.Add(keyFrame);
        }
        var animName = $"{name}_{frameTag.Name}";
        var animation = CreateAnimation(animName, keyFrames, animTimePos, bindingType);
        animations.Add(animation);
      }

      return animations;
    }

    private static List<FrameTag> CreateDefaultFrameTags(int count)
      => new List<FrameTag>() {
        new FrameTag("default", 0, count - 1, AnimationDirection.Forward)
      };

    private static ObjectReferenceKeyframe CreateKeyFrame(float animTimePos, Sprite sprite)
    {
      var keyFrame = new ObjectReferenceKeyframe();
      keyFrame.time = animTimePos;
      keyFrame.value = sprite;
      return keyFrame;
    }

    private static AnimationClip CreateAnimation(
      string name,
      IEnumerable<ObjectReferenceKeyframe> keyFrames,
      float stopTime,
      Type bindingType)
    {
      var animation = new AnimationClip()
      {
        name = name
      };
      var spriteBinding = new EditorCurveBinding()
      {
        type = bindingType,
        path = string.Empty,
        propertyName = "m_Sprite",
      };
      AnimationUtility.SetObjectReferenceCurve(animation, spriteBinding, keyFrames.ToArray());
      var animSettings = AnimationUtility.GetAnimationClipSettings(animation);
      animSettings.loopTime = true;
      animSettings.startTime = 0f;
      animSettings.stopTime = stopTime;
      AnimationUtility.SetAnimationClipSettings(animation, animSettings);
      return animation;
    }

    private static Sprite CreateNamedSprite(
      string name,
      Texture2D texture,
      UnityEditor.AssetImporters.SpriteImportData importData,
      AsepriteImporterSettings settings)
    {
      var sprite = Sprite.Create(
        texture,
        importData.rect,
        importData.pivot,
        settings.PixelsPerUnit,
        settings.ExtrudeEdges,
        settings.MeshType,
        importData.border,
        settings.GeneratePhysicsShape);
      sprite.name = name;
      return sprite;
    }

    private static Texture2D CreateBlankSpritesheet(
      AsepriteImporterSettings settings,
      Vector2Int frameSize,
      int frameCount,
      out int cols)
    {
      cols = frameCount;
      int rows = 1;

      var sheetSize = new Vector2Int(cols * frameSize.x, rows * frameSize.y);
      while (cols > 1 && sheetSize.x / 2f > sheetSize.y)
      {
        cols = Mathf.CeilToInt(cols / 2f);
        rows *= 2;
        sheetSize = new Vector2Int(cols * frameSize.x, rows * frameSize.y);
      }

      var powerTwo = NextPowerTwo(Math.Max(sheetSize.x, sheetSize.y));
      var spritesheet = new Texture2D(
        powerTwo,
        powerTwo,
        TextureFormat.RGBA32,
        false);
      spritesheet.Clear(Color.clear);
      spritesheet.filterMode = settings.FilterMode;
      spritesheet.alphaIsTransparency = false;
      spritesheet.wrapMode = settings.WrapMode;
      spritesheet.anisoLevel = settings.AnisoLevel;
      spritesheet.name = "Texture";
      return spritesheet;
    }

    private static int NextPowerTwo(int value)
    {
      // bit fiddling magic - get lowest power 2 value higher than value
      --value;
      value |= value >> 1;
      value |= value >> 2;
      value |= value >> 4;
      value |= value >> 8;
      value |= value >> 16;
      return ++value;
    }
  }
}
