using HarmonyLib;
using RimWorld;

namespace StkVacBarrier;

[HarmonyPatch(typeof(Building_Door), nameof(Building_Door.ExchangeVacuum), MethodType.Getter)]
public static class Patch_IntegratedVacBarrierDoors
{
	[HarmonyPostfix]
	public static void Postfix(ref bool __result, Building_Door __instance)
	{
		if (__result == false || !__instance.IsAirtight || __instance.def.building.alwaysExchangeVacuum)
			return;

		var compPower = __instance.GetComp<CompPowerTrader>();
		if (compPower == null || !compPower.PowerOn)
			return;

		// If cheat enabled, require research
		if (VacDoors.stkCheatVacDoors && OrbitalResearchDefOf.OrbitalTech.IsFinished)
		{
			__result = false;
			return;
		}

		var compVacProof = __instance.GetComp<CompVacProofing>();
		if (compVacProof != null && compVacProof.HasFuel)
			__result = false; // vacuum barrier active
	}

}


[HarmonyPatch(typeof(Building_Door), nameof(Building_Door.TempEqualizeRate), MethodType.Getter)]
public static class Patch_TempEqualizeRate
{
	[HarmonyPostfix]
	public static void Postfix(ref float __result, Building_Door __instance)
	{
		if (__result == 0f)
			return;

		var compPower = __instance.GetComp<CompPowerTrader>();
		if (compPower == null || !compPower.PowerOn)
			return;

		// If cheat enabled, require research
		if (VacDoors.stkCheatVacDoors && OrbitalResearchDefOf.OrbitalTech.IsFinished)
		{
			__result = 0f;
			return;
		}

		var compRefuel = __instance.GetComp<CompVacProofing>();
		if (compRefuel != null && compRefuel.HasFuel)
			__result = 0f; // vacuum barrier active
	}

}

