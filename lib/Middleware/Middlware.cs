using System;

public delegate dynamic Middleware<T>(Func<T, dynamic> cont);