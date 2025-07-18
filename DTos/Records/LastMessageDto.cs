namespace Web_chat.DTos.Records;

public record LastMessageDto(
    int ConversationId,
    string SenderId,
    string Text,
    DateTime Timestamp
);