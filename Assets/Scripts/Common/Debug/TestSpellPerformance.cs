using UnityEngine;

namespace Common
{
    /// <summary>
    /// Simple test script to verify SpellPerformanceAnalyzer works correctly
    /// Attach this to any GameObject to test the integration
    /// </summary>
    public class TestSpellPerformance : MonoBehaviour
    {
        private void Start()
        {
            // Test the integration
            SpellPerformanceIntegration.Initialize();

            // Simulate some spell casts
            SpellPerformanceIntegration.StartSpellCast("Fireball");
            Invoke("CompleteFireball", 0.5f);

            SpellPerformanceIntegration.StartSpellCast("FrostBolt");
            Invoke("CompleteFrostBolt", 0.3f);

            SpellPerformanceIntegration.StartSpellCast("Lightning");
            Invoke("CompleteLightning", 0.8f);
        }

        private void CompleteFireball()
        {
            SpellPerformanceIntegration.CompleteSpellCast("Fireball");
            SpellPerformanceIntegration.LogSpellStats("Fireball");
        }

        private void CompleteFrostBolt()
        {
            SpellPerformanceIntegration.CompleteSpellCast("FrostBolt");
            SpellPerformanceIntegration.LogSpellStats("FrostBolt");
        }

        private void CompleteLightning()
        {
            SpellPerformanceIntegration.CompleteSpellCast("Lightning");
            SpellPerformanceIntegration.LogSpellStats("Lightning");
        }

        private void Update()
        {
            // Press T to test logging all stats
            if (Input.GetKeyDown(KeyCode.T))
            {
                var analyzer = FindObjectOfType<SpellPerformanceAnalyzer>();
                if (analyzer != null)
                {
                    analyzer.LogAllSpellStats();
                }
            }

            // Press R to reset stats
            if (Input.GetKeyDown(KeyCode.R))
            {
                var analyzer = FindObjectOfType<SpellPerformanceAnalyzer>();
                if (analyzer != null)
                {
                    analyzer.ResetStats();
                    UnityEngine.Debug.Log("Spell performance stats reset!");
                }
            }
        }
    }
}
