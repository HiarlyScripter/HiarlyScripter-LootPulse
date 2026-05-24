using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;

namespace LootPulse
{
    [BepInPlugin("com.hiarlyscripter.repo.lootpulse", "LootPulse", "1.0.0")]
    public sealed class LootPulsePlugin : BaseUnityPlugin
    {
        internal static LootPulsePlugin Instance { get; private set; }
        internal static ManualLogSource Log { get; private set; }

        internal static ConfigEntry<KeyCode> ScanKey;
        internal static ConfigEntry<float>   ScanRange;
        internal static ConfigEntry<float>   CooldownSeconds;
        internal static ConfigEntry<bool>    EnableVisualBrackets;
        internal static ConfigEntry<bool>    EnableMapIcons;
        internal static ConfigEntry<bool>    RequireLevelOrRun;
        internal static ConfigEntry<bool>    VerboseLogging;

        private float _lastScanTime = -999f;

        // Resolve uma vez, reutiliza para sempre
        private static bool       _discoverApiChecked;
        private static MethodInfo _discoverWithState;
        private static MethodInfo _discoverNoParam;
        private static object     _discoverStateValue;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            ScanKey = Config.Bind("Scanner", "ScanKey", KeyCode.F,
                "Tecla para ativar o scanner de valuables.");

            ScanRange = Config.Bind("Scanner", "ScanRange", 30f,
                new ConfigDescription(
                    "Raio de detecção em metros.",
                    new AcceptableValueRange<float>(5f, 200f)));

            CooldownSeconds = Config.Bind("Scanner", "CooldownSeconds", 4f,
                new ConfigDescription(
                    "Segundos mínimos entre dois scans consecutivos.",
                    new AcceptableValueRange<float>(0f, 60f)));

            EnableVisualBrackets = Config.Bind("Scanner", "EnableVisualBrackets", true,
                "Exibir brackets visuais de descoberta nos valuables encontrados.");

            EnableMapIcons = Config.Bind("Scanner", "EnableMapIcons", true,
                "Adicionar ícone de valuable no mapa ao escanear.");

            RequireLevelOrRun = Config.Bind("Scanner", "RequireLevelOrRun", true,
                "Só escanear durante runs ativas (nível gerado). Desative para testar fora de um nível.");

            VerboseLogging = Config.Bind("Scanner", "VerboseLogging", false,
                "Logar detalhes de cada item processado individualmente.");

            Log.LogInfo("[LootPulse] v1.0.0 carregado. Pressione " + ScanKey.Value + " para escanear valuables.");
        }

        private void Update()
        {
            if (!Input.GetKeyDown(ScanKey.Value)) return;

            float now = Time.time;
            float elapsed = now - _lastScanTime;

            if (elapsed < CooldownSeconds.Value)
            {
                float remaining = CooldownSeconds.Value - elapsed;
                Log.LogInfo($"[LootPulse] Cooldown ativo: {remaining:F1}s restantes.");
                return;
            }

            if (RequireLevelOrRun.Value)
            {
                bool inLevel = false;
                try { inLevel = SemiFunc.RunIsLevel(); }
                catch (Exception ex)
                {
                    Log.LogWarning($"[LootPulse] RunIsLevel falhou ({ex.Message}). Prosseguindo sem validação.");
                    inLevel = true;
                }

                if (!inLevel)
                {
                    if (VerboseLogging.Value)
                        Log.LogInfo("[LootPulse] Ignorado — não está em nível ativo.");
                    return;
                }
            }

            _lastScanTime = now;
            RunScan();
        }

