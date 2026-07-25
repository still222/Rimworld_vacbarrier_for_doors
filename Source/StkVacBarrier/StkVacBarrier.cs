using HarmonyLib;
using Multiplayer.API;
using RimWorld;
using Verse;

namespace StkVacBarrier;

[StaticConstructorOnStartup]
public static class Startup
{
	static Startup()
	{
		var harmony = new Harmony("stk.vacbarrierenabler");
		harmony.PatchAll();

		if (MP.enabled)
			MP.RegisterAll();
	}
}

[DefOf]
public class OrbitalResearchDefOf
{
	public static ResearchProjectDef OrbitalTech;
}
