namespace SdvCode.Areas.PrivateChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using SdvCode.Areas.PrivateChat.Models.Enums;

    public class Emoji
    {
        public Emoji()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        [EnumDataType(typeof(EmojiType))]
        public EmojiType EmojiType { get; set; }
    }
}