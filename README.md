# fulib
[![Build status](https://ci.appveyor.com/api/projects/status/f9b5vpr6h5bm9wwn?svg=true)](https://ci.appveyor.com/project/embarrassedmilk/fulib)

Yet another functional library for C#. 

Inspired by [article series](https://fsharpforfunandprofit.com/posts/elevated-world/)
and this [beautiful library](https://github.com/louthy/language-ext).

It revolves around class `Result<T>` and its extensions for chaining methods together. 

`Result<T>` can be instantiated with either its value or exception/error message:
```csharp
var successfulResult = Result<int>.Success(1);
var failedResult = Result<int>.Failure("something went wrong");
var failedResultWithException = Result<int>.Failure(new InvalidOperationException("Index cannot be negative"));
```
Methods on `Result<T>`:
* `Match`
* `MatchTee`
* `MatchAsync`
* `MatchTeeAsync`

Supported extensions so far:
* `Map`
* `Apply`
* `Tee`
* `Bind`

`Result<T>` has extensions for async version of it - `Task<Result<T>>`

Both `Result<T>` and `Task<Result<T>>` support traverse - applicative and monadic versions of it. For async tasks, it's possible to use applicative version, but not parallelize execution.

This package also includes LINQ extensions, so you can write your code like this:

```csharp
var result = await
    from head in _headRepository.Get(headId)
    from body in _bodyRepository.Get(bodyId)
    from tail in _tailRepository.Get(tailId)
    select MakeCreature(head, body, tail);
```

As a bonus, there is implementation for `Reader<E,T>` and `Middleware<T>`. Examples can be found in this repository.
