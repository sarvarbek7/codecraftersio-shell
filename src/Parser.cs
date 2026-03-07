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
                }
                else if (backslashEncountered)
                {
                    currentTokenStringBuilder.Append(c);
                    backslashEncountered = false;
                }
                else
                {
                    doubleQuoteOpen = true;
                }
            }
            else if (c.IsBackslash())
            {
                if (singleQuoteOpen)
                {
                    currentTokenStringBuilder.Append(c);
                }
                else if (doubleQuoteOpen)
                {
                    if (backslashEncountered)
                    {
                        backslashEncountered = false;
                    }
                    else
                    {
                        currentTokenStringBuilder.Append(c);
                        backslashEncountered = true;
                    }
                }
                else if (backslashEncountered)
                {
                    currentTokenStringBuilder.Append(c);
                    backslashEncountered = false;
                }
                else
                {
                    backslashEncountered = true;
                }
            }
            else if (c.IsWhitespace())
            {
                if (singleQuoteOpen)
                {
                    currentTokenStringBuilder.Append(c);
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
                    var token = currentTokenStringBuilder.ToString();

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _tokens.Add(token);

                        currentTokenStringBuilder.Clear();
                    }
                }
            }
            else
            {
                if (singleQuoteOpen)
                {
                    currentTokenStringBuilder.Append(c);
                }
                else if (doubleQuoteOpen)
                {
                    if (backslashEncountered)
                    {
                        backslashEncountered = false;

                        if (c.IsBackslashEscapedInDoubleQuotes())
                        {
                            currentTokenStringBuilder.Remove(currentTokenStringBuilder.Length - 1, 1);
                            currentTokenStringBuilder.Append(c);
                        }
                        else
                        {
                            currentTokenStringBuilder.Append(c);
                        }
                    }
                    else
                    {
                        currentTokenStringBuilder.Append(c);
                    }
                }
                else if (backslashEncountered)
                {
                    currentTokenStringBuilder.Append(c);
                    backslashEncountered = false;
                }
                else
                {
                    currentTokenStringBuilder.Append(c);
                }
            }
        }
   
         var lastToken = currentTokenStringBuilder.ToString();

        if (!string.IsNullOrWhiteSpace(lastToken))
        {
            _tokens.Add(lastToken);

            currentTokenStringBuilder.Clear();
        }
    }
}