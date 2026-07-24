using Verse;
using UnityEngine;

namespace StkVacBarrier;

public class VacDoors : ModSettings
{
	public static bool stkCheatVacDoors = false;
	public static bool stkVisualiseVacDoors = true;

	public override void ExposeData()
	{
		base.ExposeData();

		Scribe_Values.Look(ref stkCheatVacDoors, "stkEnableVacDoors", false);
		Scribe_Values.Look(ref stkVisualiseVacDoors, "stkVisualiseVacDoors", true);
	}

}

public class VacDoorsMod : Mod
{
	public VacDoorsMod(ModContentPack content) : base(content)
	{
		GetSettings<VacDoors>();
	}

	public override string SettingsCategory() => "StK Vac Doors";

	public override void DoSettingsWindowContents(Rect inRect)
	{
		Listing_Standard listing = new();
		listing.Begin(inRect);

		listing.Gap();

		listing.CheckboxLabeled("StkVisualiseVacDoorsLabel".Translate(),
			ref VacDoors.stkVisualiseVacDoors,
			"StkVisualiseVacDoorsDesc".Translate());

		if (Prefs.DevMode)
		{
			listing.Gap(30f);

			listing.CheckboxLabeled("StkVacDoorsLabel".Translate(),
				ref VacDoors.stkCheatVacDoors,
				"StkVacDoorsDesc".Translate());
		}

		listing.End();
	}

}
