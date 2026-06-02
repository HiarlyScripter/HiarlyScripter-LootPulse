# 🔍 LootPulse

[![Thunderstore](https://img.shields.io/badge/Thunderstore-v1.0.4-brightgreen?style=flat-square&logo=thunderstore)](https://thunderstore.io/c/repo/p/HiarlyScripter/LootPulse/)
[![Nexus Mods](https://img.shields.io/badge/Nexus%20Mods-255-orange?style=flat-square&logo=nexusmods)](https://www.nexusmods.com/repo/mods/255)
[![R.E.P.O.](https://img.shields.io/badge/R.E.P.O.-Build%2023250495-blue?style=flat-square)](https://store.steampowered.com/app/3241660/REPO/)
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.23.5-yellow?style=flat-square)](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
[![Licença](https://img.shields.io/badge/licença-crédito%20obrigatório-red?style=flat-square)](LICENSE)

> Pressione **F** para escanear todos os valuables no raio — revela os brackets de descoberta e adiciona ícones no mapa.

---

## ✨ O que faz

Com um único aperto de tecla, o LootPulse varre todos os objetos valiosos no raio de scan e os marca com os brackets de descoberta do jogo, além de adicioná-los ao mapa. Útil para não perder nenhum item escondido em cantos, embaixo de móveis ou em locais de difícil visão.

---

## 🎯 Funcionalidades

- 🔍 **Scan por tecla** — pressione F (configurável) para ativar o pulso imediatamente
- 📍 **Brackets de descoberta** — usa a animação nativa do jogo em cada valuable encontrado
- 🗺️ **Ícones no mapa** — todos os valuables escaneados aparecem no mapa automaticamente
- 📏 **Raio configurável** — ajuste o alcance do scan de 5 a 200 metros
- 🛡️ **Resiliente a updates** — APIs críticas resolvidas via reflexão; fallbacks automáticos se o jogo atualizar
- ⚙️ **REPOConfig** — configurações editáveis dentro do próprio jogo *(opcional)*

---

## 👥 Multiplayer

> **Cada jogador** precisa instalar o mod para ver os efeitos na sua tela.

| Cenário | Resultado |
|---|---|
| Só o host tem o mod | Host vê brackets e mapa — outros jogadores não veem nada |
| Todos têm o mod | Todos escaneiam e veem os efeitos normalmente |
| Ninguém tem o mod | Nenhum efeito |

---

## ⚙️ Configurações

| Seção | Chave | Padrão | Descrição |
|---|---|---|---|
| `Scan` | `ScanKey` | `F` | Tecla que ativa o scan |
| `Scan` | `ScanRadius` | `20` | Raio do scan em metros (5–200) |

---

## 📦 Instalação

**Via r2modman (recomendado):**
1. Instale o **BepInExPack**
2. Procure e instale o **LootPulse** no Thunderstore
3. *(Opcional)* Instale o **REPOConfig**
4. Clique em **Start modded**

**Via manual:**
1. Instale o BepInExPack
2. Copie `plugins/HiarlyScripter-LootPulse/` para `BepInEx/plugins/`

---

## 📄 Licença

[Licença customizada](LICENSE) — uso e estudo permitidos. **Crédito ao autor obrigatório** em qualquer redistribuição ou trabalho derivado.

---

*Mod criado por **[HiarlyScripter](https://discord.com/users/hiarly_ferreira)** · Testado com R.E.P.O. Build `23250495` · BepInEx `5.4.23.5`*
