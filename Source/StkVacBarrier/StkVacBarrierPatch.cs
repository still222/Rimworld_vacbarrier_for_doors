using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
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

[HarmonyPatch(typeof(RefuelWorkGiverUtility), nameof(RefuelWorkGiverUtility.CanRefuel))]
public static class Patch_RefuelWorkGiverUtility
{
	// For refuel button to work as upgrade request
	[HarmonyPostfix]
	public static void Postfix(ref bool __result, Pawn pawn, Thing t, bool forced)
	{
		if (__result == true || t is not Building_Door)
			return;

		CompVacProofing compVacProofing = t.TryGetComp<CompVacProofing>();
		if (compVacProofing == null || !compVacProofing.requestedRefuel || compVacProofing.IsFull)
			return;

		// Vanilla logic, starting after "if (!forced && !compRefuelable.ShouldAutoRefuelNow)" check
		if (!pawn.CanReserve(t, 1, -1, null, forced))
			return;

		if (t.Faction != pawn.Faction)
			return;

		if (RefuelWorkGiverUtility.FindBestFuel(pawn, t) == null)
		{
			ThingFilter fuelFilter = compVacProofing.Props.fuelFilter;
			JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter.Summary));
			return;
		}

		__result = true;
	}

}
