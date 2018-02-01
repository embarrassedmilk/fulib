using System;
using System.Threading.Tasks;

namespace func {
    public class Location {
        public int Id { get; }
        public Location (int id) {
            this.Id = id;
        }

        public string Name => $"Location with id {Id}";
    }

    public class Client {
        public int Id { get; }
        public Client (int id) {
            this.Id = id;
        }

        public string Name => $"Client with id {Id}";
    }

    public class Item {
        public int Id { get; }
        public Item (int id) {
            this.Id = id;
        }

        public string Name => $"Item with id {Id}";
    }

    public class LocationRepositoryAsync {
        public Task<Result<Location>> Get(int id) {
            return id % 2 == 0 ? new Location(id).AsTaskResult() : Result<Location>.Failure("bla").AsTask();
        }
    }

    public class ClientRepositoryAsync {
        public Task<Result<Client>> Get(int id) {
            return id % 2 == 0 ? new Client(id).AsTaskResult() : Result<Client>.Failure("bla").AsTask();
        }
    }

    public class ItemRepositoryAsync {
        public Task<Result<Item>> Get(int id) {
            return id % 2 == 0 ? new Item(id).AsTaskResult() : Result<Item>.Failure("bla").AsTask();
        }
    }

    public class BindAsyncExample {
        private readonly LocationRepositoryAsync _locationRepository = new LocationRepositoryAsync();
        private readonly ClientRepositoryAsync _clientRepository = new ClientRepositoryAsync();
        private readonly ItemRepositoryAsync _itemRepository = new ItemRepositoryAsync();

        public Task SendEmail(int locationId, int clientId, int itemId) => 
            _locationRepository.Get(locationId)
                .BindTaskResult(loc => _clientRepository.Get(clientId)
                .BindTaskResult(cl => _itemRepository.Get(itemId)
                .MapTaskResult(item => CreateActualEmail(loc, cl, item))))
                .ThenVoid(email => Console.WriteLine(email));

        private Result<string> CreateActualEmail(Location location, Client client, Item item) => 
            "{item.Name} for {client.Name} will be delivered to {location.Name}".AsResult();
    }
}