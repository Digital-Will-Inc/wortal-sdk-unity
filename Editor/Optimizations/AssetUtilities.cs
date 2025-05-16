using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.WortalEditor.Optimizations
{
    // Extension class to provide useful metrics for assets
    public static class AssetExtensions
    {
        public static long GetFileSize(this string path)
        {
            if (File.Exists(path))
            {
                return new FileInfo(path).Length;
            }
            return 0;
        }

        public static string FormatSize(this long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < suffixes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {suffixes[order]}";
        }
    }

    // Container class for unused asset information
    [Serializable]
    public class UnusedAssetInfo
    {
        public string path;
        public string filename;
        public string extension;
        public long size;
        public AssetType type;

        public UnusedAssetInfo(string assetPath)
        {
            path = assetPath;
            filename = Path.GetFileName(assetPath);
            extension = Path.GetExtension(assetPath).ToLowerInvariant();
            size = assetPath.GetFileSize();
            type = DetermineAssetType(extension);
        }

        private AssetType DetermineAssetType(string ext)
        {
            switch (ext)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".tga":
                case ".psd":
                case ".tif":
                case ".tiff":
                    return AssetType.Textures;
                case ".fbx":
                case ".obj":
                case ".blend":
                    return AssetType.Models;
                case ".wav":
                case ".mp3":
                case ".ogg":
                    return AssetType.Audio;
                case ".prefab":
                    return AssetType.Prefabs;
                case ".mat":
                    return AssetType.Materials;
                case ".shader":
                    return AssetType.Shaders;
                case ".anim":
                case ".controller":
                    return AssetType.Animations;
                case ".asset":
                    return AssetType.ScriptableObjects;
                case ".ttf":
                case ".otf":
                    return AssetType.Fonts;
                default:
                    return AssetType.Other;
            }
        }
    }

    // Enum definitions for filtering and sorting
    public enum AssetType
    {
        All,
        Textures,
        Models,
        Audio,
        Prefabs,
        Materials,
        Shaders,
        Animations,
        ScriptableObjects,
        Fonts,
        Other
    }

    public enum SortMethod
    {
        Path,
        Size,
        Type,
        Name
    }
}
