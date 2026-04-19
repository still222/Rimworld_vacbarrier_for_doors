using System.Xml;
using Verse;

namespace StkVacBarrier;
public class PatchOperationInsertWithSetting : PatchOperationInsert
{
	public string setting;
	public bool expected = true;
	public bool outputLog = false;
	public string settingLogName;

	protected override bool ApplyWorker(XmlDocument xml)
	{
		var field = typeof(VacDoors).GetField(setting);
			if (field == null)
			{
				Log.Error($"[StkVacBarrier] Could not find setting {setting} in VacDoors.");
				return false;
			}
		
		if (settingLogName == null)
			settingLogName = setting;

		// Skip if setting is disabled
		bool current = (bool)field.GetValue(null);
		if (current != expected)
		{
			if (outputLog)
				Log.Message($"[StkVacBarrier] Skipping patch based on '{settingLogName}' setting.");

			return true;
		}

		if (outputLog)
		{
			var nodes = xml.SelectNodes(xpath);
			if (nodes != null)
			{
				foreach (XmlNode node in nodes)
				{
					// climb to parent ThingDef
					XmlNode parentDef = node;
					while (parentDef != null && parentDef.Name != "ThingDef")
						parentDef = parentDef.ParentNode;

					string defName = parentDef?.SelectSingleNode("defName")?.InnerText ?? "UnknownDef";

					Log.Message($"[StkVacBarrier] Applied '{settingLogName}' patch to {defName}.");
				}
			}
		}
		return base.ApplyWorker(xml);
	}
}