using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Priority levels for asset loading order
    /// </summary>
    public enum LoadPriority { High, Medium, Low }


    /// <summary>
    /// Represents a list of asset paths categorized by their type
    /// for serialization, because Unity doesn't serialize dictionaries.
    /// </summary>
    [System.Serializable]
    public class AssetTypeList
    {
        public string typeName;
        public List<string> assetPaths = new();
    }


    /// <summary>
    /// Represents a logical group of assets that should be loaded with the same priority
    /// </summary>
    [System.Serializable]
    public class AssetPriorityGroup
    {
        /// <summary>
        /// The name of this asset group
        /// </summary>
        public string name;

        /// <summary>
        /// The loading priority of this group - higher priority groups are loaded first
        /// </summary>
        public LoadPriority priority;

        /// <summary>
        /// List of asset types and their paths for serialization
        /// </summary>
        public List<AssetTypeList> assetsByTypeList = new();

        /// <summary>
        /// Dictionary to categorize assets by type for better organization and filtering
        /// </summary>
        [System.NonSerialized]
        public Dictionary<string, List<string>> assetsByType = new();

        /// <summary>
        /// Estimated size of all assets in this group in megabytes
        /// </summary>
        public float estimatedSizeMB;

        /// <summary>
        /// Indicates whether this group should be loaded on startup
        /// </summary>
        public bool loadOnStartup = true;

        /// <summary>
        /// Total count of assets in this group across all types
        /// </summary>
        public int assetsCount
        {
            get
            {
                int total = 0;
                //to avoid a null reference exception
                if (assetsByType == null)
                    return total;
                // Iterate through each type and sum the counts
                foreach (var kv in assetsByType.Values)
                {
                    total += kv.Count;
                }
                return total;
            }
        }

        /// <summary>
        /// Creates a new asset priority group
        /// </summary>
        /// <param name="name">The name of the group</param>
        /// <param name="priority">The loading priority</param>
        public AssetPriorityGroup(string name, LoadPriority priority)
        {
            this.name = name;
            this.priority = priority;
            this.assetsByType = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Updates the asset count and estimated size by checking actual file sizes
        /// </summary>
        public void UpdateCountAndSize()
        {
            long sizeBytes = 0;
            foreach (var typeList in assetsByType.Values)
            {
                foreach (var path in typeList)
                {
                    if (string.IsNullOrEmpty(path))
                        continue;

                    try
                    {
                        // Handle both addressable paths and direct file paths
                        if (System.IO.File.Exists(path))
                        {
                            var fileInfo = new FileInfo(path);
                            sizeBytes += fileInfo.Length;
                        }
                        else
                        {
                            // For addressable paths, use an estimation
                            // This is an approximation and will be replaced with actual size after loading
                            sizeBytes += 512 * 1024; // 512 KB estimation per asset
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"[AssetPriorityGroup] Error checking size for {path}: {e.Message}");
                    }
                }
            }
            estimatedSizeMB = sizeBytes / (1024f * 1024f);
        }

        /// <summary>
        /// Rebuilds the dictionary from the serialized list of asset types
        /// Call this after loading config to rebuild the runtime dictionary from the serialized list
        /// </summary>
        public void RebuildDictionary()
        {
            assetsByType = new Dictionary<string, List<string>>();
            foreach (var item in assetsByTypeList)
            {
                if (!assetsByType.ContainsKey(item.typeName))
                    assetsByType[item.typeName] = new List<string>();

                assetsByType[item.typeName].AddRange(item.assetPaths);
            }
        }

        /// <summary>
        /// Converts the dictionary to a serializable list for saving
        /// Call this before saving the config to convert the runtime dictionary to a serializable list
        /// </summary>
        public void SyncToSerializableList()
        {
            assetsByTypeList = assetsByType
                .Select(kvp => new AssetTypeList { typeName = kvp.Key, assetPaths = new List<string>(kvp.Value) })
                .ToList();
        }
    }
}