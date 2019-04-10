using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iw5_gallery.Repositories;

namespace iw5_gallery.ViewModels
{
    public class ObjectViewModel : ViewModelBase
    {
        private readonly GalleryRepository galleryRepository;
        public ObjectViewModel(GalleryRepository repository)
        {
            this.galleryRepository = repository;
        }
    }
}
