using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iw5_gallery.BL.Messages;
using iw5_gallery.BL.Models;
using iw5_gallery.BL.Repositories;
using iw5_gallery.Commands;

namespace iw5_gallery.ViewModels
{
    public class PersonListViewModel : ViewModelBase
    {
        private readonly PersonRepository personRepository;
        private readonly IMessenger messenger;

        public ObservableCollection<PersonModel> Persons { get; set; } = new ObservableCollection<PersonModel>();

        public ICommand SelectPersonCommand { get; }

        public PersonListViewModel(PersonRepository personRepository, IMessenger messenger)
        {
            this.personRepository = personRepository;
            this.messenger = messenger;

            SelectPersonCommand = new RelayCommand(PersonSelectionChanged);

            this.messenger.Register<UpdatePersonListMessage>(NewPersonAdded);
        }

        private void NewPersonAdded(UpdatePersonListMessage obj)
        {
            OnLoad();
        }

        public void OnLoad()
        {
            Persons.Clear();

            var persons = personRepository.GetAll();
            foreach (var person in persons)
            {
                Persons.Add(person);
            }
        }

        private void PersonSelectionChanged(object parameter)
        {
            if (parameter is PersonModel person)
            {
                messenger.Send(new SelectedPersonMessage { Id = person.Id });
            }
        }
    }

}
