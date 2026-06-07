# DEVLOG — LootPulse

## Backlog — Features para v1.0.1 ou v1.1.0

> Registradas em 2026-05-24. Nenhuma implementada ainda — aguardando próximo ciclo.

| # | Feature | Observação |
|---|---|---|
| 1 | **Filtro "somente não descobertos"** | Evita repulsar itens já marcados no mapa |
| 2 | **Cor/efeito por valor do item** | Destaque diferente para itens mais valiosos — depende de API que exponha valor com segurança |
| 3 | **Modo toggle (pulso automático)** | Tecla liga/desliga scan automático a cada X segundos, em vez de apertar F toda vez |
| 4 | **Scan inteligente com limite máximo** | Evita marcar 80–100 itens de uma vez e poluir mapa/HUD |
| 5 | **Som/feedback por resultado** | Feedback leve ao encontrar item; outro quando não encontra nada |
| 6 | **Config `AddMapIconsOnlyOnce`** | Evita duplicação de ícones e reduz chamadas repetidas no mapa |
| 7 | **Blacklist/whitelist por categoria** | Filtrar por valor mínimo ou tipo de valuable — depende de exposição pela API do jogo |

---

## v1.0.6 — 2026-06-04 — Runtime Approved (hotfix)

**Status:** `v1.0.6 stable runtime-approved`
**Build marker:** `v1.0.6-release`

**Causa raiz do bug corrigido:**
- O Unity/BepInEx loop original (`BaseUnityPlugin.Update()` + `new GameObject()` + `DontDestroyOnLoad`) não alcançava a lógica do scan de forma confiável na build atual do R.E.P.O. v0.4.4.3. GameObjects criados durante o Chainloader init (antes da cena Unity estar ativa) não entram no ciclo de update do jogo nessa build.
- Diagnóstico confirmado via inspeção de `Assembly-CSharp.dll` por reflexão .NET.

**Correção aplicada:**
- Harmony postfix em `GameDirector.Update` — piggybacks no loop nativo do jogo.
- Fallback chain: `GameDirector.Update` → `PlayerAvatar.Update` → `PlayerController.Update`.
- Resolução de tipo em runtime via `AppDomain.CurrentDomain.GetAssemblies()` — resiliente a renames futuros.
- Watchdog ThreadPool: avisa se 60s passarem sem tick após patch instalado.
- Frame deduplication: `Time.frameCount == _lastTickFrame` evita double-tick.

**Teste runtime aprovado (Level - Manor, singleplayer):**

| Scan | found | brackets | map  | errors |
|------|-------|----------|------|--------|
| 1    | 2     | 2/2      | 2/2  | 0      |
| 2    | 24    | 24/24    | 24/24| 0      |
| 3    | 13    | 13/13    | 13/13| 0      |

- `Harmony tick patch installed: GameDirector.Update` ✅
- `Harmony game loop active: GameDirector.Update` ✅
- `Scan key detected: F | source=GameDirector.Update` ✅
- `brackets=100% | map=100% | errors=0` ✅

**Pacote:**
- ZIP Thunderstore: `releases/HiarlyScripter-LootPulse-1.0.6.zip`
- SHA256: `CFEC9C57B983FA73573958E809B2E553D25205862907CFA7F7F9C718E0D0F90F`
- Conteúdo: `plugins/HiarlyScripter-LootPulse/LootPulse.dll`, `icon.png`, `manifest.json`, `README.md`, `CHANGELOG.md`
- `0Harmony.dll` **não empacotado** (`<Private>false</Private>` no .csproj) ✅

**Publicado em:**
- GitHub: commit `fix: release LootPulse v1.0.6 execution loop hotfix` · tag `v1.0.6` · Release com ZIP anexado
- Thunderstore: `HiarlyScripter/LootPulse` v1.0.6
- Nexus Mods: ZIP `LootPulse-v1.0.6-NexusMods.zip` atualizado

