# Changelog — LootPulse

---

## v1.0.1 — em desenvolvimento
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`

### Alterado
- *(pendente — nenhuma feature implementada ainda)*

---

## v1.0.0 — 2026-05-24
**Compatibilidade:** R.E.P.O. Build `23250495` (v0.4.3.2) · BepInEx `5.4.23.5`
**Status:** `stable runtime-approved` — testado em 11 scans, brackets=100%, mapa=100%, erros=0

### Adicionado
- Scanner de valuables via tecla configurável (padrão: `F`)
- Brackets visuais de descoberta usando `ValuableObject.Discover(ValuableDiscoverGraphic.State.Discover)`
- Adição automática de ícone no mapa via `Map.Instance.AddValuable()`
- Fallback via reflexão para API Discover (resilente a futuras mudanças de assinatura)
- Fallback via `FindObjectsOfType<ValuableObject>` se `SemiFunc.PhysGrabObjectAllValuablesWithinRange` falhar
- Config: `ScanKey`, `ScanRange`, `CooldownSeconds`, `EnableVisualBrackets`, `EnableMapIcons`, `RequireLevelOrRun`, `VerboseLogging`
- Logs detalhados de cada scan: origem, range, API usada, contagem de resultados, erros isolados
