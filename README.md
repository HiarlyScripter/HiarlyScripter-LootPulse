# 🔍 LootPulse

[![Thunderstore](https://img.shields.io/badge/Thunderstore-v1.0.6-brightgreen?style=flat-square&logo=thunderstore)](https://thunderstore.io/c/repo/p/HiarlyScripter/LootPulse/)
[![Nexus Mods](https://img.shields.io/badge/Nexus%20Mods-255-orange?style=flat-square&logo=nexusmods)](https://www.nexusmods.com/repo/mods/255)
[![R.E.P.O.](https://img.shields.io/badge/R.E.P.O.-v0.4.4.3-blue?style=flat-square)](https://store.steampowered.com/app/3241660/REPO/)
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.23.5-yellow?style=flat-square)](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
[![Licença](https://img.shields.io/badge/licença-crédito%20obrigatório-red?style=flat-square)](LICENSE)

**[Português (BR)](#português-br) | [English](#english)**

---

## Português (BR)

Pressione **F** para escanear todos os valuables no raio — revela os brackets de descoberta nativos do jogo e adiciona ícones no mapa.

### ✨ O que faz

Com um único aperto de tecla, o LootPulse localiza todos os objetos valiosos no raio configurado, ativa os brackets amarelos de descoberta do jogo em cada item e os adiciona ao mapa. Nenhum valuable fica escondido.

### 🎯 Recursos

- 🔍 **Scan por tecla** — pressione F (configurável) para ativar o pulso imediatamente
- 📍 **Brackets de descoberta** — usa a animação nativa do jogo em cada valuable encontrado
- 🗺️ **Ícones no mapa** — todos os valuables escaneados aparecem no mapa automaticamente
- 📏 **Raio configurável** — ajuste o alcance de 5 a 200 metros
- ⏱️ **Cooldown configurável** — evita spam de scan (0–60 segundos)
- 🛡️ **Resiliente a updates** — APIs críticas resolvidas via reflexão e Harmony; fallbacks automáticos

### 👥 Multiplayer

> **Cada jogador** precisa instalar o mod para ver os efeitos na sua tela.

| Cenário | Resultado |
|---|---|
| Só o host tem o mod | Apenas o host vê brackets e mapa |
| Todos têm o mod | Cada jogador usa seu próprio scanner |
| Ninguém tem o mod | Nenhuma alteração |

### 🕹️ Como usar

1. Entre em uma **run ativa** (fase gerada — não no menu ou lobby)
2. Pressione **F**
3. Todos os valuables no raio de 30m recebem **brackets amarelos de descoberta**
4. **Ícones** aparecem no mapa automaticamente

### 📦 Instalação

**Via r2modman (recomendado):**
1. Instale o **BepInExPack**
2. Procure e instale o **LootPulse** no Thunderstore
3. *(Opcional)* Instale o **REPOConfig** para editar configurações dentro do jogo
4. Clique em **Start modded**

**Via Vortex / Nexus Mods:**
1. Instale o **BepInExPack**
2. Use o botão **Mod Manager Download** — o Vortex instala automaticamente
3. Ou manualmente: extraia e copie a pasta `BepInEx/` para o diretório do jogo

**Manual:**
1. Instale o **BepInExPack**
2. Copie `plugins/HiarlyScripter-LootPulse/` para `BepInEx/plugins/`

### ⚙️ Configurações

Edite `BepInEx/config/com.hiarlyscripter.repo.lootpulse.cfg` ou use o **REPOConfig** dentro do jogo:

| Chave | Padrão | Descrição |
|---|---|---|
| `ScanKey` | `F` | Tecla que ativa o scanner |
| `ScanRange` | `30` | Raio de detecção em metros (5–200) |
| `CooldownSeconds` | `4` | Segundos mínimos entre scans (0–60) |
| `EnableVisualBrackets` | `true` | Exibe brackets de descoberta |
| `EnableMapIcons` | `true` | Adiciona ícones no mapa |
| `RequireLevelOrRun` | `true` | Exige run ativa para escanear |
| `VerboseLogging` | `false` | Log detalhado de cada item processado |

### ⚠️ Compatibilidade

> **Não instale junto com o BetterItemScanner.**
> O BetterItemScanner está quebrado na build atual do R.E.P.O. e usa a mesma tecla F. Conflito garantido.

### 🔧 Diagnóstico

Se pressionar F não fizer nada, verifique `BepInEx/LogOutput.log`. O log deve conter:

```
[LootPulse] Harmony tick patch installed: GameDirector.Update
[LootPulse] Harmony game loop active: GameDirector.Update
```

Ao pressionar F dentro de uma run:

```
[LootPulse] Scan key detected: F | source=GameDirector.Update
[LootPulse] Scan executed | origin=... | range=30m | api=SemiFunc | found=X
[LootPulse] Result | brackets=X/X | map=X/X | errors=0
```

Se `Harmony tick patch installed` aparecer mas `Scan key detected` não:
- Confirme que está dentro de uma **run ativa** (não no menu)
- Verifique se `RequireLevelOrRun` está `true` no config

### 🗓️ Changelog resumido

| Versão | Data | Destaque |
|---|---|---|
| v1.0.6 | 2026-06-04 | **Hotfix: loop de execução** — scan agora usa Harmony em `GameDirector.Update`; logs diagnósticos |
| v1.0.5 | 2026-06-02 | Ícone atualizado (design scanner) |
| v1.0.0 | 2026-05-24 | Release inicial |

### 🔮 Backlog futuro (sem data definida)

Ideias para versões futuras — não implementadas ainda, sem compromisso de prazo:

- Filtro de valuables já descobertos
- `AddMapIconsOnlyOnce` — evitar duplicação de ícones
- `MaxItemsPerScan` — limitar itens marcados por scan
- Auto-pulso / modo toggle
- Efeitos visuais por valor (se API expor valor com segurança)
- Feedback sonoro por resultado
- Whitelist/blacklist por categoria (se API expor com segurança)

---

## English

Press **F** to scan all valuables within range — reveals the game's native discovery brackets and adds map icons.

### ✨ What it does

With a single keypress, LootPulse locates all valuable objects within the configured range, triggers the game's native yellow discovery brackets on each item, and adds them to the map. No valuable stays hidden.

### 🎯 Features

- 🔍 **Key-triggered scan** — press F (configurable) to pulse immediately
- 📍 **Discovery brackets** — uses the game's native animation on each valuable found
- 🗺️ **Map icons** — all scanned valuables appear on the map automatically
- 📏 **Configurable range** — set the scan radius from 5 to 200 meters
- ⏱️ **Configurable cooldown** — prevents scan spam (0–60 seconds)
- 🛡️ **Resilient to updates** — critical APIs resolved via reflection and Harmony; automatic fallbacks

### 👥 Multiplayer

> **Each player** needs to install the mod to see the effects on their screen.

| Scenario | Result |
|---|---|
| Only host has the mod | Only host sees brackets and map |
| Everyone has the mod | Each player uses their own scanner |
| Nobody has the mod | No changes |

### 🕹️ How to use

1. Enter an **active run** (generated level — not the menu or lobby)
2. Press **F**
3. All valuables within 30m radius receive **yellow discovery brackets**
4. **Icons** appear on the map automatically

### 📦 Installation

**Via r2modman (recommended):**
1. Install **BepInExPack**
2. Search and install **LootPulse** from Thunderstore
3. *(Optional)* Install **REPOConfig** to edit settings in-game
4. Click **Start modded**

**Via Vortex / Nexus Mods:**
1. Install **BepInExPack**
2. Use the **Mod Manager Download** button — Vortex installs automatically
3. Or manually: extract and copy the `BepInEx/` folder to the game directory

**Manual:**
1. Install **BepInExPack**
2. Copy `plugins/HiarlyScripter-LootPulse/` to `BepInEx/plugins/`

### ⚙️ Configuration

Edit `BepInEx/config/com.hiarlyscripter.repo.lootpulse.cfg` or use **REPOConfig** in-game:

| Key | Default | Description |
|---|---|---|
| `ScanKey` | `F` | Key that triggers the scanner |
| `ScanRange` | `30` | Detection radius in meters (5–200) |
| `CooldownSeconds` | `4` | Minimum seconds between scans (0–60) |
| `EnableVisualBrackets` | `true` | Show discovery brackets |
| `EnableMapIcons` | `true` | Add icons to map |
| `RequireLevelOrRun` | `true` | Only scan during active runs |
| `VerboseLogging` | `false` | Log details per processed item |

### ⚠️ Compatibility

> **Do not install alongside BetterItemScanner.**
> BetterItemScanner is broken on the current R.E.P.O. build and uses the same F key. Conflict is guaranteed.

### 🔧 Troubleshooting

If pressing F does nothing, check `BepInEx/LogOutput.log`. It should contain:

```
[LootPulse] Harmony tick patch installed: GameDirector.Update
[LootPulse] Harmony game loop active: GameDirector.Update
```

When pressing F inside a run:

```
[LootPulse] Scan key detected: F | source=GameDirector.Update
[LootPulse] Scan executed | origin=... | range=30m | api=SemiFunc | found=X
[LootPulse] Result | brackets=X/X | map=X/X | errors=0
```

If `Harmony tick patch installed` appears but `Scan key detected` does not:
- Make sure you are inside an **active run** (not the menu)
- Check that `RequireLevelOrRun` is `true` in the config

### 🗓️ Changelog summary

| Version | Date | Highlight |
|---|---|---|
| v1.0.6 | 2026-06-04 | **Hotfix: execution loop** — scan now uses Harmony on `GameDirector.Update`; diagnostic logs |
| v1.0.5 | 2026-06-02 | Updated icon (scanner design) |
| v1.0.0 | 2026-05-24 | Initial release |

### 🔮 Future backlog (no timeline)

Ideas for future versions — not implemented yet, no timeline commitment:

- Filter already-discovered valuables
- `AddMapIconsOnlyOnce` — prevent duplicate map icons
- `MaxItemsPerScan` — cap items marked per scan
- Auto-pulse / toggle mode
- Visual effects by value (if API safely exposes value)
- Sound feedback on result
- Whitelist/blacklist by category (if API safely exposes it)

---

*Mod by **[HiarlyScripter](https://discord.com/users/hiarly_ferreira)** · Tested on R.E.P.O. v0.4.4.3 · BepInEx 5.4.23.5*
