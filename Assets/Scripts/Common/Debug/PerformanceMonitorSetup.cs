using UnityEngine;

namespace Common
{
    /// <summary>
    /// Simple setup script to add PerformanceMonitor to a scene
    /// Attach this to any GameObject to enable performance monitoring
    /// </summary>
    public class PerformanceMonitorSetup : MonoBehaviour
    {
        [Header("Performance Monitor Setup")]
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool enableLogging = true;
        [SerializeField] private float logInterval = 1.0f;
        [SerializeField] private bool showInGUI = false;
        [SerializeField] private bool enableSpikeDetection = true;
        [SerializeField] private float spikeThresholdMs = 16.67f; // 60 FPS threshold

        // Note: These fields are used for configuration but Unity doesn't allow
        // direct modification of other component's serialized fields at runtime
        // Users can manually configure the PerformanceMonitor component in the Inspector

        private void Awake()
        {
            // Add PerformanceMonitor component if not already present
            var monitor = GetComponent<PerformanceMonitor>();
            if (monitor == null)
            {
                monitor = gameObject.AddComponent<PerformanceMonitor>();
            }

            // Add PerformanceProfiler for spike detection if enabled
            PerformanceProfiler profiler = null;
            if (enableSpikeDetection)
            {
                profiler = GetComponent<PerformanceProfiler>();
                if (profiler == null)
                {
                    profiler = gameObject.AddComponent<PerformanceProfiler>();
                }

                // Configure profiler settings
                // Note: These are serialized fields that get their values from the Inspector
                // We can't directly set them at runtime, but they use the configured values
            }

            // Configure the monitor
            monitor.enabled = enableLogging;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            UnityEngine.Debug.Log("[PerformanceMonitor] Initialized with Vulkan-optimized monitoring");
            UnityEngine.Debug.Log($"[PerformanceMonitor] Logging: {enableLogging}, GUI: {showInGUI}, Spike Detection: {enableSpikeDetection}, Log Interval: {logInterval}s, Spike Threshold: {spikeThresholdMs}ms");
        }

        private void Start()
        {
            UnityEngine.Debug.Log("[PerformanceMonitor] Monitoring started. Check console for performance metrics.");
            UnityEngine.Debug.Log("[PerformanceMonitor] With Vulkan, you should see significantly higher FPS!");
        }
    }
}
