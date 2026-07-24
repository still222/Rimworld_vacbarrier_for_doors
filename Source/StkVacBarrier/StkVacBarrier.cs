using HarmonyLib;
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
	}
}

[DefOf]
public class OrbitalResearchDefOf
{
	public static ResearchProjectDef OrbitalTech;
}
