using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LootPulse
{
    [BepInPlugin("com.hiarlyscripter.repo.lootpulse", "LootPulse", "1.0.6")]
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
        private int   _lastTickFrame = -1;
        private bool  _harmonyLoopLogged;
        private bool  _pluginLoopLogged;

        // Set once when the patch installs; used by the static postfix to pass the source name
        private static string _patchedMethodName;

        // Discover/Map API resolved once per session
        private static bool       _discoverApiChecked;
        private static MethodInfo _discoverWithState;
        private static MethodInfo _discoverNoParam;
        private static object     _discoverStateValue;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            ScanKey = Config.Bind("Scanner", "ScanKey", KeyCode.F,
                "Key that triggers the valuable scanner.");
            ScanRange = Config.Bind("Scanner", "ScanRange", 30f,
                new ConfigDescription("Detection radius in meters.", new AcceptableValueRange<float>(5f, 200f)));
            CooldownSeconds = Config.Bind("Scanner", "CooldownSeconds", 4f,
                new ConfigDescription("Minimum seconds between consecutive scans.", new AcceptableValueRange<float>(0f, 60f)));
            EnableVisualBrackets = Config.Bind("Scanner", "EnableVisualBrackets", true,
                "Display visual discovery brackets on valuables found.");
            EnableMapIcons = Config.Bind("Scanner", "EnableMapIcons", true,
                "Add valuable icon to the map when scanning.");
            RequireLevelOrRun = Config.Bind("Scanner", "RequireLevelOrRun", true,
                "Only scan during active runs (generated level). Disable only for testing outside a level.");
            VerboseLogging = Config.Bind("Scanner", "VerboseLogging", false,
                "Log details for each individually processed item.");

            Log.LogInfo($"v1.0.6 loaded. Press {ScanKey.Value} to scan valuables.");
            Log.LogInfo(
                $"Config | key={ScanKey.Value} | range={ScanRange.Value} | cooldown={CooldownSeconds.Value} | " +
                $"visualBrackets={EnableVisualBrackets.Value} | mapIcons={EnableMapIcons.Value} | " +
                $"requireLevelOrRun={RequireLevelOrRun.Value} | verbose={VerboseLogging.Value}");

            // Build identity
            var asm = Assembly.GetExecutingAssembly();
            Log.LogInfo("Build marker: v1.0.6-release");
            Log.LogInfo($"Assembly location: {asm.Location}");
            try { Log.LogInfo($"Assembly file time UTC: {File.GetLastWriteTimeUtc(asm.Location):yyyy-MM-dd HH:mm:ss}"); }
            catch { /* non-critical */ }

            // Install Harmony tick patch with resilient fallback chain
            InstallHarmonyPatch(new Harmony("com.hiarlyscripter.repo.lootpulse"));
        }

        // Fallback: plugin's own MonoBehaviour Update() — may or may not fire depending on game behavior
        private void Update()
        {
            if (!_pluginLoopLogged)
            {
                _pluginLoopLogged = true;
                Log.LogInfo("Update loop active. (plugin MonoBehaviour)");
            }
            Tick("Plugin.Update");
        }

        // Called by HarmonyTickPostfix — primary tick path
        internal void TickFromGameLoop(string source)
        {
            if (!_harmonyLoopLogged)
            {
                _harmonyLoopLogged = true;
                Log.LogInfo($"Harmony game loop active: {source}");
            }
            Tick(source);
        }

        // Core tick — deduplicated per frame regardless of which source calls it
        private void Tick(string source)
        {
            int frame = Time.frameCount;
            if (frame == _lastTickFrame) return;
            _lastTickFrame = frame;

            // Primary key check
            bool keyPressed = Input.GetKeyDown(ScanKey.Value);
            // Fallback: if config value deserialized incorrectly, also accept literal KeyCode.F
            if (!keyPressed && ScanKey.Value != KeyCode.F)
                keyPressed = Input.GetKeyDown(KeyCode.F);

            if (!keyPressed) return;

            Log.LogInfo($"Scan key detected: {ScanKey.Value} | source={source}");

            float now     = Time.time;
            float elapsed = now - _lastScanTime;

            if (elapsed < CooldownSeconds.Value)
            {
                Log.LogInfo($"Scan blocked: cooldown active ({(CooldownSeconds.Value - elapsed):F1}s remaining)");
                return;
            }

            if (RequireLevelOrRun.Value)
            {
                bool inLevel = false;
                try { inLevel = SemiFunc.RunIsLevel(); }
                catch (Exception ex)
                {
                    Log.LogWarning($"RunIsLevel check failed ({ex.Message}). Proceeding without validation.");
                    inLevel = true;
                }
                if (!inLevel)
                {
                    Log.LogInfo("Scan blocked: not in active level/run (RequireLevelOrRun=true)");
                    return;
                }
            }

            _lastScanTime = now;
            RunScan();
        }

        // ── Harmony patch installer ───────────────────────────────────────────────────

        private static void InstallHarmonyPatch(Harmony harmony)
        {
            // Fallback chain — first patchable method wins.
            // Uses string-based runtime type resolution: resilient to future class renames.
            var candidates = new[]
            {
                ("GameDirector",     "Update"),
                ("PlayerAvatar",     "Update"),
                ("PlayerController", "Update"),
            };

            var postfixRef = new HarmonyMethod(
                typeof(LootPulsePlugin).GetMethod(
                    "HarmonyTickPostfix",
                    BindingFlags.Static | BindingFlags.NonPublic));

            foreach (var (typeName, methodName) in candidates)
            {
                try
                {
                    // Resolve type at runtime across all loaded assemblies
                    Type type = null;
                    foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = a.GetType(typeName);
                        if (type != null) break;
                    }
                    if (type == null)
                    {
                        Log.LogWarning($"Harmony fallback: type not found — {typeName}");
                        continue;
                    }

                    // Resolve Update() with no parameters (Unity lifecycle signature)
                    var method = type.GetMethod(
                        methodName,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                        null, Type.EmptyTypes, null);

                    if (method == null)
                    {
                        Log.LogWarning($"Harmony fallback: method not found — {typeName}.{methodName}");
                        continue;
                    }

                    harmony.Patch(method, postfix: postfixRef);
                    _patchedMethodName = $"{typeName}.{methodName}";
                    Log.LogInfo($"Harmony tick patch installed: {_patchedMethodName}");

                    // Watchdog: warn if the patch installs but the game never calls the method
                    var watchedMethod = _patchedMethodName;
                    System.Threading.ThreadPool.QueueUserWorkItem(_ =>
                    {
                        System.Threading.Thread.Sleep(60000);
                        if (Instance != null && !Instance._harmonyLoopLogged)
                            Log?.LogWarning(
                                $"WARNING: Harmony patch installed but no game loop tick observed yet. " +
                                $"Patched method: {watchedMethod}");
                    });

                    return; // success — stop after first successful patch
                }
                catch (Exception ex)
                {
                    Log.LogWarning($"Harmony patch failed for {typeName}.{methodName}: {ex.Message}");
                }
            }

            Log.LogError("ERROR: no valid game loop method found. LootPulse cannot scan.");
        }

        // Static postfix called by the patched game method on every frame
        private static void HarmonyTickPostfix()
        {
            Instance?.TickFromGameLoop(_patchedMethodName);
        }

        // ── Scan logic ───────────────────────────────────────────────────────────────

        private void RunScan()
        {
            // 1. Local player position
            Vector3 origin = Vector3.zero;
            try
            {
                var localPlayer = SemiFunc.PlayerAvatarLocal();
                if (localPlayer == null)             { Log.LogInfo("Scan blocked: local player not found"); return; }
                if (localPlayer.playerTransform == null) { Log.LogInfo("Scan blocked: local player not found (transform null)"); return; }
                origin = localPlayer.playerTransform.position;
            }
            catch (Exception ex) { Log.LogWarning($"Scan blocked: local player not found ({ex.Message})"); return; }

            // 2. Find valuables in range
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
                Log.LogWarning($"PhysGrabObjectAllValuablesWithinRange failed ({ex.Message}). Using fallback.");
                try   { candidates = FindValuablesFallback(origin, ScanRange.Value); }
                catch (Exception exFb) { Log.LogWarning($"Scan blocked: scan API unavailable ({exFb.Message})"); return; }
            }

            if (candidates == null) { Log.LogWarning("Scan blocked: scan API unavailable (null result)"); return; }

            int total    = candidates.Count;
            int brackets = 0;
            int mapIcons = 0;
            int errors   = 0;

            Log.LogInfo(
                $"Scan executed | origin={origin:F1} | range={ScanRange.Value}m | " +
                $"api={(usedSemiFunc ? "SemiFunc" : "FindObjectsOfType")} | found={total}");

            // 3. Process each item
            foreach (var pgo in candidates)
            {
                if (pgo == null) continue;
                try
                {
                    var vo = pgo.GetComponent<ValuableObject>();
                    if (vo == null)
                    {
                        if (VerboseLogging.Value) Log.LogDebug($"No ValuableObject on: {pgo.name}");
                        continue;
                    }
                    if (VerboseLogging.Value) Log.LogDebug($"Processing: {pgo.name}");
                    if (EnableVisualBrackets.Value && TryApplyBrackets(vo)) brackets++;
                    if (EnableMapIcons.Value    && TryAddToMap(vo))        mapIcons++;
                }
                catch (Exception ex) { errors++; Log.LogWarning($"Error processing {pgo?.name}: {ex.Message}"); }
            }

            Log.LogInfo($"Result | brackets={brackets}/{total} | map={mapIcons}/{total} | errors={errors}");
        }

        // ── Discover API ─────────────────────────────────────────────────────────────

        private static bool TryApplyBrackets(ValuableObject vo)
        {
            try
            {
                ResolveDiscoverApi();
                if (_discoverWithState != null) { _discoverWithState.Invoke(vo, new[] { _discoverStateValue }); return true; }
                if (_discoverNoParam   != null) { _discoverNoParam.Invoke(vo, null); return true; }
                return false;
            }
            catch (Exception ex) { if (VerboseLogging.Value) Log?.LogDebug($"Discover exception: {ex.Message}"); return false; }
        }

        private static void ResolveDiscoverApi()
        {
            if (_discoverApiChecked) return;
            _discoverApiChecked = true;

            var voType  = typeof(ValuableObject);
            var pubInst = BindingFlags.Public | BindingFlags.Instance;

            Type graphicType = null;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                graphicType = a.GetType("ValuableDiscoverGraphic");
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
                        Log?.LogInfo("Discover API resolved: ValuableObject.Discover(ValuableDiscoverGraphic.State)");
                        return;
                    }
                }
            }

            var legacy = voType.GetMethod("Discover", pubInst, null, Type.EmptyTypes, null);
            if (legacy != null)
            {
                _discoverNoParam = legacy;
                Log?.LogInfo("Discover API resolved: ValuableObject.Discover() [no-param fallback]");
                return;
            }

            Log?.LogWarning("Discover API not found in any form. Visual brackets disabled.");
        }

        // ── Map API ──────────────────────────────────────────────────────────────────

        private static bool TryAddToMap(ValuableObject vo)
        {
            try
            {
                var mapInst = Map.Instance;
                if (mapInst == null) { if (VerboseLogging.Value) Log?.LogDebug("Map.Instance is null."); return false; }
                mapInst.AddValuable(vo);
                return true;
            }
            catch (Exception ex) { if (VerboseLogging.Value) Log?.LogDebug($"Map.AddValuable failed: {ex.Message}"); return false; }
        }

        // ── Valuable fallback search ──────────────────────────────────────────────────

        private static List<PhysGrabObject> FindValuablesFallback(Vector3 origin, float range)
        {
            var result    = new List<PhysGrabObject>();
            float sqRange = range * range;
            foreach (var vo in FindObjectsOfType<ValuableObject>())
            {
                if (vo == null) continue;
                if ((vo.transform.position - origin).sqrMagnitude > sqRange) continue;
                var pgo = vo.GetComponent<PhysGrabObject>();
                if (pgo != null) result.Add(pgo);
            }
            Log?.LogInfo($"Fallback: {result.Count} valuables found via FindObjectsOfType.");
            return result;
        }
    }
}
