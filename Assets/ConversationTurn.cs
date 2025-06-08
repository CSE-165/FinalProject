using System;

public class ConversationTurn
{
    public string speaker;
    public string message;

    public ConversationTurn(string speaker, string message)
    {
        this.speaker = speaker;
        this.message = message;
    }
}