using System;
using System.Collections.Generic;
using System.Drawing;
using iw5_gallery.DAL.Entities;

namespace iw5_gallery.BL.Models
{
    public class ImageDetailModel
    {
        public ImageDetailModel(bool insert)
        {
            if(insert == true)
                Date = DateTime.Now;
        }
        public Guid Id { get; set; }
        public Image Image { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public ICollection<PersonModel> People { get; set; } = new List<PersonModel>();
        public ICollection<ObjectModel> Objects { get; set; } = new List<ObjectModel>();
        public ICollection<AlbumDetailModel> Albums { get; set; } = new List<AlbumDetailModel>();
    }
}