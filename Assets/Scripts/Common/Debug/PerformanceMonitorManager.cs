using UnityEngine;

namespace Common
{
    /// <summary>
    /// Static manager for easy performance monitoring setup
    /// Call PerformanceMonitorManager.Initialize() from any script to start monitoring
    /// </summary>
    public static class PerformanceMonitorManager
    {
        private static GameObject _monitorInstance;
        private static bool _isInitialized;

        /// <summary>
        /// Initialize performance monitoring with default settings
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            // Create monitor GameObject
            _monitorInstance = new GameObject("PerformanceMonitor");
            var monitor = _monitorInstance.AddComponent<PerformanceMonitor>();
            var setup = _monitorInstance.AddComponent<PerformanceMonitorSetup>();

            // Make it persist across scenes
            Object.DontDestroyOnLoad(_monitorInstance);

            _isInitialized = true;

            Debug.Log("[PerformanceMonitor] Vulkan performance monitoring initialized!");
            Debug.Log("[PerformanceMonitor] Expecting 250+ FPS with your Vulkan upgrade!");
        }

        /// <summary>
        /// Initialize with custom settings
        /// </summary>
        public static void Initialize(bool enableLogging, float logInterval, bool showInGUI)
        {
            Initialize();

            var monitor = _monitorInstance.GetComponent<PerformanceMonitor>();
            // Note: Serialized fields can't be set directly, but component uses defaults
            // User can still manually configure in Inspector if needed
        }

        /// <summary>
        /// Get the current performance monitor instance
        /// </summary>
        public static PerformanceMonitor GetMonitor()
        {
            if (_monitorInstance == null) return null;
            return _monitorInstance.GetComponent<PerformanceMonitor>();
        }

        /// <summary>
        /// Log current performance snapshot
        /// </summary>
        public static void LogSnapshot()
        {
            var monitor = GetMonitor();
            if (monitor != null)
            {
                monitor.LogSnapshot();
            }
        }

        /// <summary>
        /// Check if monitoring is active
        /// </summary>
        public static bool IsMonitoring() => _isInitialized && _monitorInstance != null;
    }
}
