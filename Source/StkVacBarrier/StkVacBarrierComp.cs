using System.Collections.Generic;
using Multiplayer.API;
using RimWorld;
using UnityEngine;
using Verse;

namespace StkVacBarrier;
public class CompVacProofing : CompRefuelable
{
	public new CompProperties_VacProofing Props => (CompProperties_VacProofing)props;
	public bool requestedRefuel = false;	// To request gravlite through button
	private Building_Door door;
	public override void PostExposeData()
	{
		// Old mod version compat
		if (Scribe.mode == LoadSaveMode.LoadingVars && Scribe.loader.curXmlParent["CompVacProofing.fuel"] == null)
		{
			base.PostExposeData();
			return;
		}

		Scribe_Values.Look(ref fuel, "CompVacProofing.fuel", 0f);
		Scribe_Values.Look(ref requestedRefuel, "CompVacProofing.requestedRefuel", false);
		Scribe_Values.Look(ref configuredTargetFuelLevel, "CompVacProofing.configuredTargetFuelLevel", -1f);
		Scribe_Values.Look(ref allowAutoRefuel, "CompVacProofing.allowAutoRefuel", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && !Props.showAllowAutoRefuelToggle)
			allowAutoRefuel = Props.initialAllowAutoRefuel;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);

		door = (Building_Door)parent;
	}

	public override void ReceiveCompSignal(string signal)
	{
		if (IsFull && signal == "Refueled")
			requestedRefuel = false;
	}

	public override string CompInspectStringExtra()
	{
		if (door == null || !door.IsAirtight)
			return "";

		if (!HasFuel && !Props.outOfFuelMessage.NullOrEmpty())
			return $"{Props.outOfFuelMessage} ({GetFuelCountToFullyRefuel()}x {Props.fuelFilter.AnyAllowedDef.label})";

		return $"{Props.fullyFueled}";
	}

	public override void CompTick()
	{}

	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		if (door == null || !door.IsAirtight || parent.Faction != Faction.OfPlayer)
			yield break;

		foreach (var gizmo in base.CompGetGizmosExtra())
			yield return gizmo;

		if (IsFull)
			yield break;

		// TODO: Sound from clicking
		Command_Action command_Action;
		if (requestedRefuel)
		{
			command_Action = new()
			{
				defaultLabel = "StkCancelRequestGravliteButton".TranslateSimple(),
				defaultDesc = "StkCancelRequestGravliteButtonDesc".TranslateSimple(),
				icon = ContentFinder<Texture2D>.Get("Things/Item/Resource/GravlitePanels/GravlitePanels_C"),
				action = CancelRefuel
			};
		}
		else
		{
			command_Action = new()
			{
				defaultLabel = "StkRequestGravliteButton".TranslateSimple(),
				defaultDesc = "StkRequestGravliteButtonDesc".TranslateSimple(),
				icon = ContentFinder<Texture2D>.Get("Things/Item/Resource/GravlitePanels/GravlitePanels_C"),
				action = RequestRefuel
			};
		}
		yield return command_Action;
	}

	[SyncMethod]
	private void RequestRefuel()
	{
		if (!IsFull)
			requestedRefuel = true;
	}

	[SyncMethod]
	private void CancelRefuel()
	{
		requestedRefuel = false;
	}

}

public class CompProperties_VacProofing : CompProperties_Refuelable
{
	public string fullyFueled = "VacBarrier installed";
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
