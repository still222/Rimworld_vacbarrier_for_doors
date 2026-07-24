using RimWorld;
using UnityEngine;
using Verse;

namespace StkVacBarrier;

public class CompMoteEmitterVacField : CompMoteEmitter
{
	public new CompProperties_MoteEmitterVacField Props => (CompProperties_MoteEmitterVacField)props;
	private CompVacProofing vacProofComp;
	private CompPowerTrader powerTraderComp;
	private Building_Door door;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);

		door = (Building_Door)parent;
		vacProofComp = parent.GetComp<CompVacProofing>();
		powerTraderComp = parent.GetComp<CompPowerTrader>();
	}

	public override void CompTick()
	{
		if (!VacDoors.stkVisualiseVacDoors || !parent.Spawned || door == null || vacProofComp == null || powerTraderComp == null)
			return;

		if (!powerTraderComp.PowerOn || !door.IsAirtight || door.Open ? !Props.activeWhenOpen : !Props.activeWhenClosed)
			return;

		if (!vacProofComp.HasFuel && !(VacDoors.stkCheatVacDoors && OrbitalResearchDefOf.OrbitalTech.IsFinished))
			return;

		if (mote == null || mote.Destroyed)
			Emit();

		else
			Maintain();
	}

	public override void Emit()
	{
		base.Emit();
		mote.instanceColor = Props.color;
	}

}

public class CompProperties_MoteEmitterVacField : CompProperties_MoteEmitter
{
	public Color color = new(0.6f, 0.8f, 1f, 0.35f);
	public bool activeWhenOpen;
	public bool activeWhenClosed;
	public CompProperties_MoteEmitterVacField()
	{
		compClass = typeof(CompMoteEmitterVacField);
	}

}
