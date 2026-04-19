using RimWorld;
using UnityEngine;
using Verse;

namespace StkVacBarrier;
public class CompProperties_MoteEmitterVacField : CompProperties_MoteEmitter
{
	public Color color = new(0.6f, 0.8f, 1f, 0.35f);
	public bool triggerOnlyWhenOpen;
	public bool triggerOnlyWhenClosed;
	public CompProperties_MoteEmitterVacField()
	{
		compClass = typeof(CompMoteEmitterVacField);
	}
}

// Nomral CompMoteEmitter, but also checks for fueled status
public class CompMoteEmitterVacField : CompMoteEmitter
{
	public CompProperties_MoteEmitterVacField Props => (CompProperties_MoteEmitterVacField)props;
	private CompRefuelable cachedComp;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);

		cachedComp = parent.GetComp<CompRefuelable>();
	}

	public override void CompTick()
	{
		if (!VacDoors.stkVisualiseVacDoors)
			return;

		if (parent is Building_Door door)
		{
			if (!door.IsAirtight)
				return;

			if (!door.Open && Props.triggerOnlyWhenOpen)
				return;

			if (door.Open && Props.triggerOnlyWhenClosed)
				return;
		}

		bool hasFuel = VacDoors.stkFueledVacDoors && cachedComp != null && cachedComp.HasFuel;
		bool researchReady = VacDoors.stkEnableVacDoors && Startup.OrbitalTech != null && Startup.OrbitalTech.IsFinished;

		if (hasFuel || researchReady)
			base.CompTick();
	}

	public override void Emit()
	{
		base.Emit();
		mote.instanceColor = Props.color;
	}
}