**Backlog v1.1.0+** (sem implementação nesta versão):
- Filtro de valuables já descobertos
- `AddMapIconsOnlyOnce`, `MaxItemsPerScan`
- Auto-pulso / modo toggle
- Efeitos visuais por valor, feedback sonoro
- Whitelist/blacklist por categoria

---

## v1.0.0 — 2026-05-24 — Runtime Approved (stable)

**Status:** `v1.0.0 stable runtime-approved`
**Checkpoint:** `releases\v1.0.0-stable\`

**Resumo do teste runtime:**
- Partida em R.E.P.O. Build 23250495, Level - Wizard, singleplayer
- Ambiente de teste limpo — log sem interferência de outros scanners
- LootPulse carregou com sucesso (linha 42 do log)
- Discover API resolvida via reflexão na primeira chamada: `ValuableObject.Discover(ValuableDiscoverGraphic.State)`

**Resultados — 11 scans executados:**

| Scan | Origem               | encontrados | brackets  | mapa      | erros |
|------|----------------------|-------------|-----------|-----------|-------|
| 1    | (1.7, -0.9, 5.4)     | 83          | 83/83     | 83/83     | 0     |
| 2    | (6.2, 0.0, 7.2)      | 83          | 83/83     | 83/83     | 0     |
| 3    | (14.5, 0.0, 13.8)    | 82          | 82/82     | 82/82     | 0     |
| 4    | (19.8, 0.0, 10.8)    | 69          | 69/69     | 69/69     | 0     |
| 5    | (25.1, -2.3, 8.2)    | 48          | 48/48     | 48/48     | 0     |
| 6    | (19.0, 0.0, 9.5)     | 69          | 69/69     | 69/69     | 0     |
| 7    | (15.6, 0.0, 11.6)    | 75          | 75/75     | 75/75     | 0     |
| 8    | (16.9, 0.0, 13.6)    | 82          | 82/82     | 82/82     | 0     |
| 9    | (7.2, 0.0, 23.2)     | 91          | 91/91     | 91/91     | 0     |
| 10   | (-6.2, 0.0, 30.3)    | 78          | 78/78     | 78/78     | 0     |
| 11   | (-5.0, 0.0, 31.7)    | 75          | 75/75     | 75/75     | 0     |

**Confirmações:**
- Brackets visuais: ✅ 100% em todos os scans
- Ícones no mapa: ✅ 100% em todos os scans
- `erros=0` no LootPulse: ✅ confirmado em todos os scans
- Cooldown: ✅ funcionando (mensagens de tempo restante visíveis no log)
- Validação visual in-game: ✅ brackets amarelos visíveis nos itens, ícones aparecendo no mapa

**Autoria:**
- Código 100% original — zero cópia de mods de terceiros
- Livre de direitos autorais de terceiros
- APIs do jogo usadas como interfaces públicas de modding BepInEx

---

## v1.0.0 — 2026-05-24 — Release inicial

**Contexto:**
Mod criado do zero para scan de valuables em R.E.P.O. Build 23250495.

**APIs verificadas via reflexão .NET na build local:**
- `SemiFunc.PhysGrabObjectAllValuablesWithinRange(float, Vector3, bool, LayerMask)` → `List<PhysGrabObject>` ✅
- `ValuableObject.Discover(ValuableDiscoverGraphic.State)` → `bool` ✅
  - Enum `State` nested em `ValuableDiscoverGraphic`: valores Discover, Reminder, Bad, Custom
- `Map.Instance` → `Map` (campo estático) ✅
- `Map.AddValuable(ValuableObject)` → `void` (público) ✅
- `PlayerAvatar.playerTransform` → `Transform` (público) ✅
- `PlayerAvatar.localCameraTransform` → REMOVIDO ❌ (causa crash em mods que referenciam esse campo)

**Decisões de design:**
- Sem Harmony — Update() no BaseUnityPlugin é suficiente
- ValuableObject.Discover via reflexão (resilente a futuras mudanças de assinatura)
- Fallback FindObjectsOfType se SemiFunc falhar
- Cada item processado em try/catch individual — erro num item não cancela o scan
- Log completo a cada scan: permite diagnóstico runtime sem abrir debugger
