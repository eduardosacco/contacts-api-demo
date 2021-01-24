using System;

namespace ContactsAPI.Models
{
    public class ProfileImage
    {
        public long Id { get; set; }

        public long ContactRecordId { get; set; }

        public Byte[] Image { get; set; }
    }
}
