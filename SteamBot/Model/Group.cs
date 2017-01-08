using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamBot.Model
{
    public class Group : _BaseEntity
    {
        public string GroupId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
    }
}
