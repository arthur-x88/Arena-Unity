using UnityEngine;

namespace Common
{
    /// <summary>
    /// Example integration script showing how to use SpellPerformanceAnalyzer
    /// Add this to your spell casting system or integrate the calls directly
    /// </summary>
    public static class SpellPerformanceIntegration
    {
        private static SpellPerformanceAnalyzer _analyzer;

        /// <summary>
        /// Initialize the spell performance analyzer
        /// Call this once at game start
        /// </summary>
        public static void Initialize()
        {
            // Create or find the analyzer
            var analyzerObject = GameObject.Find("SpellPerformanceAnalyzer");
            if (analyzerObject == null)
            {
                analyzerObject = new GameObject("SpellPerformanceAnalyzer");
                _analyzer = analyzerObject.AddComponent<SpellPerformanceAnalyzer>();
                Object.DontDestroyOnLoad(analyzerObject);
            }
            else
            {
                _analyzer = analyzerObject.GetComponent<SpellPerformanceAnalyzer>();
            }

            UnityEngine.Debug.Log("[SpellPerformanceIntegration] Spell performance analysis ready!");
        }

        /// <summary>
        /// Call this when starting to cast a spell
        /// </summary>
        public static void StartSpellCast(string spellName)
        {
            if (_analyzer != null)
            {
                _analyzer.OnSpellCastStart(spellName);
            }
        }

        /// <summary>
        /// Call this when spell cast is complete
        /// </summary>
        public static void CompleteSpellCast(string spellName)
        {
            if (_analyzer != null)
            {
                _analyzer.OnSpellCastComplete(spellName);
            }
        }

        /// <summary>
        /// Get performance statistics for a spell
        /// </summary>
        public static void LogSpellStats(string spellName)
        {
            if (_analyzer != null)
            {
                _analyzer.GetSpellStats(spellName);
            }
        }

        /// <summary>
        /// Example usage in your spell casting code:
        ///
        /// void CastSpell(string spellName) {
        ///     // Start performance tracking
        ///     SpellPerformanceIntegration.StartSpellCast(spellName);
        ///
        ///     // Your spell casting logic here
        ///     InstantiateSpellEffect(spellName);
        ///     SendNetworkMessage(spellName);
        ///     PlaySoundEffect(spellName);
        ///
        ///     // End performance tracking
        ///     SpellPerformanceIntegration.CompleteSpellCast(spellName);
        /// }
        /// </summary>
    }
}


