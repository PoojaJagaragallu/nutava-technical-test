namespace Nutava.Test.NumberToWord.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets/sets request id.
        /// </summary>
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
