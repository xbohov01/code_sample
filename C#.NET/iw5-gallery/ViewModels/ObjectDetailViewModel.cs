using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using iw5_gallery.BL.Messages;
using iw5_gallery.BL.Models;
using iw5_gallery.BL.Repositories;
using iw5_gallery.Commands;

namespace iw5_gallery.ViewModels
{
    public class ObjectDetailViewModel : ViewModelBase
    {
        private readonly ObjectRepository objectRepository;
        private readonly ImageRepository imageRepository;
        private readonly IMessenger messenger;
        private ObjectModel model;
        private Guid selectedImageId;

        public ObjectModel Model
        {
            get { return model; }
            set
            {
                if (Equals(value, Model)) return;
                model = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveObjectCommand { get; }
        public ICommand DeleteObjectCommand { get; }
        public ICommand TagObject { get; }
        public ICommand SelectObjectPhotoCommand { get; }
        public ICommand UntagObjectCommand { get; }

        public ObjectDetailViewModel(ObjectRepository objectRepository, ImageRepository imageRepository, IMessenger messenger)
        {
            this.imageRepository = imageRepository;
            this.objectRepository = objectRepository;
            this.messenger = messenger;

            SaveObjectCommand = new SaveObjectCommand(objectRepository, this, messenger);
            DeleteObjectCommand = new RelayCommand(DeleteObject);
            TagObject = new RelayCommand(SendObjectForTag);
            SelectObjectPhotoCommand = new RelayCommand(ObjectPhotoSelectionChanged);
            UntagObjectCommand = new RelayCommand(UntagObjectFromPhoto);

            this.messenger.Register<SelectedObjectMessage>(SelectedObject);
            this.messenger.Register<NewObjectMessage>(NewObjectMessageReceived);
        }

        private void DeleteObject()
        {
            if (model == null) return;
            if (Model.Id != Guid.Empty)
            {
                var objectId = Model.Id;

                Model = new ObjectModel();
                objectRepository.RemoveObject(objectId);
                messenger.Send(new UpdateObjectListMessage());
            }
        }

        private void NewObjectMessageReceived(NewObjectMessage obj)
        {
            Model = new ObjectModel();
        }

        private void SelectedObject(SelectedObjectMessage message)
        {
            Model = objectRepository.GetById(message.Id);
        }

        private void SendObjectForTag()
        {
            if (Model == null)
            {
                return;
            }
            messenger.Send(new ObjectForTagMessage(Model.Id));
        }

        private void ObjectPhotoSelectionChanged(object parameter)
        {
            if (parameter is ImageDetailModel image)
            {
                selectedImageId = image.Id;
                messenger.Send(new SelectedImageMessage() { Id = image.Id });
            }
        }
        private void UntagObjectFromPhoto()
        {
            if (Model == null) return;
            imageRepository.UntagObjectInImage(Model.Id, selectedImageId);
            Model = objectRepository.GetById(Model.Id);
        }
    }
}
