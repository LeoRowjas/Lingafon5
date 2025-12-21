using System;
using Microsoft.AspNetCore.Http;

namespace Lingafon.API.Models
{
    public class VoiceMessageCreateRequest
    {
        public Guid DialogId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
