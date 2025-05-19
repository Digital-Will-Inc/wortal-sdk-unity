using System.Collections.Generic;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Configuration for the LazyLoadManager system
    /// </summary>
    [CreateAssetMenu(fileName = "LazyLoadConfig", menuName = "Wortal/Lazy Load Config")]
    public class LazyLoadConfig : ScriptableObject
    {
        /// <summary>
        /// Whether to enable lazy loading for assets
        /// </summary>
        [Tooltip("Enable lazy loading for assets")]
        public bool enableLazyLoad = false;

        /// <summary>
        /// Maximum allowed size in MB to preload on startup 
        /// </summary>
        [Tooltip("Maximum total size (in MB) of assets to preload at startup")]
        public int preloadBudgetMB = 20;

        /// <summary>
        /// Whether to prewarm assets by instantiating them once
        /// </summary>
        [Tooltip("If enabled, assets will be instantiated once to prewarm them")]
        public bool enableAssetPrewarm = false;

        /// <summary>
        /// List of asset groups with their priorities
        /// </summary>
        [Tooltip("Groups of assets to manage with their respective priorities")]
        public List<AssetPriorityGroup> assetPriorityGroups = new();
    }
}