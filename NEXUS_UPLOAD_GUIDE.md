# Guia de Upload — LootPulse v1.0.0 no Nexus Mods

## 1. Acessar o formulário de upload

- URL do jogo no Nexus: `https://www.nexusmods.com/repo`
- Clique em **Upload mod** (canto superior direito, logado)

---

## 2. Campos do formulário

| Campo | Valor |
|---|---|
| **Mod name** | `LootPulse` |
| **Summary** | `Press F to pulse-scan all valuables in range — shows discover brackets and adds map icons. Resilient to game updates via reflection-based API fallbacks.` |
| **Version** | `1.0.0` |
| **Category** | `Utilities` *(ou a mais próxima disponível para R.E.P.O.)* |
| **Language** | `Any` |
| **Is adult content?** | `No` |

---

## 3. Descrição (BBCode)

1. Abra o arquivo `NEXUS_DESCRIPTION.bbcode` nesta pasta
2. Selecione tudo (`Ctrl+A`) e copie (`Ctrl+C`)
3. No formulário do Nexus, clique em **"Import description"** (ícone de seta para cima no editor)
4. Cole o conteúdo e confirme

> ⚠️ Use **Import description**, nunca cole diretamente no editor visual — o editor visual quebra as tags BBCode.

---

## 4. Arquivo principal (ZIP)

Arquivo: `LootPulse-v1.0.0-NexusMods.zip` (nesta pasta)

- Estrutura interna: `BepInEx/plugins/HiarlyScripter-LootPulse/LootPulse.dll`
- O Vortex instala automaticamente com essa estrutura

---

## 5. Imagens

Pasta `media/` neste diretório:

| Arquivo | Uso |
|---|---|
| `nexus_banner_1300x372.png` | **Main image** (banner do topo da página) |
| `nexus_gallery_1920x1080.png` | **Images** (galeria da página) |

---

## 6. Tags (opcional)

```
loot, scanner, valuable, map, quality of life, bepinex
```

---

## 7. Checklist final antes de publicar

- [ ] Mod name: `LootPulse`
- [ ] Summary preenchido (máx 255 chars)
- [ ] Versão: `1.0.0`
- [ ] Descrição importada via **Import description** (não colada manualmente)
- [ ] ZIP correto anexado: `LootPulse-v1.0.0-NexusMods.zip`
- [ ] Banner (Main image) carregado
- [ ] Imagem de galeria carregada
- [ ] Adult content: **No**
- [ ] Clicar **Save** e verificar a página pública

---

## 8. Após publicar

Me passe o link da página do mod no Nexus para:
- Atualizar o GitHub Release com o link
- Verificar se o BBCode renderizou corretamente
- Adicionar o badge Nexus ao README do GitHub
