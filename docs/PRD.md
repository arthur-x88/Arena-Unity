# Arena Clash — Gameplay Flow PRD (Unity 2019 LTS)

**Doc owner:** You • **Version:** v0.1 • **Last updated:** 2025‑09‑13

---

## 0) TL;DR

A fast, role‑less, skill‑forward arena brawler (2v2 default, 3v3 party mode). Each hero ships with **4 fixed abilities + 1 ultimate** (cooldown‑only economy). **Rounds target 90–120s**, best‑of‑5 sets (full match ≈ 8–12 minutes). **No items/talents/gear**; only cosmetics.

**Unity 2019 LTS** implementation emphasis: ScriptableObject‑driven data, server‑authoritative netcode (Mirror recommended; Photon PUN 2 acceptable), Addressables for content, deterministic ability timing, and light analytics.

---

## 1) Goals & Success Criteria

**Design Goals**

1. <20s median queue, <200ms p95 end‑to‑end input→impact.
2. 90–120s round p50, **TTK 8–12s** under focus fire.
3. D1 retention >40%, W1 >20%; median 6 rounds/session.

**Non‑Goals**

* No PvE, no persistent power progression, no open‑world.

---

## 2) Player Pillars

* **Instant action:** one‑click requeue, bot backfill for leavers.
* **Clarity > complexity:** readable abilities, limited CC chains, consistent DR rules.
* **Agency & mastery:** dodge i‑frames, LOS play, predictable cooldown trades.

---

## 3) Platforms & Performance Targets

* **Phase 1:** Windows PC (KBM + controller).
* **Target:** 60 FPS @ 1080p on GTX 1060 / RX 580 class.
* **Unity:** 2019.4 LTS. Packages: Addressables, Post‑Processing v2, (optional) New Input System 1.x.

---

## 4) Game Modes

* **Ranked 2v2** (default SBMM), **Unranked 2v2**, **Party 3v3 (Unranked)**, **Practice vs Bots**.
* Custom lobbies (invite‑only) unlocked after tutorial.

---

## 5) High‑Level Gameplay Loop

1. **Boot → Title** (auth, settings cache)
2. **Main Menu** (Play, Heroes, Locker, Settings)
3. **Queue (SBMM)** → **Match Found**
4. **Loading** (map + hero data streamed via Addressables)
5. **Pre‑Round (10s):** spawn, ability preview, team ping
6. **Round Live (90–120s target):** fight → **Dampening** begins at t=60s
7. **Sudden Death (optional):** hard pressure at t=120s
8. **Round End:** score update, 10s recap
9. **Repeat** until one team reaches 3 rounds
10. **Post‑Match:** rewards, rank delta, one‑tap requeue

---

## 6) Match Flow (Detailed)

**State enum (authoritative on server):**

* `Matchmaking → Loading → HeroLock → PreRound → RoundLive → RoundEnd → MatchEnd`

**Timings (tunable):

* HeroLock: 8s (first round only; later rounds 5s). Includes 1‑of‑2 Hero Facet selection in Round 1 (see Facets) to lightly fork playstyle without in‑round complexity.
* PreRound: 10s (spawn, camera fly‑in, tips)
* RoundLive: soft cap 120s; Dampening starts at 60s (+2% dmg taken every 5s)
* RoundEnd: 10s (recap + next round interstitial)

**Win Condition:** eliminate all opponents (no respawns). If timer reaches 120s + 30s overtime and both teams alive, closest to center objective wins (or highest cumulative damage as fallback tie‑break).

---

## 7) Combat Model

**Camera:** high‑angle third‑person / isometric (≈ 45° tilt), fixed zoom tiers (close/medium), collision‑aware camera.

**Movement:**

* **Base (WoW‑style):** WASD with A/D as strafe by default; hold Right Mouse to turn character and camera together; Left+Right Mouse to run forward. Mouse‑look + RMB governs facing for ability direction/LOS. This preserves the familiar arena feel.
* **Controller:** twin‑stick (left move / right aim) maintained for gamepad parity.
* **No universal dash.** Movement bursts (Blink/Charge/etc.) are hero abilities, not baseline. Jump is cosmetic only; no sprint.

**Stats Baseline:**

* Health: 1000 (tank archetypes up to 1150; skirmishers 900).
* Self‑sustain: light only (e.g., 60 HPS channeled, interruptible).
* DPS budget (focus fire): \~120–150 DPS per hero.

**TTK Budgeting:** two attackers focusing a 1000 HP target should secure a kill in \~8–12s if the target **fails** a defensive trade (no dash/deflect/LOS). With good play, fights extend to 60–90s pre‑dampening.

**Damage Types:** direct, DoT, AoE; all numeric values normalized to server tick (20Hz authoritative, client interp 60Hz).

**Crowd Control & DR:**

* Categories: **Stun, Root, Silence, Knockback, Soft (slow/disorient)**.
* DR per category: 1st full, 2nd 50%, 3rd 25%, then immune for 15s.
* Max hard‑CC chain on a target: 3s effective without trinket‑style break.

**Dampening / Sudden Death:**

