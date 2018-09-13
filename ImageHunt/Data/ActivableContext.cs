using System;
using Microsoft.EntityFrameworkCore;

namespace ImageHunt.Data
{
  public class ActivableContext<T> : DbContext where T: DbContext
  {
    public ActivableContext()
    {
      
    }

    public ActivableContext(DbContextOptions options) : base(options)
    {
      
    }
    public static T CreateInstance(DbContextOptions dbContextOptions)
    {
      return Activator.CreateInstance(typeof(T), dbContextOptions) as T;
    }
  }
}