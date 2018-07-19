﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Controllers;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class ContextHubTest : BaseTest
    {
      private ContextHub _target;
      private ITurnContext _turnContext;
      private IAdapter _adapter;

      public ContextHubTest()
      {
        _turnContext = A.Fake<ITurnContext>();
        _adapter = A.Fake<TelegramAdapter>();
        _testContainerBuilder.RegisterInstance(_adapter).As<IAdapter>();
        _testContainerBuilder.RegisterInstance(_turnContext).As<ITurnContext>();
        _testContainerBuilder.RegisterType<ContextHub>().SingleInstance();
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<ContextHub>();
      }
      [Fact]
      public async Task GetContext_Message()
      {
        // Arrange
        var update = new Update() {Message = new Message() {Text = "toto", Chat = new Chat() {Id = 15}}};
        // Act
        var context = await _target.GetContext(update);
        // Assert
        Check.That(context).Equals(_turnContext);
        Check.That(context.ChatId).Equals(15);
        Check.That(context.Activity).IsNotNull();
        Check.That(context.Activity.Text).Equals(update.Message.Text);
        Check.That(context.Activity.ActivityType).Equals(ActivityType.Message);
      }

      [Fact]
      public async Task GetContext_second_message()
      {
      // Arrange
        var update1 = new Update() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } };
        var update2 = new Update() { Message = new Message() { Text = "tata", Chat = new Chat() { Id = 15 } } };
        // Act
        var context1 = await _target.GetContext(update1);
        var context2 = await _target.GetContext(update2);
        // Assert
        Check.That(context1).Equals(context2);
        Check.That(context2.Activity.Text).Equals(update2.Message.Text);
      }
    [Fact]
      public async Task GetContext_CallbackQuery()
      {
        // Arrange
        var update = new Update() {CallbackQuery = new CallbackQuery(){Message = new Message(){Chat = new Chat(){Id = 15}}}};
        // Act
        var context = await _target.GetContext(update);
        // Assert
        Check.That(context).Equals(_turnContext);
        Check.That(context.ChatId).Equals(15);
      }
         [Fact]
        public async Task ResetContext()
        {
            // Arrange
            var update = new Update() { CallbackQuery = new CallbackQuery() { Message = new Message() { Chat = new Chat() { Id = 15 } } } };
            await _target.GetContext(update);
            update = new Update() {Message = new Message(){Text = "/reset", Chat = new Chat(){Id = 15}}};
            // Act
            await _target.ResetContext(update);
            // Assert
            var fieldInfo = typeof(ContextHub).GetField("_turnContexts", BindingFlags.NonPublic|BindingFlags.Instance);
            var value = fieldInfo.GetValue(_target) as Dictionary<long, ITurnContext>;

            Check.That(value.ContainsKey(update.Message.Chat.Id)).IsFalse();
        }
   }
}
