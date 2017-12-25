using System;
using System.Threading.Tasks;
using LanguageExt;

namespace func {
    public class LocationRepositoryAsync {
        public Option<Location> Get(int id) {
            return id % 2 == 0 ? Option<Location>.Some(new Location(id)) : Option<Location>.None;
        }
    }

    public class ClientRepositoryAsync {
        public Option<Client> Get(int id) {
            return id % 2 == 0 ? Option<Client>.Some(new Client(id)) : Option<Client>.None;
        }
    }

    public class ItemRepositoryAsync {
        public Option<Item> Get(int id) {
            return id % 2 == 0 ? Option<Item>.Some(new Item(id)) : Option<Item>.None;
        }
    }

    public class AdvancedToeterAsync {
        private readonly LocationRepositoryAsync _locationRepository = new LocationRepositoryAsync();
        private readonly ClientRepositoryAsync _clientRepository = new ClientRepositoryAsync();
        private readonly ItemRepositoryAsync _itemRepository = new ItemRepositoryAsync();

        public void SendEmail(int locationId, int clientId, int itemId) {
            
            var letter = _locationRepository.Get(locationId)
                .Bind(location => _clientRepository.Get(clientId)
                .Bind(client => _itemRepository.Get(itemId)
                .Bind(item => CreateActualEmail(location, client, item))));
            
            // var letter = 
            //     from location in _locationRepository.Get(locationId)
            //     from client in _clientRepository.Get(clientId)
            //     from item in _itemRepository.Get(itemId)
            //     select CreateActualEmail(location, client, item);

            letter.Match(
                Some: val => Console.WriteLine(val),
                None: () => Console.WriteLine("Smth went wrong...")
            );
        }

        private Option<string> CreateActualEmail(Location location, Client client, Item item) {
            return Option<string>.Some($"{item.Name} for {client.Name} will be delivered to {location.Name}");
        }
    }
}