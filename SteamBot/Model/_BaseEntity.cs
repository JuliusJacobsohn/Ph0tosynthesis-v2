using System;
using System.ComponentModel.DataAnnotations;

namespace SteamBot.Model
{
    public class _BaseEntity
    {
        public int Id { get; set; }
        public DateTime EditedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public void OnCreate(string name)
        {
            CreatedDate = DateTime.Now;
            CreatedBy = name;
            OnEdited(name);
            Deleted = false;
        }
        public void OnEdited(string name)
        {
            EditedDate = DateTime.Now;
            EditedBy = name;
        }
    }
}