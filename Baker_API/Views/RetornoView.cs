using System.Runtime.Serialization;

namespace Baker_API.Views
{
    public class RetornoView
    {
        [DataMember]
        public object? Data { get; set; }  
        public string? Mensagem {  get; set; }
        public string? StackTrace {  get; set; } 

    }
}
