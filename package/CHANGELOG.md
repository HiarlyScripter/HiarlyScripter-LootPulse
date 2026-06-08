# Changelog — LootPulse

---

## v1.0.7 — 2026-06-08
**Compatibilidade:** R.E.P.O. v0.4.4.3 · BepInEx `5.4.23.5`

### Changed
- README updated to bilingual PT-BR + EN (no code or gameplay changes)

---

## v1.0.6 — 2026-06-04
**Compatibilidade:** R.E.P.O. v0.4.4.3 · BepInEx `5.4.23.5`

### Fixed
- Execution loop: scan tick now driven by a Harmony postfix on `GameDirector.Update`, bypassing MonoBehaviour Update lifecycle issues in R.E.P.O. v0.4.4.3
- Internal DLL version synced from `1.0.0` to `1.0.6`

### Improved
- Startup diagnostics: build marker, assembly location, assembly file timestamp, config dump at load
- Scan diagnostics: key detected log (with tick source), blocked reason, scan result per call (brackets / map icons / errors)
- Harmony fallback chain: `GameDirector.Update` → `PlayerAvatar.Update` → `PlayerController.Update` — clear error if no method found
- Watchdog: warns if Harmony patch installs but no game loop tick is observed within 60 seconds
- All logs in English
- No gameplay feature changes

---

## v1.0.5 — 2026-06-02
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Alterado
- Changelog atualizado com histórico completo de versões

---

## v1.0.4 — 2026-06-02
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Alterado
- Ícone atualizado: scanner com anéis dourados e nome "Loot Pulse" centralizado

---

## v1.0.3 — 2026-06-02
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Alterado
- Ícone atualizado (primeira versão com design do scanner da galeria)

---

## v1.0.2 — 2026-05-25
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Alterado
- Ícone corrigido para design do scanner dourado
- Primeira publicação no Nexus Mods

---

## v1.0.0 — 2026-05-24
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Adicionado
- Scanner de valuables via tecla configurável (padrão: F)
- Brackets visuais de descoberta nativos do jogo
- Ícones no mapa para valuables escaneados
- Config: ScanKey, ScanRange, CooldownSeconds, EnableVisualBrackets, EnableMapIcons, RequireLevelOrRun, VerboseLogging
- Fallbacks via reflexão para APIs críticas
