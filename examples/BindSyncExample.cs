using System;
using System.Threading.Tasks;
using LanguageExt;

namespace func {
    public class Location {
        public int Id { get; }
        public Location (int id) {
            this.Id = id;
        }

        public string Name => $"Location with id {Id}";
    }

    public class LocationRepository {
        public Option<Location> Get(int id) {
            return id % 2 == 0 ? Option<Location>.Some(new Location(id)) : Option<Location>.None;
        }
    }

    public class Client {
        public int Id { get; }
        public Client (int id) {
            this.Id = id;
        }

        public string Name => $"Client with id {Id}";
    }

    public class ClientRepository {
        public Option<Client> Get(int id) {
            return id % 2 == 0 ? Option<Client>.Some(new Client(id)) : Option<Client>.None;
        }
    }

    public class Item {
        public int Id { get; }
        public Item (int id) {
            this.Id = id;
        }

        public string Name => $"Item with id {Id}";
    }

    public class ItemRepository {
        public Option<Item> Get(int id) {
            return id % 2 == 0 ? Option<Item>.Some(new Item(id)) : Option<Item>.None;
        }
    }

    public class BindSyncExample {
        private readonly LocationRepository _locationRepository = new LocationRepository();
        private readonly ClientRepository _clientRepository = new ClientRepository();
        private readonly ItemRepository _itemRepository = new ItemRepository();

        public void SendEmail(int locationId, int clientId, int itemId) {
            
            //monadic approach
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