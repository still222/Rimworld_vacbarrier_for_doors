using RimWorld;
using Verse;

namespace StkVacBarrier;
public class CompVacProofing : CompRefuelable
{
	public new CompProperties_VacProofing Props => (CompProperties_VacProofing)props;
	public override void PostExposeData()
	{
		// Old mod version compat
		if (Scribe.mode == LoadSaveMode.LoadingVars && Scribe.loader.curXmlParent["CompVacProofing.fuel"] == null)
		{
			base.PostExposeData();
			return;
		}

		Scribe_Values.Look(ref fuel, "CompVacProofing.fuel", 0f);
		Scribe_Values.Look(ref configuredTargetFuelLevel, "CompVacProofing.configuredTargetFuelLevel", -1f);
		Scribe_Values.Look(ref allowAutoRefuel, "CompVacProofing.allowAutoRefuel", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && !Props.showAllowAutoRefuelToggle)
		{
			allowAutoRefuel = Props.initialAllowAutoRefuel;
		}

	}

	public override void CompTick()
	{}

}

public class CompProperties_VacProofing : CompProperties_Refuelable
{
	public CompProperties_VacProofing()
	{
		compClass = typeof(CompVacProofing);

		fuelLabel = "Gravlite proofing";
		fuelGizmoLabel = "Integrated VacBarrier";
		outOfFuelMessage = "No VacBarrier";
		fuelCapacity = 3f;
		autoRefuelPercent = -1f;
		minimumFueledThreshold = 3f;
		consumeFuelOnlyWhenUsed = true;
		drawOutOfFuelOverlay = false;
		hideGizmosIfNotPlayerFaction = true;
	}

}
