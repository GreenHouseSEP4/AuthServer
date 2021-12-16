using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MoneyTrackDatabaseAPI.Models
{
    public class Device
    {
        [Key]
        public string Eui { get; set; }

        public Device(string eui)
        {
            Eui = eui;
        }
    }
    
}