# Checkpoint 2026-11-06 — LootPulse, etapa só leitura (update do jogo)
- Jogo: Steam buildid 26407089, Assembly-CSharp.dll 06/11/2026 20:55. Unity 6000.3.17 (UnityPlayer.dll) e UnityEngine.CoreModule.dll de 29/10/2026: a troca de Unity veio ANTES do patch de 06/11.
- Compila contra a DLL nova: 0 erros, 1 warning (CS0618 FindObjectsOfType, preexistente). Saída em scratchpad, nada alterado em src/.
- APIs conferidas por decompilação (ilspycmd): SemiFunc.PhysGrabObjectAllValuablesWithinRange(float,Vector3,bool,LayerMask), RunIsLevel(), PlayerAvatarLocal(), ValuableObject.Discover(ValuableDiscoverGraphic.State)->bool, enum State {Discover,Reminder,Bad,Custom}, Map.Instance, Map.AddValuable(ValuableObject), PlayerAvatar.playerTransform, GameDirector.Update() privado sem parâmetros: todas inalteradas.
- LogOutput.log (REPO - Test, 21:26, pós-patch): 3 scans, brackets/mapa 100%, errors=0; sem MissingMethod/NullReference do LootPulse.
- Status: UNTESTED na build nova até o "aprovado" do Hiarly.

## Atualização (mesma data)
- Branch `update/novo-patch-2026-11`. Único ajuste de código: `FindObjectsOfType` -> `FindObjectsByType(FindObjectsSortMode.None)` no fallback. Build 0 erros / 0 warnings.
- DLL nova NÃO copiada para o perfil `REPO - Test` (REPO.exe aberto trava o arquivo). Copiar com o jogo fechado. Resultado: UNTESTED.
- Roteiro: entrar numa fase (solo), apertar F 3x; conferir brackets, ícones no mapa e `errors=0` no log; testar cooldown e F fora da fase (deve bloquear).
