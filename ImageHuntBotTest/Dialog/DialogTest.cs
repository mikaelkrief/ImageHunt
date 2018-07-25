using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntTelegramBot.Dialogs.Prompts;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
  public class DialogTest : BaseTest
    {
      private IDialog _target;

      public DialogTest()
      {

        _container = _testContainerBuilder.Build();
        _target = A.Fake<AbstractDialog>();
      }

    [Fact]
      public async Task AddChildren_And_Begin()
      {
      // Arrange
        _target = A.Fake<AbstractDialog>(options=>options.CallsBaseMethods());

      var childrenDialog = A.Fake<IDialog>();
        _target.AddChildren(childrenDialog);
        var context = A.Fake<ITurnContext>();
        // Act
        await _target.Begin(context);
        // Assert
        A.CallTo(() => context.Begin(childrenDialog)).MustHaveHappened();
      }
    [Fact]
      public async Task AddChildren_And_Begin_And_Continue()
      {
      // Arrange
        _target = A.Fake<AbstractDialog>(options=>options.CallsBaseMethods());

      var childrenDialog = A.Fake<IDialog>();
        _target.AddChildren(childrenDialog);
        var context = A.Fake<ITurnContext>();
        var activity1 = A.Fake<IActivity>();
        // Act
        A.CallTo(() => context.Activity).Returns(activity1);
        await _target.Begin(context);
        await _target.Continue(context);
        // Assert
        A.CallTo(() => context.Begin(childrenDialog)).MustHaveHappened();
        A.CallTo(() => childrenDialog.Continue(context)).MustHaveHappened();
      }


    [Fact]
      public async Task AddChildren_And_Begin_And_Continue_on_multiple_subDialog()
      {
      // Arrange
        _target = A.Fake<AbstractDialog>(options=>options.CallsBaseMethods());

      var childrenDialog1 = A.Fake<NumberPrompt<int>>(op=>op.CallsBaseMethods()
                                                            .WithArgumentsForConstructor(new object[] {"",null, null}));
        _target.AddChildren(childrenDialog1);
      //A.CallTo(() => childrenDialog1.Begin(A<ITurnContext>._))
      //  .Invokes(async (ITurnContext turnContext) => { await turnContext.Continue(); });
      var childrenDialog2 = A.Fake<NumberPrompt<int>>(op => op.CallsBaseMethods()
                                                              .WithArgumentsForConstructor(new[] { "", null, null }));
      _target.AddChildren(childrenDialog2);
        var context = A.Fake<TurnContext>(op=>op.CallsBaseMethods());
        var activity1 = A.Fake<IActivity>();
        // Act
        A.CallTo(() => context.Activity).Returns(activity1);
        await context.Begin(_target);
        await context.Continue();
        await context.Continue();
        // Assert
        A.CallTo(() => context.Begin(childrenDialog1)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => childrenDialog1.Continue(context)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => childrenDialog2.Begin(context)).MustHaveHappened(Repeated.Exactly.Once);
        //A.CallTo(() => childrenDialog2.Continue(context)).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => context.End()).MustHaveHappened();
      }

      public class DummyDialog : AbstractDialog
      {
        public override async Task Reply(ITurnContext turnContext)
        {
          var activity = A.Fake<IActivity>();
          await turnContext.ReplyActivity(activity);
        }

          public override string Command { get; }

          public override async Task Continue(ITurnContext turnContext)
        {
          var activity = A.Fake<IActivity>();
          await turnContext.ReplyActivity(activity);
        }

        public DummyDialog(ILogger logger) : base(logger)
        {
        }
      }

      [Fact]
      public void NoChildren_Begin_Should_Reply()
      {
        // Arrange
        _target = A.Fake<DummyDialog>(op=>op.CallsBaseMethods());
        var context = A.Fake<ITurnContext>();
        // Act
        _target.Begin(context);
        // Assert
        A.CallTo(() => _target.Begin(A<ITurnContext>._)).MustHaveHappened();
      }

      [Fact]
      public async Task TurnContextEnd_Should_Give_Control_To_Next_Dialog()
      {
        // Arrange
        _target = A.Fake<AbstractDialog>(op=>op.CallsBaseMethods());
        var childrenDialog1 = A.Fake<NumberPrompt<int>>(op=>op.CallsBaseMethods()
                                                              .WithArgumentsForConstructor(new []{ "First Prompt", null, null }) );
        _target.AddChildren(childrenDialog1);
        var childrenDialog2 = A.Fake<NumberPrompt<int>>(op => op.CallsBaseMethods()
          .WithArgumentsForConstructor(new[] { "Second Prompt", null, null }));
        _target.AddChildren(childrenDialog2);
        var context = A.Fake<TurnContext>(op=>op.CallsBaseMethods());

        // Act
        var activity1 = new Activity(){ChatId = 15, ActivityType = ActivityType.Message, Text = "toto"};
        A.CallTo(() => context.Activity).Returns(activity1);
        await context.Begin(_target);
        var activity2 = new Activity() { ChatId = 15, ActivityType = ActivityType.Message, Text = "toto" };
        A.CallTo(() => context.Activity).Returns(activity2);
        await context.Continue();
        // Assert
        A.CallTo(() => context.ReplyActivity(A<Activity>.That.Matches(a=>a.Text == "First Prompt"))).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => context.ReplyActivity(A<Activity>.That.Matches(a=>a.Text == "Second Prompt"))).MustHaveHappened(Repeated.Exactly.Once);
      }
      [Fact]
      public async Task Only1SubDialog()
      {
        // Arrange
        _target = A.Fake<AbstractDialog>(op=>op.CallsBaseMethods());
        var childrenDialog1 = A.Fake<NumberPrompt<int>>(op=>op.CallsBaseMethods()
                                                              .WithArgumentsForConstructor(new []{ "First Prompt", null, null }) );
        _target.AddChildren(childrenDialog1);
        var context = A.Fake<TurnContext>(op=>op.CallsBaseMethods());
        //A.CallTo(() => context.End()).Invokes(() => context.EndCalled+= Raise.With(context, new EventArgs()) );

        // Act
        var activity1 = new Activity(){ChatId = 15, ActivityType = ActivityType.Message, Text = "toto"};
        A.CallTo(() => context.Activity).Returns(activity1);
        await context.Begin(_target);
        var activity2 = new Activity() { ChatId = 15, ActivityType = ActivityType.Message, Text = "toto" };
        A.CallTo(() => context.Activity).Returns(activity2);
        await context.Continue();
        // Assert
        A.CallTo(() => context.ReplyActivity(A<Activity>.That.Matches(a=>a.Text == "First Prompt"))).MustHaveHappened(Repeated.Exactly.Once);
        A.CallTo(() => context.End()).MustHaveHappened(Repeated.Exactly.Twice);
      }
  }

  public class DummyDialog2 : AbstractDialog
  {
    public DummyDialog2(ILogger logger) : base(logger)
    {
    }

      public override string Command { get; }
  }
}
