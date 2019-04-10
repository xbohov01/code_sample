using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iw5_gallery.BL.Models;
using iw5_gallery.DAL;

namespace iw5_gallery.BL.Repositories
{
    public class ObjectRepository
    {
        private readonly Mapper mapper = new Mapper();

        public ObjectModel GetObjectByName(string name)
        {
            using (var galleryDbContext = new GalleryDbContext())
            {
                var entity = galleryDbContext.Objects.FirstOrDefault(o => o.Name == name);
                return mapper.MapEntityTObjectModel(entity);
            }
        }

        public ObjectModel InsertObject(ObjectModel obj)
        {
            using (var context = new GalleryDbContext())
            {
                var entity = mapper.MapObjectModelToEntityObject(obj);
                entity.ObjectId = Guid.NewGuid();
                context.Objects.Add(entity);
                context.SaveChanges();

                return mapper.MapEntityObjectToObjectModel(entity);
            }
        }

        public void UpdateObject(ObjectModel obj)
        {
            using (var galleryDbContext = new GalleryDbContext())
            {
                var entity = galleryDbContext.Objects.First(o => o.ObjectId == obj.Id);

                entity.Name = obj.Name;
                entity.Tags = mapper.MapImageModelListToImageEntityList(obj.Photos);

                galleryDbContext.SaveChanges();
            }
        }

        public void RemoveObject(Guid id)
        {
            using (var galleryDbContext = new GalleryDbContext())
            {
                var entity = galleryDbContext.Objects.First(p => p.ObjectId == id);

                galleryDbContext.TagSubjects.Attach(entity);

                galleryDbContext.TagSubjects.Remove(entity);

                try
                {
                    galleryDbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }

            }
        }

        public ObjectModel GetById(Guid id)
        {
            using (var galleryDbContext = new GalleryDbContext())
            {
                var objectEntity = galleryDbContext.Objects
                    .FirstOrDefault(r => r.ObjectId == id);

                return mapper.MapEntityObjectToObjectModel(objectEntity);
            }
        }

        public List<ObjectModel> GetAll()
        {
            using (var galleryDbContext = new GalleryDbContext())
            {
                return galleryDbContext.Objects
                    .Select(mapper.MapEntityObjectToObjectModel)
                    .ToList();
            }
        }
    }
}
