using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using NFluent;
using Xunit;

namespace ImageHuntBotTest
{
  public class DummyModel
  {
    public int TheInt { get; set; }
    public double TheDouble { get; set; }
    public DateTime TheDateTime { get; set; }
    public string TheString { get; set; }
  }
    public class FileStorageTest : IDisposable
    {
      private FileStorage _target;
      private DirectoryInfo _dirInfo;

      public FileStorageTest()
      {
        var tempFolder = Path.GetTempPath();
        _dirInfo = Directory.CreateDirectory(Path.Combine(tempFolder, Path.GetRandomFileName()));
        _target = new FileStorage(_dirInfo.FullName);
      }

      [Fact]
      public async Task Write()
      {
        // Arrange
        var keyPairs = new[] {new KeyValuePair<string, object>("key1", DateTime.Now)};
        // Act
        await _target.Write(keyPairs);
        // Assert
        var files = Directory.EnumerateFiles(_dirInfo.FullName);
        Check.That(files).HasSize(1);
      }

      [Fact]
      public async Task WriteObject()
      {
        // Arrange
        var dummy = new DummyModel()
        {
          TheDateTime = DateTime.Today,
          TheDouble = 156.3,
          TheInt = 15,
          TheString = "Toto"
        };
        var keyPairs = new[] {new KeyValuePair<string, object>("key1", dummy) };
        // Act
        await _target.Write(keyPairs);
        // Assert
        var files = Directory.EnumerateFiles(_dirInfo.FullName);
        Check.That(files).HasSize(1);
      }

      [Fact]
      public async Task WriteDeleteObject()
      {
        // Arrange
        var dummy = new DummyModel()
        {
          TheDateTime = DateTime.Today,
          TheDouble = 156.3,
          TheInt = 15,
          TheString = "Toto"
        };
        var keyPairs = new[] {new KeyValuePair<string, object>("key1", dummy) };
        await _target.Write(keyPairs);
        // Act
        await _target.Delete(new[] {"key1"});
        // Assert
        var files = Directory.EnumerateFiles(_dirInfo.FullName);
        Check.That(files).HasSize(0);
      }
      [Fact]
      public async Task WriteReadObject()
      {
        // Arrange
        var dummy = new DummyModel()
        {
          TheDateTime = DateTime.Today,
          TheDouble = 156.3,
          TheInt = 15,
          TheString = "Toto"
        };
        var keyPairs = new[] {new KeyValuePair<string, object>("key1", dummy) };
        await _target.Write(keyPairs);
        // Act
        var result = await _target.Read<DummyModel>("key1");
        // Assert
        Check.That(result).HasSize(1);
      }

      public void Dispose()
      {
        Directory.Delete(_dirInfo.FullName, true);
      }
    }
}

