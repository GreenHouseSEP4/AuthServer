using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MoneyTrackDatabaseAPI.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string Eui { get; set; }

        public Device(string eui)
        {
            Eui = eui;
        }

        public Device(int id, string eui)
        {
            this.Id = id;
            Eui = eui;
        }
    }
    
}