* T=60s: apply **+2% damage taken every 5s** (stacking).
* T=120s: spawn shrinking safe zone (arena ring constricts by 2m every 5s, min radius 6m).

---

## 8) Abilities System (ScriptableObjects)

### Facets (pre‑round)
- Each hero defines two mutually exclusive Facets (small, readable modifiers) chosen during HeroLock in Round 1 only. Examples: “Blink +2m range” vs “Dash grants 10% shield for 1s”. No mid‑round respec, no talent trees.

**Principles:** deterministic timing, server‑auth outcomes, client‑predicted start (FX/sound), rollback on reject.

**Data Objects**

* `HeroDefinition` (SO): id, name, role hint, base stats, ability set.
* `HeroFacet` (SO): id, description, numeric deltas, mutually exclusive pairs per hero.
* `AbilityDefinition` (SO): id, icon, cooldown, cast time, projectile archetype, hitbox, tags (CC category, mobility, defensive), damage curve, VFX/SFX refs.
* `StatusEffect` (SO): id, category, duration, stacking rules, DR flag.

**Runtime Components**

* `AbilityController` (per hero): input mapping, cooldown timers, cast validation (LOS, range, resource).
* `HitResolverServer`: sweeps/casts, team filters, lag compensation window (100ms default).
* `StatusController`: DR tracker per category, cleansers.

