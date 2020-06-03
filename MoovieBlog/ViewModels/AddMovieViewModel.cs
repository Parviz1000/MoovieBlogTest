using MoovieBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace MoovieBlog.ViewModels
{
    public class AddMoovieViewModel
    {
        public User User{ get; set; }

        public string MoovieId { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }

        public string Director { get; set; }

        public string SelectedFile { get; set; }
        public string VideoLink { get; set; }

    }
}
