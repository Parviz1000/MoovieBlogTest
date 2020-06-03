using MoovieBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoovieBlog.ViewModels
{
    public class MooviesViewModel
    {
        public IEnumerable<Moovie> Movies { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public User User { get; set; }
    }
}
