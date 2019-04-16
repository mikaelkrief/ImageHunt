using System.Collections.Generic;
using System.Text.RegularExpressions;
using ImageHunt.Data;
using ImageHuntCore.Model;

namespace ImageHunt.Updater
{
  public abstract class AbstractUpdater : IUpdater
  {
    protected readonly HuntContext Context;
    protected readonly Game Game;
    private readonly string _rawArguments;
    protected Dictionary<string, string> Arguments = new Dictionary<string, string>();

    public AbstractUpdater(HuntContext context, Game game, string arguments)
    {
      Context = context;
      Game = game;
      _rawArguments = arguments;
      SplitArguments();
    }

    protected void SplitArguments()
    {
      var splited = _rawArguments.Split(' ');
      var regex = new Regex(@"--(\w*)=(.*)");
      foreach (var s in splited)
      {
        if (regex.IsMatch(s))
        {
          var groupCollection = regex.Matches(s)[0].Groups;
          var key = groupCollection[1].Value;
          var value = groupCollection[2].Value;
          Arguments.Add(key, value);

        }
      }
    }
    public abstract void Execute();
  }
}