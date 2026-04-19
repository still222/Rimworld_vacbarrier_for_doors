using HarmonyLib;
using Verse;

namespace StkVacBarrier
{
	[StaticConstructorOnStartup]
	public static class Startup
	{
		public static readonly ResearchProjectDef OrbitalTech = 
			DefDatabase<ResearchProjectDef>.GetNamedSilentFail("OrbitalTech");

		static Startup()
		{
			var harmony = new Harmony("stk.vacbarrierenabler");
			harmony.PatchAll();

			if (OrbitalTech == null)
				Log.Warning($"[StkVacBarrier] Orbital tech def couldn't be found for some reason");
		}
	}
}