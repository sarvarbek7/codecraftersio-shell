public static class CharExtensions
{
    public static bool IsSingleQuote(this char c) => c == '\'';
    public static bool IsDoubleQuote(this char c) => c == '"';
    public static bool IsBackslash(this char c) => c == '\\';
}