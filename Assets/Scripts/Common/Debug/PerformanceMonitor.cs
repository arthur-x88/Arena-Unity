using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

namespace Common
{
    /// <summary>
    /// Performance monitoring utility for tracking FPS, memory usage, and GPU metrics
    /// Especially useful after switching to Vulkan for performance comparison
    /// </summary>
    public class PerformanceMonitor : MonoBehaviour
    {
        [Header("Performance Monitoring")]
        [SerializeField] private bool enableLogging = true;
        [SerializeField] private float logInterval = 1.0f;
        [SerializeField] private bool showInGUI = false;

        private float _lastLogTime;
        private float _fps;
        private float _minFps = float.MaxValue;
        private float _maxFps;
        private float _avgFps;
        private int _frameCount;
        private float _totalFps;

        private void Update()
        {
            // Calculate FPS
            _fps = 1.0f / Time.deltaTime;
            _frameCount++;
            _totalFps += _fps;

            if (_fps < _minFps) _minFps = _fps;
            if (_fps > _maxFps) _maxFps = _fps;

            // Log performance metrics at intervals
            if (enableLogging && Time.time - _lastLogTime >= logInterval)
            {
                LogPerformanceMetrics();
                _lastLogTime = Time.time;
            }
        }

        private void LogPerformanceMetrics()
        {
            _avgFps = _totalFps / _frameCount;

            // Memory usage
            long memoryUsage = Profiler.GetTotalAllocatedMemoryLong();
            long reservedMemory = Profiler.GetTotalReservedMemoryLong();
            long unusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong();

            // GPU time (approximate)
            float gpuTime = Time.deltaTime * 1000f; // Rough estimate in ms

            // Performance analysis
            string performanceStatus = GetPerformanceStatus(_fps);

            // Log to console
            UnityEngine.Debug.Log($"[Performance] FPS: {_fps:F1} | Avg: {_avgFps:F1} | Min: {_minFps:F1} | Max: {_maxFps:F1} | Status: {performanceStatus}");
            UnityEngine.Debug.Log($"[Performance] Memory: {memoryUsage / 1024 / 1024}MB allocated | {reservedMemory / 1024 / 1024}MB reserved | {unusedReservedMemory / 1024 / 1024}MB unused");
            UnityEngine.Debug.Log($"[Performance] GPU Time: ~{gpuTime:F2}ms | Frame Time: {Time.deltaTime * 1000f:F2}ms");

            // Warn about performance issues
            if (_fps < 30f)
            {
                UnityEngine.Debug.LogWarning($"[Performance] CRITICAL: FPS dropped below 30! Frame time: {Time.deltaTime * 1000f:F2}ms");
            }
            else if (_fps < 60f)
            {
                UnityEngine.Debug.LogWarning($"[Performance] WARNING: FPS below 60! Frame time: {Time.deltaTime * 1000f:F2}ms");
            }

            // Reset min/max tracking periodically
            if (_frameCount > 300) // Reset every ~5 seconds at 60fps
            {
                UnityEngine.Debug.Log($"[Performance] Resetting stats - Previous session: Avg {_avgFps:F1} FPS, Range {_minFps:F1}-{_maxFps:F1}");
                _minFps = float.MaxValue;
                _maxFps = 0;
                _totalFps = 0;
                _frameCount = 0;
            }
        }

        private string GetPerformanceStatus(float fps)
        {
            if (fps >= 200f) return "EXCELLENT (Vulkan Working!)";
            if (fps >= 120f) return "VERY GOOD";
            if (fps >= 60f) return "GOOD";
            if (fps >= 30f) return "FAIR";
            return "POOR (Needs Optimization)";
        }

        private void OnGUI()
        {
            if (!showInGUI) return;

            // Display performance metrics on screen
            GUI.Label(new Rect(10, 10, 300, 20), $"FPS: {_fps:F1} (Avg: {_avgFps:F1})");
            GUI.Label(new Rect(10, 30, 300, 20), $"Memory: {Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024}MB");
            GUI.Label(new Rect(10, 50, 300, 20), $"Frame Time: {Time.deltaTime * 1000f:F2}ms");
        }

        /// <summary>
        /// Get current FPS
        /// </summary>
        public float GetCurrentFPS() => _fps;

        /// <summary>
        /// Get average FPS since last reset
        /// </summary>
        public float GetAverageFPS() => _avgFps;

        /// <summary>
        /// Get minimum FPS recorded
        /// </summary>
        public float GetMinFPS() => _minFps;

        /// <summary>
        /// Get maximum FPS recorded
        /// </summary>
        public float GetMaxFPS() => _maxFps;

        /// <summary>
        /// Reset FPS statistics
        /// </summary>
        public void ResetStats()
        {
            _minFps = float.MaxValue;
            _maxFps = 0;
            _totalFps = 0;
            _frameCount = 0;
            _avgFps = 0;
        }

        /// <summary>
        /// Log current performance snapshot
        /// </summary>
        public void LogSnapshot()
        {
            LogPerformanceMetrics();
        }
    }
}
