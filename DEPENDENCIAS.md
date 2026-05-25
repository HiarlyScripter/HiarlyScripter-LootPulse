# Dependências — LootPulse

Este mod precisa de outros mods instalados para funcionar corretamente. **Sem eles, o LootPulse não vai carregar.**

---

## Obrigatórias

### 1. BepInExPack
**O que é:** Framework base que permite rodar mods no R.E.P.O.
**Onde baixar:** [Thunderstore — BepInExPack](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
**Como instalar:** Pelo r2modman, basta procurar "BepInExPack" e clicar em instalar. Se você já roda outros mods, provavelmente já tem.

> ✅ **Nenhuma dependência de mods de conteúdo.** O LootPulse usa exclusivamente APIs nativas do R.E.P.O. — nenhum mod externo de itens ou inimigos é necessário.

---

## Opcionais

### REPOConfig
**O que é:** Permite editar as configurações do mod diretamente dentro do jogo, sem precisar abrir arquivos de texto.
**Onde baixar:** [Thunderstore — REPOConfig](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/)
**Necessário apenas se:** Você quiser ajustar `ScanKey` ou `ScanRadius` sem sair do jogo. Sem ele, as configs ainda funcionam normalmente pelo arquivo `.cfg`.

---

## Verificando se está tudo instalado

1. Abra o r2modman e veja a lista de mods ativos no perfil.
2. Confirme que o **BepInExPack** está na lista e **habilitado**.
3. Inicie o jogo pelo r2modman (não diretamente pelo Steam).
4. Para confirmar que o mod carregou, abra o arquivo `BepInEx/LogOutput.log` e procure por: `[LootPulse] v1.0.0 carregado.`
