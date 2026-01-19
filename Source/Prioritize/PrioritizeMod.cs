using Mlie;
using UnityEngine;
using Verse;

namespace Prioritize;

[StaticConstructorOnStartup]
internal class PrioritizeMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static PrioritizeMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public PrioritizeMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<PrioritizeSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal PrioritizeSettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Prioritize";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("P_UseUnsafePatchesTitle".Translate(), ref Settings.UseUnsafePatches,
            "P_UseUnsafePatchesDesc".Translate());
        listingStandard.CheckboxLabeled("P_UseLowerAsHighPriorityTitle".Translate(),
            ref Settings.UseLowerAsHighPriority,
            "P_UseLowerAsHighPriorityDesc".Translate());
        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("P_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}