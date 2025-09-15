using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

namespace Common
{
    /// <summary>
    /// Advanced performance profiler to identify causes of frame rate drops
    /// Use this to diagnose the 52.99ms frame time spikes you experienced
    /// </summary>
    public class PerformanceProfiler : MonoBehaviour
    {
        [Header("Performance Profiling")]
        [SerializeField] private bool enableDetailedProfiling = true;
        [SerializeField] private float spikeThresholdMs = 16.67f; // 60 FPS threshold
        [SerializeField] private int maxSpikeHistory = 10;

        /// <summary>
        /// Performance spike data structure
        /// </summary>
        public struct PerformanceSpike
        {
            public float frameTime;
            public float fps;
            public long memoryAllocated;
            public long memoryReserved;
            public string timestamp;
            public int frameCount;
        }

        private List<PerformanceSpike> _spikeHistory = new List<PerformanceSpike>();
        private Stopwatch _frameTimer = new Stopwatch();
        private int _frameCount;

        private void Start()
        {
            _frameTimer.Start();
            string profilingMode = enableDetailedProfiling ? "Detailed" : "Basic";
            UnityEngine.Debug.Log($"[PerformanceProfiler] {profilingMode} profiling enabled. Monitoring for performance spikes...");
            UnityEngine.Debug.Log($"[PerformanceProfiler] Spike threshold: {spikeThresholdMs}ms, Max history: {maxSpikeHistory}");
        }

        private void Update()
        {
            _frameCount++;
            float frameTime = Time.deltaTime * 1000f; // Convert to milliseconds

            // Detect performance spikes
            if (frameTime > spikeThresholdMs)
            {
                RecordPerformanceSpike(frameTime);
            }

            // Periodic analysis (only if detailed profiling is enabled)
            if (enableDetailedProfiling && _frameCount % 300 == 0) // Every ~5 seconds
            {
                AnalyzePerformance();
            }
        }

        private void RecordPerformanceSpike(float frameTime)
        {
            var spike = new PerformanceSpike
            {
                frameTime = frameTime,
                fps = 1.0f / Time.deltaTime,
                memoryAllocated = Profiler.GetTotalAllocatedMemoryLong(),
                memoryReserved = Profiler.GetTotalReservedMemoryLong(),
                timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff"),
                frameCount = _frameCount
            };

            _spikeHistory.Add(spike);

            // Keep only recent spikes
            if (_spikeHistory.Count > maxSpikeHistory)
            {
                _spikeHistory.RemoveAt(0);
            }

            // Immediate warning for critical spikes
            if (frameTime > 50f) // >50ms = critical
            {
                UnityEngine.Debug.LogError($"[PerformanceProfiler] 🚨 CRITICAL SPIKE DETECTED! {frameTime:F2}ms ({spike.fps:F1} FPS)");
                UnityEngine.Debug.LogError($"[PerformanceProfiler] Memory: {spike.memoryAllocated / 1024 / 1024}MB allocated, {spike.memoryReserved / 1024 / 1024}MB reserved");
                LogPotentialCauses();
            }
            else if (frameTime > 33f) // >33ms = warning
            {
                UnityEngine.Debug.LogWarning($"[PerformanceProfiler] ⚠️ Performance spike: {frameTime:F2}ms ({spike.fps:F1} FPS)");
            }
        }

        private void LogPotentialCauses()
        {
            UnityEngine.Debug.Log("[PerformanceProfiler] 🔍 Potential causes of performance spike:");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Garbage Collection (GC.Alloc spikes)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Asset loading (Resources.Load, AssetBundle loading)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Network operations (Bolt network processing)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Particle system instantiation");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Physics calculations (heavy collision detection)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • UI updates (complex layout recalculations)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Shader compilation (first-time shader usage)");
            UnityEngine.Debug.Log("[PerformanceProfiler] • Scene loading or object instantiation");
        }

        private void AnalyzePerformance()
        {
            if (_spikeHistory.Count == 0) return;

            UnityEngine.Debug.Log($"[PerformanceProfiler] 📊 Performance Analysis (Last {_spikeHistory.Count} spikes):");

            float avgSpikeTime = 0;
            float maxSpikeTime = 0;
            float minSpikeFps = float.MaxValue;

            foreach (var spike in _spikeHistory)
            {
                avgSpikeTime += spike.frameTime;
                if (spike.frameTime > maxSpikeTime) maxSpikeTime = spike.frameTime;
                if (spike.fps < minSpikeFps) minSpikeFps = spike.fps;
            }

            avgSpikeTime /= _spikeHistory.Count;

            UnityEngine.Debug.Log($"[PerformanceProfiler] Average spike time: {avgSpikeTime:F2}ms");
            UnityEngine.Debug.Log($"[PerformanceProfiler] Worst spike: {maxSpikeTime:F2}ms ({minSpikeFps:F1} FPS)");
            UnityEngine.Debug.Log($"[PerformanceProfiler] Spike frequency: {_spikeHistory.Count} in last 5 seconds");

            if (maxSpikeTime > 50f)
            {
                UnityEngine.Debug.LogWarning("[PerformanceProfiler] ⚠️ High frequency of critical spikes detected!");
            }
        }

        private void OnDestroy()
        {
            _frameTimer.Stop();

            if (_spikeHistory.Count > 0)
            {
                UnityEngine.Debug.Log($"[PerformanceProfiler] Session ended. Total spikes recorded: {_spikeHistory.Count}");
                UnityEngine.Debug.Log($"[PerformanceProfiler] Worst spike: {_spikeHistory[_spikeHistory.Count - 1].frameTime:F2}ms");
            }
        }

        /// <summary>
        /// Get the most recent performance spike data
        /// </summary>
        public PerformanceSpike GetLastSpike()
        {
            return _spikeHistory.Count > 0 ? _spikeHistory[_spikeHistory.Count - 1] : default;
        }

        /// <summary>
        /// Get all recorded spikes
        /// </summary>
        public List<PerformanceSpike> GetSpikeHistory()
        {
            return new List<PerformanceSpike>(_spikeHistory);
        }

        /// <summary>
        /// Clear spike history
        /// </summary>
        public void ClearHistory()
        {
            _spikeHistory.Clear();
            UnityEngine.Debug.Log("[PerformanceProfiler] Spike history cleared");
        }
    }
}
