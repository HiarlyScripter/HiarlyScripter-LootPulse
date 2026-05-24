# 🔍 LootPulse

[![Versão](https://img.shields.io/badge/versão-1.0.0-brightgreen?style=flat-square)]()
[![R.E.P.O.](https://img.shields.io/badge/R.E.P.O.-Build%2023250495-blue?style=flat-square)]()
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.23.5-yellow?style=flat-square)]()
[![Multiplayer](https://img.shields.io/badge/Multiplayer-Client--side-9b59b6?style=flat-square)]()
[![REPOConfig](https://img.shields.io/badge/REPOConfig-compatível-orange?style=flat-square)]()

> Pressione **F** para pulsar o radar — todos os valuables no raio acendem com brackets amarelos e aparecem no mapa.

---

## ✨ O que faz

LootPulse é um scanner de valuables para R.E.P.O. Ao pressionar a tecla de scan, o mod localiza todos os objetos valiosos no raio configurado, exibe o efeito visual de descoberta nativo do jogo (brackets amarelos) em cada um e os registra no mapa.

## 📡 Funcionalidades

- 📡 **Scan de valuables** — detecta todos os objetos valiosos no raio configurado
- 🟡 **Brackets visuais** — ativa o efeito de descoberta nativo do jogo em cada valuable
- 🗺️ **Ícones no mapa** — adiciona automaticamente ao mapa do R.E.P.O.
- 🔄 **Cooldown configurável** — evita spam de scan
- 🛡️ **Resiliente a updates** — fallbacks automáticos se alguma API do jogo mudar
- 📋 **Logs detalhados** — sabe exatamente quantos itens foram encontrados e processados

## 👥 Quem precisa instalar?

> Cada jogador que quiser usar o scanner precisa ter o mod instalado.

| Cenário | Resultado |
|---|---|
| Só o host tem o mod | Apenas o host vê os brackets e o mapa atualizado |
| Todos os jogadores têm | Cada jogador usa seu próprio scanner independentemente |
| Ninguém tem | Nenhuma alteração no jogo |

## ⚙️ Configurações

| Seção | Chave | Padrão | Descrição |
|---|---|---|---|
| Scanner | ScanKey | F | Tecla para ativar o scanner |
| Scanner | ScanRange | 30 | Raio de detecção em metros (5–200) |
| Scanner | CooldownSeconds | 4 | Segundos mínimos entre scans (0–60) |
| Scanner | EnableVisualBrackets | true | Exibir brackets visuais nos valuables |
| Scanner | EnableMapIcons | true | Adicionar valuables ao mapa |
| Scanner | RequireLevelOrRun | true | Só ativa durante runs (não no menu/lobby) |
| Scanner | VerboseLogging | false | Log detalhado de cada item processado |

## 📦 Dependências

| Mod | Obrigatória? | Versão |
|---|---|---|
| BepInExPack | ✅ Sim | 5.4.2100+ |
| REPOConfig | ❌ Opcional | Qualquer — edita configs in-game |

## 🖼️ Screenshots

*Em breve.*
<!-- SCREENSHOTS_PLACEHOLDER -->

## 🛠️ Instalação rápida

**Via r2modman (recomendado):**
1. Instale o BepInExPack pelo Thunderstore
2. Instale o LootPulse
3. Start modded

**Manual:**
1. Instale BepInExPack
2. Copie `plugins/HiarlyScripter-LootPulse/` para `BepInEx/plugins/`
3. Inicie o jogo

## ❓ Problemas comuns

| Problema | Solução |
|---|---|
| Brackets não aparecem, mas o log mostra "encontrados=X" | Confira se `EnableVisualBrackets=true` no config |
| Nenhum valuable encontrado no scan | Verifique se está dentro de um nível (não no menu/shop) e se há valuables no raio |
| Scanner dispara com a mesma tecla de outro mod | Mude a `ScanKey` nas configurações para uma tecla sem conflito |
| Erro no log após apertar F no menu | Defina `RequireLevelOrRun=true` ou só use em nível ativo |

---

*Mod criado por **[HiarlyScripter](https://discord.com/users/hiarly_ferreira)** — Testado com R.E.P.O. Build `23250495` · BepInEx `5.4.23.5`*
