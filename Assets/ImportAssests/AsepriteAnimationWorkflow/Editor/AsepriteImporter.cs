using System.IO;


namespace APIShift.AsepriteAnimationWorkflow
{
  [UnityEditor.AssetImporters.ScriptedImporter(1, new[] { "aseprite", "ase" })]
  public class AsepriteImporter : UnityEditor.AssetImporters.ScriptedImporter
  {
    public AsepriteImporterSettings Settings = new AsepriteImporterSettings();

    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
      var loader = new AsepriteLoader();
      var file = loader.LoadFile(ctx.assetPath);
      var name = Path.GetFileNameWithoutExtension(ctx.assetPath);
      var assets = file.CreateAssets(Settings, name);
      assets.AddToContext(ctx);
    }
  }
}