using System.Diagnostics.CodeAnalysis;

#pragma warning disable Danger_Identifier
var feature = new DangerFeature();
#pragma warning restore Danger_Identifier

[Experimental("Danger_Identifier")]
internal class DangerFeature
{
}