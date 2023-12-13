using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using increasedDeadlines;

namespace IncreasedDeadlines.NewQuotaPatch {
    [HarmonyPatch(typeof(TimeOfDay))]
    public class QuotaPatch {
        [HarmonyPatch(nameof(TimeOfDay.SetNewProfitQuota))]
        [HarmonyPostfix]
        static void NewQuota(TimeOfDay __instance) {
            bool isHost = RoundManager.Instance.IsHost;
            int checkQuota = __instance.profitQuota / IncreasedDeadlinesMod.quotaCheck;

            if (checkQuota > 0) {
                if (isHost) {
                    __instance.timeUntilDeadline = (float)(__instance.quotaVariables.deadlineDaysAmount + checkQuota) * __instance.totalTime;
                    IncreasedDeadlinesMod.logSrc.LogInfo($"Quota check is {checkQuota}, timeUntilDeadline is {__instance.timeUntilDeadline}");
                    TimeOfDay.Instance.SyncTimeClientRpc(__instance.globalTime, (int)__instance.timeUntilDeadline);
                } else {
                    IncreasedDeadlinesMod.logSrc.LogInfo("Not host, not sending Rpc. Host will handle.");
                }
            }
        }
    }
}