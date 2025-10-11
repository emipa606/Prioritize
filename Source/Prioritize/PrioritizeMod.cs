using Mlie;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Prioritize;

internal class PrioritizeMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static PrioritizeMod instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public PrioritizeMod(ModContentPack content) : base(content)
    {
        instance = this;
        Settings = GetSettings<PrioritizeSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        new Harmony("Mlie.Prioritize").PatchAll(Assembly.GetExecutingAssembly());
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("P_UseUnsafePatchesTitle".Translate(), ref Settings.UseUnsafePatches,
            "P_UseUnsafePatchesDesc".Translate());
        listing_Standard.CheckboxLabeled("P_UseLowerAsHighPriorityTitle".Translate(),
            ref Settings.UseLowerAsHighPriority,
            "P_UseLowerAsHighPriorityDesc".Translate());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("P_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}