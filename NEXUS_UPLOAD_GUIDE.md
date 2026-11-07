# Guia de Upload — LootPulse v1.0.8 no Nexus Mods

## 1. Acessar a página do mod (já publicado — atualizar, não criar)

- URL direta: `https://www.nexusmods.com/repo/mods/255`
- Clique em **Manage mod** → **Files** → **Add file**

---

## 2. Adicionar novo arquivo (aba Files)

| Campo | Valor |
|---|---|
| **File name** | `LootPulse-v1.0.8-NexusMods` |
| **Version** | `1.0.8` |
| **Description** | `v1.0.8 — Revalidated for the 2026-11-06 game update (build 26407089). No gameplay changes.` |
| **Set as main file** | ✅ Sim |

Arquivo a anexar: `releases\LootPulse-v1.0.8-NexusMods.zip`

- Estrutura interna: `BepInEx/plugins/HiarlyScripter-LootPulse/LootPulse.dll`
- O Vortex instala automaticamente com essa estrutura

---

## 3. Atualizar a versão na página principal

- Vá em **Manage mod** → **Edit mod**
- Campo **Version**: altere para `1.0.8`
- Clique **Save**

---

## 4. Atualizar a descrição (BBCode)

1. Abra o arquivo `NEXUS_DESCRIPTION.bbcode` nesta pasta
2. Selecione tudo (`Ctrl+A`) e copie (`Ctrl+C`)
3. Em **Edit mod** → aba **Description**, clique em **"Import description"** (ícone de seta no editor)
4. Cole o conteúdo e confirme
5. Salve

> ⚠️ Use **Import description**, nunca cole diretamente no editor visual — o editor visual quebra as tags BBCode.

---

## 5. Post de changelog / update (opcional)

Se a página permitir "Posts" ou "Articles", use o texto curto bilíngue:

**PT-BR:**
`v1.0.8 revalida o mod para o update do R.E.P.O. de 06/11/2026 (build 26407089). Nenhuma mudança de gameplay.`

**EN:**
`v1.0.8 revalidates the mod for the 2026-11-06 R.E.P.O. update (build 26407089). No gameplay changes.`

---

## 6. Resposta ao usuário que reportou o bug

Cole no comentário/bug report do usuário:

```
Thanks again for the report. I reproduced the issue and found the cause: LootPulse was
loading correctly, but on the current R.E.P.O. build the original Unity/BepInEx update
loop was not reliably reaching the scan logic, so pressing F could do nothing even though
the plugin appeared loaded.

This is fixed in v1.0.8.

The scan trigger now runs through a Harmony-patched game loop, and I also added clearer
diagnostic logs for startup, config, key detection, blocked scans, and scan results.

Please update to v1.0.8 and test again. When pressing F inside an active level, the log
should show:

  Scan key detected: F
  Scan executed
  Result | brackets=X/X | map=X/X | errors=0

Also make sure the old BetterItemScanner mod is not installed/enabled at the same time,
since it is broken on the current R.E.P.O. build and uses the same key.
```

---

## 7. Checklist final

- [ ] Arquivo `LootPulse-v1.0.8-NexusMods.zip` adicionado como **main file**
- [ ] Versão da página atualizada para `1.0.8`
- [ ] Descrição importada via **Import description**
- [ ] Resposta postada ao usuário que reportou o bug
- [ ] Página pública carregando corretamente

---

## 8. Após publicar

Passe o link da página para atualizar o GitHub Release com o badge/link do Nexus.
