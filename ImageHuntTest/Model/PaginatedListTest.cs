﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Xunit;

namespace ImageHuntTest.Model
{
    public class PaginatedListTest
    {
        class Entity
        {
            public int Value { get; set; }
        }
        [Fact]
        public async Task FactMethodName()
        {
            // Arrange
            //var source = new DbSet<Entity>();
            //var rand = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    source.Add(new Entity(){Value = rand.Next()});
            //}

            //var mockSource = A.Fake<IAsyncQueryProvider>();

            //var target = await PaginatedList<int>.CreateAsync(source.AsQueryable(), 2, 10);
            //// Act

            //// Assert
        }
    }
}