**Example (C# POCO, simplified)**

```csharp
[CreateAssetMenu(menuName="Defs/Ability")]
public class AbilityDefinition : ScriptableObject {
    public string AbilityId;
    public Sprite Icon;
    public float Cooldown;
    public float CastTime;
    public float Range;
    public DamageSpec Damage;
    public ProjectileSpec Projectile; // null for instant
    public CCSpec CrowdControl; // null if none
    public List<string> Tags; // "mobility", "defensive", etc.
}
```

---

## 9) Example Hero Kits (v0.1)

**Skirmisher — “Blinkblade”**

* A1 *Quick Slash*: 80 dmg, 0.2s cast, 4s CD.
* A2 *Blink*: 6m dash, i‑frame 8f, 6s CD.
* A3 *Smoke Veil*: 1.5s 50% damage reduction, 12s CD.
* A4 *Grapple*: 10m skillshot, 0.3s stun on hit, 10s CD.
* **ULT *Blade Storm***: 300 dmg over 3s, self‑root, immune to soft CC, 45s CD.

**Control — "Bastion Arcanist"**

* A1 *Arc Bolt*: 70 dmg, chain 2 targets, 5s CD.
* A2 *Barrier Dome*: 3m radius, blocks projectiles 2s, 14s CD.
* A3 *Rune Bind*: 1.5s root, 12s CD (DR: Root).
* A4 *Dispel Wave*: cleanse ally + remove soft CCs in 4m, 15s CD.
* **ULT *Arc Nova***: 200 dmg + 0.75s stun in 4m, 50s CD.

**Bruiser — "Iron Vanguard"**

* A1 *Hammer Slam*: 100 dmg cone, 6s CD.
* A2 *Fortify*: 30% damage reduction 2s, 12s CD.
* A3 *Charge*: 8m rush, 0.5s knockback, 10s CD.
* A4 *Taunting Roar*: 30% slow 2s (soft CC), 10s CD.
* **ULT *Bulwark Stand***: 4s 50% DR + reflect 20% projectiles, 55s CD.

---

## 10) Networking Model

- "Micro‑tick hits": client sends input timestamps; server performs rewind (100ms window) for hit resolution to reduce “I shot first” disputes while remaining fully authoritative.

* **Authoritative server** (headless Unity + Mirror). Client sends input intents; server validates casts, resolves hits; client predicts start FX and reconciles.
* **Tick:** 20Hz server; client interp to 60Hz. **Lag compensation:** rewind colliders to past 100ms for projectile/hitscan.
* **Anti‑cheat surface:** no client health/position authority; checksum ability CDs; server clocks the DR and dampening.
* **Alternative (fastest to market):** Photon PUN 2 (rooms ≤6). Less control over auth; use for prototypes only.

**Key RPCs / Messages**

* `CmdCastAbility(abilityId, targetVector, timestamp)`
* `RpcAbilityStarted(entityId, abilityId, serverTime)`
* `RpcAbilityResolved(entityId, abilityId, hitResults[])`
* `RpcStatusApplied(entityId, statusId, duration)`

---

## 11) Scenes, Prefabs, and Folder Layout

### Systemic Arena Features (rotating, optional)
- Watcher Pad: interactable grants a short vision pulse (e.g., 3s wall‑sense) with a round‑limited cooldown.
- Twin Pad: paired telepads that allow one fast‑rotate per round per player.
- Reactive Shield Field (headline): a visible field that deflects projectiles and visibly deforms for ~0.6–1.0s on impact; shared state across clients for cinematic consistency.

Keep these simple and readable; objectives are optional and must not overshadow the duel.

**Scenes**

* `Boot` → `Title` → `MainMenu` → `Matchmaking` → `Arena_Map01` (additive subscenes: Nav, Props, Lighting, Spawns) → `PostMatch`.

**Prefabs**

* `Hero_[Name]` (model, Animator, AbilityController, StatusController)
* `Projectile_[Archetype]` (pooled)
* `AbilityVFX_[Id]` (Addressables)
* `UI_HUD`, `UI_Scoreboard`, `UI_PostMatch`

**Folders**

* `Assets/_Project/Art|Audio|Code|Prefabs|Scenes|ScriptableObjects|UI`
* `Assets/_Project/ScriptableObjects/Heroes|Abilities|Statuses|Maps`

---

## 12) Input & Controls

**Keyboard/Mouse**

* Move: WASD • Aim: mouse • Abilities: Q/E/R/F • Ultimate: Space • Dash: Shift • Ping: Middle‑mouse

**Controller**

* Move: LS • Aim: RS • Abilities: X/Y/B/RB • Ultimate: LB+RB • Dash: A • Ping: D‑Pad Up
* **Aim Assist:** radial slowdown near valid targets; toggle in settings.

---

## 13) UI/UX Flow

**HUD**

* Bottom‑center ability bar w/ numeric cooldowns, ult meter.
* Left: health bar + status pips (DR stack icons by category).
* Top: round score (best‑of‑5), timer, dampening icon + %.

**End‑of‑Round Card**

* Damage dealt/taken, CC uptime, ability trades ("You dashed 0.2s before Nova").

**Post‑Match**

* Rank stars delta (visible), hidden MMR adjusted; cosmetics progress; one‑tap requeue.

---

## 14) Matchmaking & Ranking

* **Hidden MMR** per player per mode.
* **Visible Rank:** tiers (Bronze→Mythic), 5 stars per tier; win = +1 star (MMR gates tier promotions/demotions).
* Party MMR = avg of top 2 accounts (2v2) / top 3 (3v3).
* **Placement:** 5 matches with inflated uncertainty (faster convergence).

**Backfill & Bots**

* If a player disconnects >10s in PreRound, bot fills for that round; if during RoundLive, continue 2v1 until RoundEnd (no mid‑round backfill to avoid desync).

---

## 15) Audio/VFX Readability

* Ability start cue ≤150ms from input (client‑predicted).
* Unique CC category sounds; DR proc chime.
* Colorblind‑safe palettes for team/CC states.
* Shared, authoritative obscurant state (e.g., smoke/shield volumes) to avoid per‑client divergence.

---

## 16) Telemetry & Anti‑Toxicity

**Input Integrity Policy**
- No multi‑action macros or hardware automation (e.g., SOCD/Snap‑Tap counter‑strafe macros). Server heuristics flag unnaturally perfect timing patterns; policy is published and enforced.

**Events**

* `match_start/end`, `round_start/end`, `ability_cast/resolved`, `status_applied`, `dampening_step`, `disconnect/reconnect`.
* QoS: ping, jitter, packet loss; client FPS.

**Social**

* No all‑chat. Post‑match commendations (Teamwork, Sportsmanship) award +5% XP next match.

---

## 17) Content Roadmap v0 → v1

**Seasons**
- Visible rank stars reset cadence with light recalibration seeded from prior rating (no hard resets); in‑client timeline communicates season milestones.

* **v0 Prototype (4–6 weeks):** 1 map, 3 heroes (above), offline vs bots + local host P2P; Mirror migration stub.
* **v0.5:** Dedicated server build, SBMM, cosmetics stub, analytics.
* **v1:** 2 maps, 12 heroes, ranked season 0, live‑ops weekly modifier.

---

## 18) Risks & Mitigations

* **Netcode desync / hit validation:** server rewind + small generous client boxes; authoritative DR/dampening on server.
* **Healer problem:** avoided (no pure healers).
* **Queue starvation:** role‑less, 2v2 focus, bots for Practice; regional bucket merging during off‑hours.

---

## 19) Acceptance Checklist (v0)

### Valve‑Inspired Work Items (v0.5+)
1) Facet System (pre‑round): add HeroFacet SO + pre‑round selection UI; cap numeric deltas to readability.
2) Systemic headline: prototype Reactive Shield Field (projectile deflection with visible deformation).
3) Sub‑tick‑style hits: extend server with input timestamps + 100ms rewind in hit resolver.
4) Round pacing: lock 120s soft cap + sudden‑death ring; dampening telegraphed every 5s.
5) Micro‑objectives: Watcher Pad (vision pulse) & Twin Pad (one‑use fast‑rotate per round).
6) Season flow: visible rank stars + seasonal recalibration UI.
7) Input integrity: server checks for impossible regular strafe/dash patterns; publish policy.
8) Cinematic readability: shared VFX state for obscurants (smokes/shields).
9) Patch comms: ship named beats with “why it changed” and short gifs (patch site template).
10) Cosmetics future‑proofing: stable item IDs/materials and carry‑forward policy.

* [ ] Round flows through all states with timers and UI.
* [ ] Abilities cast, hit, apply damage/CC with DR.
* [ ] Dash with i‑frame works and is readable.
* [ ] Dampening ramps damage taken and displays correctly.
* [ ] End‑of‑round recap shows 3 key insights.
* [ ] One‑tap requeue functional.
* [ ] Mirror server build runs headless; 4 clients connect, complet
