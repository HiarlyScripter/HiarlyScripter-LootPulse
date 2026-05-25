# Guia de Instalação — LootPulse

> ✅ Testado com **R.E.P.O. Build `23250495`** (v0.4.3.2) · **BepInExPack `5.4.23.5`** · **REPOConfig `1.2.6`** *(opcional)*
>
> ⚠️ Se o jogo atualizar e o mod parar de funcionar, verifique se há uma versão nova do LootPulse compatível.

---

## Dependências

### Obrigatórias (instale antes do mod)

| Mod | Versão testada | Link | Para que serve |
|---|---|---|---|
| **BepInExPack** | `5.4.23.5` | [Thunderstore](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) | Framework base — obrigatório para qualquer mod |

> ✅ **Nenhuma dependência extra além do BepInEx.** O LootPulse usa apenas APIs nativas do R.E.P.O.

### Opcionais

| Mod | Versão testada | Link | Para que serve |
|---|---|---|---|
| **REPOConfig** | `1.2.6` | [Thunderstore](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/) | Editar as configurações do mod dentro do próprio jogo, sem abrir arquivos |

---

## Como instalar

### Opção 1 — r2modman (recomendado)

1. Abra o **r2modman** e selecione o jogo **R.E.P.O.**
2. Clique em **Online** e procure por `BepInExPack` → instale
3. Procure por `LootPulse` → instale
4. *(Opcional)* Procure por `REPOConfig` → instale
5. Clique em **Start modded** para jogar

### Opção 2 — Manual

1. Instale o **BepInExPack** primeiro
2. Copie a pasta `plugins/HiarlyScripter-LootPulse/` para dentro de `BepInEx/plugins/` no diretório do jogo
3. Inicie o jogo uma vez — o arquivo de config será gerado automaticamente em `BepInEx/config/com.hiarlyscripter.repo.lootpulse.cfg`

---

## Quem precisa instalar?

> **Cada jogador** precisa ter o mod instalado para ver os brackets e os ícones no mapa.

O scan é processado localmente — cada cliente marca os itens na sua própria tela. A experiência é independente entre jogadores.

| Cenário | Resultado |
|---|---|
| Só o host tem o mod | Apenas o host vê os brackets e o mapa — outros não |
| Todos têm o mod | Todos escaneiam normalmente |
| Ninguém tem o mod | Nenhum efeito |

---

## Configurações disponíveis

Edite em `BepInEx/config/com.hiarlyscripter.repo.lootpulse.cfg` ou use o **REPOConfig** dentro do jogo:

| Seção | Chave | Padrão | O que faz |
|---|---|---|---|
| `Scan` | `ScanKey` | `F` | Tecla que ativa o scan de valuables |
| `Scan` | `ScanRadius` | `20` | Raio do scan em metros — 5 (perto) a 200 (toda a fase) |

---

## Problemas comuns

| Problema | Solução |
|---|---|
| Pressiono F e nada acontece | Confirme que o mod carregou — abra `BepInEx/LogOutput.log` e procure por `[LootPulse]` |
| Os brackets aparecem mas o mapa não | API do mapa pode ter mudado com update do jogo — reporte como bug com o log completo |
| Conflito de tecla com outro mod | Altere `ScanKey` para outra tecla no arquivo `.cfg` ou via REPOConfig |
| O mod não carrega | Verifique se o **BepInExPack** está instalado e habilitado no r2modman |

---

*Mod criado por **HiarlyScripter**.*
