using HarmonyLib;
using RimWorld;

namespace StkVacBarrier;

[HarmonyPatch(typeof(Building_Door))]
[HarmonyPatch("get_ExchangeVacuum")]
public static class Patch_IntegratedVacBarrierDoors
{
	public static bool Prefix(ref bool __result, Building_Door __instance)
	{
		// Common checks
		var compPower = __instance.GetComp<CompPowerTrader>();
		bool baseExchangeVacuum = !__instance.IsAirtight || __instance.def.building.alwaysExchangeVacuum;

		if (compPower == null || !compPower.PowerOn || baseExchangeVacuum)
			return true; // no power or not airtight → run original


		// Check if panels installed
		if (VacDoors.stkFueledVacDoors)
		{
			var compRefuel = __instance.GetComp<CompRefuelable>();
			if (compRefuel != null && compRefuel.HasFuel)
			{
				__result = false; // vacuum barrier active
				return false;
			}
		}

		// If cheat enabled, require research
		if (VacDoors.stkEnableVacDoors)
		{
			if (Startup.OrbitalTech != null && Startup.OrbitalTech.IsFinished)
			{
				__result = false; // vacuum barrier active
				return false;
			}
		}

		return true; // run vanilla
	}
}


[HarmonyPatch(typeof(Building_Door))]
[HarmonyPatch("get_TempEqualizeRate")]
public static class Patch_TempEqualizeRate
{
	public static bool Prefix(ref float __result, Building_Door __instance)
	{
		// Common checks
		var compPower = __instance.GetComp<CompPowerTrader>();
		if (compPower == null || !compPower.PowerOn)
			return true; // no power, run original


		// Check if panels installed
		if (VacDoors.stkFueledVacDoors)
		{
			var compRefuel = __instance.GetComp<CompRefuelable>();
			if (compRefuel != null && compRefuel.HasFuel)
			{
				__result = 0f; // vacuum barrier active
				return false;
			}
		}

		// If cheat enabled, require research
		if (VacDoors.stkEnableVacDoors)
		{
			if (Startup.OrbitalTech != null && Startup.OrbitalTech.IsFinished)
			{
				__result = 0f;
				return false; // skip original
			}
		}

		return true; // otherwise, run original
	}
}