public static class CharExtensions
{
    public static bool IsSingleQuote(this char c) => c == CharRegistry.SingleQuote;
    public static bool IsDoubleQuote(this char c) => c == CharRegistry.DoubleQuote;
    public static bool IsBackslash(this char c) => c == CharRegistry.Backslash;
    public static bool IsDollarSign(this char c) => c == CharRegistry.DollarSign;
    public static bool IsBacktick(this char c) => c == CharRegistry.Backtick;
    public static bool IsNewLine(this char c) => c == CharRegistry.NewLine;
    public static bool IsCarriageReturn(this char c) => c == CharRegistry.CarriageReturn;
    public static bool IsTab(this char c) => c == CharRegistry.Tab;
    public static bool IsWhitespace(this char c) => char.IsWhiteSpace(c);

    public static bool IsBackslashEscapedInDoubleQuotes(this char c)
    {
        return c.IsDoubleQuote() || c.IsBackslash() || c.IsDollarSign() || c.IsBacktick() || c.IsNewLine();
    }
}