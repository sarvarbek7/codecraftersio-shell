using System.Text;

public class Parser
{
    private static string SINGLE_QUOTE = "'";
    private static string DOUBLE_QUOTE = "\"";

    private readonly List<string> _tokens = [];

    public List<string> Tokens => _tokens;

    public void Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        bool singleQuoteOpen = false;
        bool doubleQuoteOpen = false;

        var currentToken = new StringBuilder();

        foreach (char c in input)
        {
            if (c == SINGLE_QUOTE[0] && !doubleQuoteOpen)
            {
                singleQuoteOpen = !singleQuoteOpen;
                continue;
            }

            if (c == DOUBLE_QUOTE[0] && !singleQuoteOpen)
            {
                doubleQuoteOpen = !doubleQuoteOpen;
                continue;
            }

            if (char.IsWhiteSpace(c) && !(singleQuoteOpen || doubleQuoteOpen))
            {
                var token = currentToken.ToString();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _tokens.Add(token);

                    currentToken.Clear();
                }
                continue;
            }

            currentToken.Append(c);
        }

        var lastToken = currentToken.ToString();

        if (!string.IsNullOrWhiteSpace(lastToken))
        {
            _tokens.Add(lastToken);

            currentToken.Clear();
        }
    }
}