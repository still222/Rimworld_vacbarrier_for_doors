using RimWorld;
using UnityEngine;
using Verse;

namespace StkVacBarrier;

public class CompMoteEmitterVacField : CompMoteEmitter
{
	public new CompProperties_MoteEmitterVacField Props => (CompProperties_MoteEmitterVacField)props;
	private CompVacProofing cachedComp;
	private Building_Door door;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);

		cachedComp = parent.GetComp<CompVacProofing>();
		door = (Building_Door)parent;
	}

	public override void CompTick()
	{
		if (!VacDoors.stkVisualiseVacDoors)
			return;

		if (!door.IsAirtight ||
			(door.Open && Props.activeOnlyWhenClosed) ||
			(!door.Open && Props.activeOnlyWhenOpen))
			return;

		bool hasFuel = cachedComp != null && cachedComp.HasFuel;
		if (hasFuel || (VacDoors.stkCheatVacDoors && OrbitalResearchDefOf.OrbitalTech.IsFinished))
			base.CompTick();
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
	public bool activeOnlyWhenOpen;
	public bool activeOnlyWhenClosed;
	public CompProperties_MoteEmitterVacField()
	{
		compClass = typeof(CompMoteEmitterVacField);
	}

}

