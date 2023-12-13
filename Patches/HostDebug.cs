using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using increasedDeadlines;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.Netcode;

namespace IncreasedDeadlines.HostDebug {
    [HarmonyPatch(typeof(TimeOfDay), "Update")]
    public class Debug {
        static KeyControl debugKey = Keyboard.current.numpad5Key;
        [HarmonyPostfix]
        public static void DebugQuotaFulfilled(TimeOfDay __instance) {
            bool host = RoundManager.Instance.IsHost;
            if (debugKey.wasPressedThisFrame && IncreasedDeadlinesMod.configDebug.Value && host) {
                IncreasedDeadlinesMod.logSrc.LogInfo("Host pressed num5, increasing quota fulfilled by 100");
                IncreasedDeadlinesMod.logSrc.LogInfo($"Is host: {host}");
                __instance.quotaFulfilled += 100;
                StartOfRound.Instance.profitQuotaMonitorText.text = $"PROFIT QUOTA:\n${__instance.quotaFulfilled} / ${__instance.profitQuota}";
            } else if (debugKey.wasPressedThisFrame && !IncreasedDeadlinesMod.configDebug.Value && host) {
                IncreasedDeadlinesMod.logSrc.LogWarning("Host pressed num5, but debug mode is disabled");
            } else if (debugKey.wasPressedThisFrame && !host) {
                IncreasedDeadlinesMod.logSrc.LogWarning("Not host, wont do anything.");
            }
        }
    }
}