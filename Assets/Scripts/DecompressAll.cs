using UnityEngine;
using UnityEditor;

public class DecompressAll : EditorWindow
{
    [MenuItem("Tools/Disable Compression on All Sprites")]
    static void DisableSpriteCompression()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.textureType == TextureImporterType.Sprite)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
                count++;
            }
        }

        Debug.Log($"Disabled compression on {count} sprite textures!");
    }
}
