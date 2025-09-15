using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Analyzes spell performance metrics for optimization and debugging
    /// Tracks casting times, success rates, and performance bottlenecks
    /// </summary>
    public class SpellPerformanceAnalyzer : MonoBehaviour
    {
        /// <summary>
        /// Data structure for spell performance statistics
        /// </summary>
        public struct SpellStats
        {
            public string spellName;
            public int totalCasts;
            public int successfulCasts;
            public float averageCastTime;
            public float minCastTime;
            public float maxCastTime;
            public float totalTimeSpent;
            public float lastCastTime;

            public float SuccessRate => totalCasts > 0 ? (float)successfulCasts / totalCasts * 100f : 0f;
        }

        [Header("Spell Performance Analysis")]
        [SerializeField] private bool enableLogging = true;
        [SerializeField] private float performanceThresholdMs = 100f; // Flag spells taking longer than this

        private Dictionary<string, SpellStats> _spellStats = new Dictionary<string, SpellStats>();
        private Dictionary<string, Stopwatch> _activeCasts = new Dictionary<string, Stopwatch>();
        private Dictionary<string, int> _activeCastCount = new Dictionary<string, int>();

        private void Awake()
        {
            UnityEngine.Debug.Log("[SpellPerformanceAnalyzer] Initialized spell performance analysis");
        }

        /// <summary>
        /// Start tracking a spell cast
        /// </summary>
        public void OnSpellCastStart(string spellName)
        {
            // Handle multiple simultaneous casts of the same spell
            if (!_activeCastCount.ContainsKey(spellName))
                _activeCastCount[spellName] = 0;

            _activeCastCount[spellName]++;

            // Only create new stopwatch for first cast of this spell
            if (_activeCastCount[spellName] == 1)
            {
                if (!_activeCasts.ContainsKey(spellName))
                    _activeCasts[spellName] = new Stopwatch();

                _activeCasts[spellName].Restart();
            }

            if (enableLogging)
                UnityEngine.Debug.Log($"[SpellPerf] Started casting: {spellName}");
        }

        /// <summary>
        /// Complete tracking of a spell cast
        /// </summary>
        public void OnSpellCastComplete(string spellName)
        {
            if (!_activeCastCount.ContainsKey(spellName) || _activeCastCount[spellName] <= 0)
            {
                UnityEngine.Debug.LogWarning($"[SpellPerf] WARNING: OnSpellCastComplete called for {spellName} but no active cast found!");
                return;
            }

            _activeCastCount[spellName]--;

            // Only record stats when all casts of this spell are complete
            if (_activeCastCount[spellName] == 0 && _activeCasts.ContainsKey(spellName))
            {
                var stopwatch = _activeCasts[spellName];
                stopwatch.Stop();

                float castTimeMs = (float)stopwatch.Elapsed.TotalMilliseconds;
                bool isSuccessful = true; // You can modify this based on your spell success logic

                RecordSpellCast(spellName, castTimeMs, isSuccessful);

                if (enableLogging)
                {
                    string performance = castTimeMs > performanceThresholdMs ? "⚠️ SLOW" : "✅ FAST";
                    UnityEngine.Debug.Log($"[SpellPerf] Completed: {spellName} ({castTimeMs:F2}ms) {performance}");
                }
            }
        }

        /// <summary>
        /// Record spell cast statistics
        /// </summary>
        private void RecordSpellCast(string spellName, float castTimeMs, bool isSuccessful)
        {
            if (!_spellStats.ContainsKey(spellName))
            {
                _spellStats[spellName] = new SpellStats
                {
                    spellName = spellName,
                    minCastTime = float.MaxValue
                };
            }

            var stats = _spellStats[spellName];
            stats.totalCasts++;
            stats.totalTimeSpent += castTimeMs;
            stats.averageCastTime = stats.totalTimeSpent / stats.totalCasts;
            stats.lastCastTime = castTimeMs;

            if (castTimeMs < stats.minCastTime)
                stats.minCastTime = castTimeMs;

            if (castTimeMs > stats.maxCastTime)
                stats.maxCastTime = castTimeMs;

            if (isSuccessful)
                stats.successfulCasts++;

            _spellStats[spellName] = stats;

            // Warn about performance issues
            if (castTimeMs > performanceThresholdMs)
            {
                UnityEngine.Debug.LogWarning($"[SpellPerf] ⚠️ SLOW SPELL: {spellName} took {castTimeMs:F2}ms (threshold: {performanceThresholdMs}ms)");
            }
        }

        /// <summary>
        /// Get performance statistics for a specific spell
        /// </summary>
        public void GetSpellStats(string spellName)
        {
            if (_spellStats.ContainsKey(spellName))
            {
                var stats = _spellStats[spellName];
                UnityEngine.Debug.Log($"[SpellPerf] 📊 Stats for {spellName}:");
                UnityEngine.Debug.Log($"[SpellPerf] Total casts: {stats.totalCasts}");
                UnityEngine.Debug.Log($"[SpellPerf] Success rate: {stats.SuccessRate:F1}%");
                UnityEngine.Debug.Log($"[SpellPerf] Average time: {stats.averageCastTime:F2}ms");
                UnityEngine.Debug.Log($"[SpellPerf] Fastest: {stats.minCastTime:F2}ms");
                UnityEngine.Debug.Log($"[SpellPerf] Slowest: {stats.maxCastTime:F2}ms");
                UnityEngine.Debug.Log($"[SpellPerf] Total time spent: {stats.totalTimeSpent:F2}ms");
                UnityEngine.Debug.Log($"[SpellPerf] Last cast: {stats.lastCastTime:F2}ms");
            }
            else
            {
                UnityEngine.Debug.Log($"[SpellPerf] No stats available for: {spellName}");
            }
        }

        /// <summary>
        /// Get performance statistics for all spells
        /// </summary>
        public void LogAllSpellStats()
        {
            UnityEngine.Debug.Log("[SpellPerf] 📊 PERFORMANCE SUMMARY:");

            if (_spellStats.Count == 0)
            {
                UnityEngine.Debug.Log("[SpellPerf] No spell casts recorded yet.");
                return;
            }

            foreach (var kvp in _spellStats)
            {
                var stats = kvp.Value;
                string performance = stats.averageCastTime > performanceThresholdMs ? "⚠️" : "✅";
                UnityEngine.Debug.Log($"[SpellPerf] {performance} {stats.spellName}: {stats.averageCastTime:F2}ms avg ({stats.totalCasts} casts, {stats.SuccessRate:F1}% success)");
            }
        }

        /// <summary>
        /// Reset all performance statistics
        /// </summary>
        public void ResetStats()
        {
            _spellStats.Clear();
            _activeCasts.Clear();
            _activeCastCount.Clear();
            UnityEngine.Debug.Log("[SpellPerf] All statistics reset");
        }

        /// <summary>
        /// Get raw spell statistics data
        /// </summary>
        public Dictionary<string, SpellStats> GetAllStats()
        {
            return new Dictionary<string, SpellStats>(_spellStats);
        }

        /// <summary>
        /// Get statistics for a specific spell
        /// </summary>
        public SpellStats GetStats(string spellName)
        {
            return _spellStats.ContainsKey(spellName) ? _spellStats[spellName] : default;
        }
    }
}
