namespace PVOapi.Models
{
    public class PvoModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = "";
        public double? AverageValue { get; set; }

       

        // Status codes and messages
        public const int OK = 0;
        public const int WrongInputFormat = 1;
        public const int ErrorInCalculation = 2;
    }
}