# Examples — Respawn & Recall

Respawn timer (0.1)
- Base 10s + 1s per elapsed minute; clamp at 25s

Illustrative formula
```csharp path=null start=null
int RespawnSeconds(float t) {
  int minutes = Mathf.FloorToInt(t / 60f);
  return Mathf.Clamp(10 + minutes, 10, 25);
}
```

Recall (spell-based)
- Spell: Recall Spell.asset
- CastTime: 6000 ms; GlobalCooldown: standard (1500 ms)
- AttributesExtra: CastableOnlyNonShapeShifted (optional); DoesNotTriggerGcd if desired
- On damage/movement: cancel cast (aura interrupt flags or explicit check)
- On success: EffectTeleportDirect → to base spawn

UX
- Recall button on HUD; disabled while moving/in combat if you prefer stricter rules
