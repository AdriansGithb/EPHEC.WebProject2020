using System;

namespace MVCClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string _Message { get; set; }

        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string message)
        {
            _Message = message;
        }


    }
}
