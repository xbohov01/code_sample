using System.Windows;
using System.Windows.Input;
using iw5_gallery.BL.Messages;
using iw5_gallery.Commands;
using iw5_gallery.Views;

namespace iw5_gallery.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessenger view_messenger;

        public ICommand AddPhoto { get; }
        public ICommand AddAlbum { get; }
        public ICommand AddPerson { get; }
        public ICommand AddObject { get; }

        public MainViewModel(IMessenger messenger)
        {
            view_messenger = messenger;
            AddPhoto = new RelayCommand(AddNewPhoto);
            AddAlbum = new RelayCommand(AddNewAlbum);
            AddPerson = new RelayCommand(AddNewPerson);
            AddObject = new RelayCommand(AddNewObject);

            //view_messenger.Register<AdjustImageWindow>(AdjustImageWindowSize);
        }

        private void AddNewPhoto()
        {
            var PhotoAddWindow = new ImageCreateWindow();
            view_messenger.Send(new NewImageMessage());
            PhotoAddWindow.ShowDialog();
        }

        private void AddNewAlbum()
        {
            var AlbumAddWindow = new AlbumCreateWindow();
            view_messenger.Send(new NewAlbumMessage());
            AlbumAddWindow.ShowDialog();
        }

        private void AddNewPerson()
        {
            var personAddWindow = new PersonCreateWindow();
            view_messenger.Send(new NewPersonMessage());
            personAddWindow.ShowDialog();
        }

        private void AddNewObject()
        {
            var objectAddWindow = new ObjectCreateWindow();
            view_messenger.Send(new NewObjectMessage());
            objectAddWindow.ShowDialog();
        }
    }
}
