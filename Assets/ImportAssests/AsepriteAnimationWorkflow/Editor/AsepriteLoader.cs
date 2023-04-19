using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace APIShift.AsepriteAnimationWorkflow
{
  public class AsepriteLoader
  {
    private const int ChunkHeaderSize = 6;

    private Header _header;
    private IEnumerable<Frame> _frames;
    private IList<Layer> _layers;
    private IndexedPalette _palette;

    public Aseprite LoadFile(string filePath)
    {
      using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      {
        var reader = new BinaryReader(stream);
        var header = ReadHeader(reader);
        var frames = new List<Frame>(header.FrameCount);
        var layers = new List<Layer>();
        var frameTags = new List<FrameTag>();

        IndexedPalette palette = null;
        var colorReader = GetColorReader(header.ColorDepth, () => palette);
        for (var frameIndex = 0; frameIndex < header.FrameCount; ++frameIndex)
        {
          // FRAME
          reader.ReadUInt32(); // Length
          reader.ReadUInt16(); // Magic number
          var oldChunkCount = reader.ReadUInt16();
          var frameDuration = reader.ReadUInt16();
          reader.ReadBytes(2); // For future use
          var newChunkCount = reader.ReadUInt32();
          uint actualChunkCount = newChunkCount > 0 ? newChunkCount : oldChunkCount;

          var cels = new List<Cel>((int)actualChunkCount);
          for (int chunkIndex = 0; chunkIndex < actualChunkCount; ++chunkIndex)
          {
            var chunkLength = reader.ReadUInt32();
            var chunkType = (ChunkType)reader.ReadUInt16();
            switch (chunkType)
            {
              case ChunkType.Cel:
                var cel = ReadCel(reader, frames, layers, colorReader, chunkLength);
                cels.Add(cel);
                break;
              case ChunkType.Layer:
                var layer = ReadLayer(reader, layers);
                layers.Add(layer);
                break;
              case ChunkType.FrameTags:
                frameTags = ReadFrameTags(reader);
                break;
              case ChunkType.Palette:
                palette = ReadIndexedPalette(reader, header.TransparentColorIndex);
                break;
              default:
                // Discard chunk
                reader.BaseStream.Position += chunkLength - ChunkHeaderSize;
                break;
            }
          }
          var frame = new Frame(cels, frameDuration / 1000f);
          frames.Add(frame);
        }
        return new Aseprite(header.FrameSize, frames, frameTags);
      }
    }

    private static Header ReadHeader(BinaryReader reader)
    {
      reader.ReadUInt32(); // File size
      reader.ReadUInt16(); // Magic number
      var frameCount = reader.ReadUInt16();
      var width = reader.ReadUInt16();
      var height = reader.ReadUInt16();
      var colorDepth = (ColorDepth)reader.ReadUInt16();
      reader.ReadUInt32(); // Flags
      reader.ReadUInt16(); // Deprecated (speed)
      reader.ReadUInt32(); // Ignore
      reader.ReadUInt32(); // Ignore
      var transparentColorIndex = reader.ReadByte();
      reader.ReadBytes(3); // Ignore
      reader.ReadUInt16(); // Color count
      reader.ReadByte(); // Pixel width
      reader.ReadByte(); // Pixel height
      reader.ReadBytes(92); // For future use

      return new Header(
        new Vector2Int(width, height),
        colorDepth,
        frameCount,
        transparentColorIndex);
    }

    private static List<FrameTag> ReadFrameTags(BinaryReader reader)
    {
      var tagCount = reader.ReadUInt16();
      reader.ReadBytes(8); // For future use
      return Enumerable
            .Range(0, tagCount)
            .Select(i => ReadFrameTag(reader))
            .ToList();
    }

    private static FrameTag ReadFrameTag(BinaryReader reader)
    {
      var frameFrom = reader.ReadUInt16();
      var frameTo = reader.ReadUInt16();
      var direction = (AnimationDirection)reader.ReadByte();
      reader.ReadBytes(8); // For future use
      reader.ReadBytes(3); // Tag color
      reader.ReadByte(); // Extra byte
      var nameLength = reader.ReadUInt16();
      var name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));

      return new FrameTag(name, frameFrom, frameTo, direction);
    }

    private static IndexedPalette ReadIndexedPalette(
      BinaryReader reader,
      byte transparentIndex)
    {
      var paletteSize = reader.ReadUInt32();
      var startIndex = reader.ReadUInt32();
      var endIndex = reader.ReadUInt32();
      reader.ReadBytes(8); // For future use
      var colors = Enumerable
                  .Range(0, (int)paletteSize)
                  .Select(i =>
                  {
                    var flags = reader.ReadUInt16();
                    var r = reader.ReadByte();
                    var g = reader.ReadByte();
                    var b = reader.ReadByte();
                    var a = reader.ReadByte();
                    if ((flags & 1) != 0)
                    {
                      reader.ReadString(); // Strip unused color name bytes
                    }
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                  })
                  .ToList();
      return new IndexedPalette(
        colors,
        startIndex,
        endIndex,
        transparentIndex);
    }

    private static Layer ReadLayer(BinaryReader reader, IList<Layer> layers)
    {
      var flags = reader.ReadUInt16();
      var layerType = (LayerType)reader.ReadUInt16();
      var childLevel = reader.ReadUInt16();
      reader.ReadUInt16(); // Default layer width
      reader.ReadUInt16(); // Default layer height
      var blendMode = (BlendMode)reader.ReadUInt16();
      var layerOpacity = reader.ReadByte() / 255f;
      reader.ReadBytes(3); // For future use
      reader.ReadBytes(reader.ReadUInt16()); // Layer name
      var parent = layers.LastOrDefault(l => l.ChildLevel < childLevel);
      var visible = flags % 2 == 1 && (parent == null || parent.Visible);
      return new Layer(layerType, blendMode, layerOpacity, visible, childLevel);
    }

    private static Cel ReadCel(
      BinaryReader reader,
      IList<Frame> frames,
      IList<Layer> layers,
      Func<BinaryReader, Color> colorReader,
      uint length)
    {
      var layerIndex = reader.ReadUInt16();
      var x = reader.ReadInt16();
      var y = reader.ReadInt16();
      var celOpacity = reader.ReadByte() / 255f;
      var celType = (CelType)reader.ReadUInt16();
      reader.ReadBytes(7); // For future use

      switch (celType)
      {
        case CelType.LinkedCel:
          var linkedFrame = reader.ReadUInt16();
          return frames[linkedFrame].Cels[layerIndex];
        case CelType.RawCel:
        case CelType.CompressedImage:
          var width = reader.ReadUInt16();
          var height = reader.ReadUInt16();
          Color[] pixels;
          if (celType == CelType.RawCel)
          {
            pixels = ReadCelPixels(reader, colorReader, width, height);
          }
          else
          {
            reader.ReadBytes(2);
            var compressedSize = (int)(length - 22) - ChunkHeaderSize;
            var compressedBytes = reader.ReadBytes(compressedSize);
            using (var compressedStream = new MemoryStream(compressedBytes))
            {
              using (var uncompressedReader = new BinaryReader(compressedStream.GzipDeflate()))
              {
                pixels = ReadCelPixels(uncompressedReader, colorReader, width, height);
              }
            }
          }
          return new Cel(layers[layerIndex], pixels, x, y, width, height, celOpacity);
        default:
          throw new NotSupportedException($"Cel Type: {celType} not supported. Is the file corrupted?");
      }
    }

    private static Color[] ReadCelPixels(
      BinaryReader reader,
      Func<BinaryReader, Color> colorReader,
      int width,
      int height)
    => Enumerable
      .Range(0, width * height)
      .Select(i => colorReader(reader))
      .ToArray();

    private static Func<BinaryReader, Color> GetColorReader(
      ColorDepth colorDepth,
      Func<IndexedPalette> paletteSelector)
    {
      switch (colorDepth)
      {
        case ColorDepth.RGBA:
          return reader =>
          {
            var b = reader.ReadBytes(4);
            return new Color(b[0] / 255f, b[1] / 255f, b[2] / 255f, b[3] / 255f);
          };
        case ColorDepth.Grayscale:
          return reader =>
          {
            var b = reader.ReadBytes(2);
            return new Color(b[0] / 255f, b[0] / 255f, b[0] / 255f, b[1] / 255f);
          };
        case ColorDepth.Indexed:
          return reader =>
          {
            var b = reader.ReadByte();
            return paletteSelector()?.GetColor(b) ?? Color.clear;
          };
        default:
          throw new NotSupportedException($"Color Depth: {colorDepth} not supported. Is the file corrupted?");
      }
    }
  }
}
