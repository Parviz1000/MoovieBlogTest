using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoovieBlog.Models
{
    public class Moovie
    {
        public string MoovieId { get; set; }
        public string UserEmail { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Director { get; set; }
        public string Date { get; set; }
        public string SelectedFile { get; set; }
        public string VideoLink { get; set; }

    }
}
