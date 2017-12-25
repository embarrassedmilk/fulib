// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace func
// {
//     public class Request {}

//     public class Context {
//         public string Prop1 {get;set;}
//         public string Prop2 {get;set;}
//         public string Prop3 {get;set;}
//     }

//     public class MrChainer
//     {
//         private Action<Exception> _catcher;
        
        

//         public MrChainer WithFirstStep<T>(Func<T> firstStep) {
//             return this;
//         }

//         public MrChainer Catch(Action<Exception> act) {
//             _catcher = act;
//             return this;
//         }

//         public void ExecuteAsync() {
//             foreach (var step in _funcs) {
//                 try { 
//                     step(_context);
//                 }
//                 catch(Exception ex) {
//                     _catcher(ex);
//                 }
//             }
//         }
//     }

//     public class Toeter {
//         public Toeter()
//         {
            
//         }

//         public void ExecuteComplexTask(Request request) {
//             /*
//             new MrChainer()
//                 .WithFirstStep<int>(() => { return 1; })
//                 .WithNextStep<int, string>((intParam) => { return intParam.ToString(); })
//                 .WithNextStep<string, IEnumerable<string>>((stringParam) => { return new List<string>(stringParam); })
//                 .Catch((ex) => { Console.WriteLine(ex); })
//                 .Execute();
//              */
//             // new MrChainer<Context>()
//             //     .WithContext(() => new Context())
//             //     .WithStep((context) => { context.Prop1 = "1"; })
//             //     .WithStep((context) => { context.Prop1 = "2"; })
//             //     .WithStep((context) => { context.Prop1 = "3"; })
//             //     .Catch((ex) => { Console.WriteLine(ex); })
//             //     .ExecuteAsync();

//             new MrChainer()
//                 .WithFirstStep<int>(() => { return 1; })
//                 .WithNextStep<int, string>((intParam) => { return intParam.ToString(); })
//                 .WithNextStep<string, IEnumerable<string>>((stringParam) => { return new List<string>(stringParam); })
//                 .Catch((ex) => { Console.WriteLine(ex); })
//                 .Execute();
//         }
//     }
// }