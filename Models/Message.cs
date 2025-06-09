namespace Web_chat.Models;

public class Message
{
    public int Id { get; set; }                     // первичный ключ
    public string Sender { get; set; }              // userName
    public string Text { get; set; }                // само сообщение
    public DateTime Timestamp { get; set; }         // когда было отправлено
}