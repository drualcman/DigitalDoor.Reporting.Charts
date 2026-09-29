namespace DotNetBasics.Charts.Helpers;

internal static class StableIdentifier
{
    private const uint FnvOffsetBasis = 2166136261;
    private const uint FnvPrime = 16777619;

    public static string Create(string prefix, string content)
    {
        uint hash = FnvOffsetBasis;
        foreach (char character in content ?? string.Empty)
        {
            hash = unchecked((hash ^ character) * FnvPrime);
        }
        return $"{prefix}-{hash:x8}";
    }
}