        private void RunScan()
        {
            // --- 1. Posição do jogador local ---
            Vector3 origin = Vector3.zero;
            try
            {
                var localPlayer = SemiFunc.PlayerAvatarLocal();
                if (localPlayer == null)
                {
                    Log.LogWarning("[LootPulse] PlayerAvatarLocal retornou null. Abortando scan.");
                    return;
                }
                if (localPlayer.playerTransform == null)
                {
                    Log.LogWarning("[LootPulse] playerTransform é null. Abortando scan.");
                    return;
                }
                origin = localPlayer.playerTransform.position;
            }
            catch (Exception ex)
            {
                Log.LogWarning($"[LootPulse] Erro ao obter posição do jogador: {ex.Message}. Abortando.");
                return;
            }

            // --- 2. Buscar valuables no range ---
            List<PhysGrabObject> candidates = null;
            bool usedSemiFunc = false;

            try
            {
                candidates = SemiFunc.PhysGrabObjectAllValuablesWithinRange(
                    ScanRange.Value, origin, false, default(LayerMask));
                usedSemiFunc = true;
            }
            catch (Exception ex)
            {
                Log.LogWarning($"[LootPulse] PhysGrabObjectAllValuablesWithinRange falhou ({ex.Message}). Usando fallback.");
                candidates = FindValuablesFallback(origin, ScanRange.Value);
            }

            int total    = candidates?.Count ?? 0;
            int brackets = 0;
            int mapIcons = 0;
            int errors   = 0;

            Log.LogInfo(
                $"[LootPulse] Scan executado | origem={origin:F1} | range={ScanRange.Value}m | " +
                $"api={(usedSemiFunc ? "SemiFunc.PhysGrabObjectAllValuablesWithinRange" : "FindObjectsOfType fallback")} | " +
                $"encontrados={total}");

            // --- 3. Processar cada item ---
            if (candidates != null)
            {
                foreach (var pgo in candidates)
                {
                    if (pgo == null) continue;
                    try
                    {
                        var vo = pgo.GetComponent<ValuableObject>();
                        if (vo == null)
                        {
                            if (VerboseLogging.Value)
                                Log.LogDebug($"[LootPulse] Sem ValuableObject em: {pgo.name}");
                            continue;
                        }

                        if (VerboseLogging.Value)
                            Log.LogDebug($"[LootPulse] Processando: {pgo.name}");

                        if (EnableVisualBrackets.Value && TryApplyBrackets(vo))
                            brackets++;

                        if (EnableMapIcons.Value && TryAddToMap(vo))
                            mapIcons++;
                    }
                    catch (Exception ex)
                    {
                        errors++;
                        Log.LogWarning($"[LootPulse] Erro ao processar {pgo?.name}: {ex.Message}");
                    }
                }
            }

            Log.LogInfo(
                $"[LootPulse] Resultado | brackets={brackets}/{total} | " +
                $"mapa={mapIcons}/{total} | erros={errors}");
        }

        // --- API de bracket visual ---

        private static bool TryApplyBrackets(ValuableObject vo)
        {
            try
            {
                ResolveDiscoverApi();

                if (_discoverWithState != null)
                {
                    _discoverWithState.Invoke(vo, new[] { _discoverStateValue });
                    return true;
                }

                if (_discoverNoParam != null)
                {
                    _discoverNoParam.Invoke(vo, null);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (VerboseLogging.Value)
                    Log?.LogDebug($"[LootPulse] Discover exception: {ex.Message}");
                return false;
            }
        }

        private static void ResolveDiscoverApi()
        {
            if (_discoverApiChecked) return;
            _discoverApiChecked = true;

            var voType  = typeof(ValuableObject);
            var pubInst = BindingFlags.Public | BindingFlags.Instance;

            // Localizar ValuableDiscoverGraphic por reflexão — resiliente a renomeação futura
            Type graphicType = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                graphicType = asm.GetType("ValuableDiscoverGraphic");
                if (graphicType != null) break;
            }

            if (graphicType != null)
            {
                var stateType = graphicType.GetNestedType("State", BindingFlags.Public);
                if (stateType != null && stateType.IsEnum)
                {
                    var m = voType.GetMethod("Discover", pubInst, null, new[] { stateType }, null);
                    if (m != null)
                    {
                        _discoverWithState  = m;
                        _discoverStateValue = Enum.Parse(stateType, "Discover");
                        Log?.LogInfo("[LootPulse] Discover API resolvida: ValuableObject.Discover(ValuableDiscoverGraphic.State)");
                        return;
                    }
                }
            }

            // Fallback: Discover() sem parâmetro (API de builds antigas)
            var legacy = voType.GetMethod("Discover", pubInst, null, Type.EmptyTypes, null);
            if (legacy != null)
            {
                _discoverNoParam = legacy;
                Log?.LogInfo("[LootPulse] Discover API resolvida: ValuableObject.Discover() [fallback sem parâmetro]");
                return;
            }

            Log?.LogWarning("[LootPulse] Discover API não encontrada em nenhuma forma. Brackets visuais desabilitados.");
        }

        // --- API de mapa ---

        private static bool TryAddToMap(ValuableObject vo)
        {
            try
            {
                var mapInst = Map.Instance;
                if (mapInst == null)
                {
                    if (VerboseLogging.Value)
                        Log?.LogDebug("[LootPulse] Map.Instance é null.");
                    return false;
                }
                mapInst.AddValuable(vo);
                return true;
            }
            catch (Exception ex)
            {
                if (VerboseLogging.Value)
                    Log?.LogDebug($"[LootPulse] Map.AddValuable falhou: {ex.Message}");
                return false;
            }
        }

        // --- Fallback de busca de valuables ---

        private static List<PhysGrabObject> FindValuablesFallback(Vector3 origin, float range)
        {
            var result  = new List<PhysGrabObject>();
            float sqRange = range * range;

            var allVo = FindObjectsOfType<ValuableObject>();
            foreach (var vo in allVo)
            {
                if (vo == null) continue;
                if ((vo.transform.position - origin).sqrMagnitude > sqRange) continue;
                var pgo = vo.GetComponent<PhysGrabObject>();
                if (pgo != null) result.Add(pgo);
            }

            Log?.LogInfo($"[LootPulse] Fallback: {result.Count} valuables encontrados via FindObjectsOfType.");
            return result;
        }
    }
}
