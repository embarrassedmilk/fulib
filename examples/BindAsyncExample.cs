using System;
using System.Threading.Tasks;
using LanguageExt;

namespace func {
    public class LocationRepositoryAsync {
        public async Task<Option<Location>> Get(int id) {
            await Task.CompletedTask;
            return id % 2 == 0 ? Option<Location>.Some(new Location(id)) : Option<Location>.None;
        }
    }

    public class ClientRepositoryAsync {
        public async Task<Option<Client>> Get(int id) {
            await Task.CompletedTask;
            return id % 2 == 0 ? Option<Client>.Some(new Client(id)) : Option<Client>.None;
        }
    }

    public class ItemRepositoryAsync {
        public async Task<Option<Item>> Get(int id) {
            await Task.CompletedTask;
            return id % 2 == 0 ? Option<Item>.Some(new Item(id)) : Option<Item>.None;
        }
    }

    public class BindAsyncExample {
        private readonly LocationRepositoryAsync _locationRepository = new LocationRepositoryAsync();
        private readonly ClientRepositoryAsync _clientRepository = new ClientRepositoryAsync();
        private readonly ItemRepositoryAsync _itemRepository = new ItemRepositoryAsync();

        public async Task SendEmail(int locationId, int clientId, int itemId) {
            
            var letter = _locationRepository.Get(locationId)
                .BindAsync(loc =>  _clientRepository.Get(clientId)
                .BindAsync(cl => _itemRepository.Get(itemId)
                .BindAsync(item => CreateActualEmail(loc, cl, item))));

            await letter.Match(
                Some: val => Console.WriteLine(val),
                None: () => Console.WriteLine("Smth went wrong...")
            );
        }

        private Option<string> CreateActualEmail(Location location, Client client, Item item) {
            return Option<string>.Some($"{item.Name} for {client.Name} will be delivered to {location.Name}");
        }
    }
}