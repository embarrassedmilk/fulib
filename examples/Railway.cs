using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace func {
    public class Command {
        public string Id { get;set; }
        public string MessageId { get;set; }
    }

    public class CommandProcessor {
        public Task<Result<int>> HandleCommand(Command command) {
            return Task.FromResult(Result<int>.Success(1));
        }
    }

    public class MessageQueue {
        public Task<Result<IReadOnlyCollection<Command>>> GetCommands() {
            return Task.FromResult(Result<IReadOnlyCollection<Command>>.Success(new List<Command>{
                new Command {Id="bla1", MessageId="1"},
                new Command {Id="bla2", MessageId="2"}
            }));
        }

        public Task MarkCommandAsHandled(string messageId) {
            return Task.CompletedTask;
        }
    }

    public class CommandListener
    {
        private readonly CommandProcessor _commandProcessor;
        private readonly MessageQueue _messageQueue;

        public CommandListener(MessageQueue messageQueue, CommandProcessor allocateCommandProcessor)
        {
            _messageQueue = messageQueue;
            _commandProcessor = allocateCommandProcessor;
        }

        public Task HandleCommands()
        {
            return _messageQueue
                        .GetCommands()
                        .ThenVoid(messages => Task.WhenAll(messages.Select(HandleCommand)));
        }

        private Task HandleCommand(Command command)
        {
            return _commandProcessor
                        .HandleCommand(command)
                        .ThenVoid(c => _messageQueue.MarkCommandAsHandled(command.MessageId));
        }
    }


    public class ReadModel {
        public string Id { get;set; }
    }

    public class ReadModelDto {
        public string Id { get;set; }
    }

    public class WriteModel {
        public string Id { get;set; }
    }

    public class WriteModelDto {
        public string Id { get;set; }
    }

    public class DatabaseReader {
        public static Result<List<ReadModelDto>> RetrieveFromDb(string sql) {
            return ExecuteWithPolicy(() => new List<ReadModelDto>{ new ReadModelDto{ Id = "bla" } });
        }

        private static Result<T> ExecuteWithPolicy<T>(Func<T> func) {
            return Result<T>.Success(func());
        }
    }

    // public class DatabaseWriterLowLevel {
    //     public static void WriteItemsLowLevel(IReadOnlyCollection<WriteModelDto> writeModels) {
    //         if (writeModels.Count == 4) {
    //             throw new Exception("Wrong stuff");
    //         }
    //     }
    // }
    
    // public class DatabaseWriter {
    //     public static Result<IReadOnlyCollection<WriteModel>> WriteItems(IReadOnlyCollection<WriteModel> writeModels) {

    //     }
    // }

    public class DatabaseItemsRepo {
        public Task<Result<IReadOnlyCollection<ReadModel>>> GetItems() {
            return GetQueryText().Then(sql => DatabaseReader.RetrieveFromDb(sql).Map(ConvertToItems));
        }

        private IReadOnlyCollection<ReadModel> ConvertToItems(List<ReadModelDto> dto) {
            return dto.Select(ToItem).ToList().AsReadOnly();
        }

        private ReadModel ToItem(ReadModelDto dto) {
            return new ReadModel { Id = dto.Id };
        }

        private Task<Result<string>> GetQueryText() {
            return Task.FromResult(Result<string>.Success("bla"));
        }
    }
}