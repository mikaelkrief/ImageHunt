using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.AttributeFilters;
using ImageHuntBotBuilder;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class FileStorageTest : BaseTest<FileStorage>, IDisposable
    {
        private DirectoryInfo _dirInfo;

        public FileStorageTest()
        {
            var tempFolder = Path.GetTempPath();
            _dirInfo = Directory.CreateDirectory(Path.Combine(tempFolder, Path.GetRandomFileName()));

            _testContainerBuilder.RegisterType<FileStorage>()
                .WithParameter(new NamedParameter("folder", _dirInfo.FullName));
            Build();
        }
        public void Dispose()
        {
            Directory.Delete(_dirInfo.FullName, true);
        }

        [Fact]
        public async Task Should_Write_State()
        {
            // Arrange
            var values = new Dictionary<string, object>()
            {
                {"value1", "val" }    ,
                {"value2", DateTime.Now },
            };
            // Act
            await _target.WriteAsync(values);
            // Assert
            var files = Directory.EnumerateFiles(_dirInfo.FullName);
            Check.That(files).HasSize(values.Count);
        }
        [Fact]
        public async Task Should_Read_State()
        {
            // Arrange
            var values = new Dictionary<string, object>()
            {
                {"value1", "val" }    ,
                {"value2", DateTime.Now },
            };
            await _target.WriteAsync(values);

            // Act
            var result = await _target.ReadAsync(values.Keys.ToArray());
            // Assert
            var files = Directory.EnumerateFiles(_dirInfo.FullName);
            Check.That(files).HasSize(values.Count);
            Check.That(result).HasSize(values.Count);

            Check.That(result).Equals(values);

        }
        [Fact]
        public async Task Should_Delete_State()
        {
            // Arrange
            var values = new Dictionary<string, object>()
            {
                {"value1", "val" }    ,
                {"value2", DateTime.Now },
                {"value3", 1235 },
            };
            await _target.WriteAsync(values);
            // Act
            await _target.DeleteAsync(values.Keys.Take(1).ToArray());
            // Assert
            var files = Directory.EnumerateFiles(_dirInfo.FullName);
            Check.That(files).HasSize(2);
        }
        [Fact]
        public async Task Should_Save_Change_State()
        {
            // Arrange
            var values = new Dictionary<string, object>()
            {
                {"value1", "val" }    ,
                {"value2", DateTime.Now },
            };
            await _target.WriteAsync(values);
            values["value1"] = "val2";
            // Act
            await _target.WriteAsync(values);
            // Assert
            var result = await _target.ReadAsync(values.Keys.ToArray());
            var files = Directory.EnumerateFiles(_dirInfo.FullName);
            Check.That(files).HasSize(values.Count);
            Check.That(result).HasSize(values.Count);

            Check.That(result).Equals(values);

        }

    }
}
