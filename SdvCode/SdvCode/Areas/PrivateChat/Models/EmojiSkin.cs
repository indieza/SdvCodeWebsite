using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.PrivateChat.Models
{
    public class EmojiSkin
    {
        public EmojiSkin()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int Position { get; set; }

        [ForeignKey(nameof(Emoji))]
        public string EmojiId { get; set; }

        public Emoji Emoji { get; set; }
    }
}