namespace ReviewWebsite.Models.ViewModel
{
    public class ResponseViewModel
    {
        public required String Code { get; set; }
        public required String Message { get; set; }
        public required String Data { get; set; }
        public required String ExtraData { get; set; }
    }
}
