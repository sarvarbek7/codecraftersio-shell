using System.Text;

public class Parser
{
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
        bool backslashEncountered = false;

        var currentToken = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c.IsBackslash())
            {
                if (singleQuoteOpen || doubleQuoteOpen)
                {
                    currentToken.Append(c);
                    continue;
                }
                else if (backslashEncountered)
                {
                    currentToken.Append(c);
                    backslashEncountered = false;
                    continue;
                }
                else
                {
                    backslashEncountered = true;
                    continue;
                }

            }
            else if (c.IsSingleQuote() && !doubleQuoteOpen && !backslashEncountered)
            {
                singleQuoteOpen = !singleQuoteOpen;
                continue;
            }
            else if (c.IsDoubleQuote() && !singleQuoteOpen && !backslashEncountered)
            {
                doubleQuoteOpen = !doubleQuoteOpen;
                continue;
            }
            else if (char.IsWhiteSpace(c) && !(singleQuoteOpen || doubleQuoteOpen || backslashEncountered))
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

            if (backslashEncountered)
            {
                backslashEncountered = false;
            }
        }

        var lastToken = currentToken.ToString();

        if (!string.IsNullOrWhiteSpace(lastToken))
        {
            _tokens.Add(lastToken);

            currentToken.Clear();
        }
    }

    public void TokenizeV2(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        bool singleQuoteOpen = false;
        bool doubleQuoteOpen = false;
        bool backslashEncountered = false;

        var currentTokenStringBuilder = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c.IsSingleQuote())
            {
                if (singleQuoteOpen)
                {
                    singleQuoteOpen = false;
                }
                else if (doubleQuoteOpen)
                {
                    currentTokenStringBuilder.Append(c);
                }
                else if (backslashEncountered)
                {
                    currentTokenStringBuilder.Append(c);
                    backslashEncountered = false;
                }
                else
                {
                    singleQuoteOpen = true;
                }
            }
            else if (c.IsDoubleQuote())
            {
                if (doubleQuoteOpen)
                {
                    if (backslashEncountered)
                    {
                        currentTokenStringBuilder.Remove(currentTokenStringBuilder.Length - 1, 1);
                        currentTokenStringBuilder.Append(c);
                        backslashEncountered = false;
                    }
                    else
                    {
                        doubleQuoteOpen = false;
                    }
                }
                else if (singleQuoteOpen)
                {
                    currentTokenStringBuilder.Append(c);
                } else if (backslashEncountered)
                {
                    currentTokenStringBuilder.Append(c);
                    backslashEncountered = false;
                }
                else
                {
                    doubleQuoteOpen = true;
                }
            }
            else if (c.IsWhitespace())
            {
                
            }
        }
    }
}