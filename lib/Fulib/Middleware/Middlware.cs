using System;

namespace Fulib
{
    public delegate dynamic Middleware<T>(Func<T, dynamic> cont);
}