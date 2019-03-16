using System;
using Microsoft.EntityFrameworkCore;

namespace ImageHuntCore
{
    public class ActivableContext<T> : DbContext where T : DbContext
    {
        public static T CreateInstance(DbContextOptions dbContextOptions)
        {
            return Activator.CreateInstance(typeof(T), dbContextOptions) as T;
        }
    }
}