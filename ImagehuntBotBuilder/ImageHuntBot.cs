﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHuntBotBuilder
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class ImageHuntBot : IBot
    {
        private readonly ImageHuntBotAccessors _accessors;
        private readonly IActionWebService _actionWebService;
        private readonly ITeamWebService _teamWebService;
        private readonly ICommandRepository _commandRepository;
        private readonly INodeVisitorHandler _nodeVisitorHandler;
        private readonly ILogger _logger;
        private IStringLocalizer _localizer;
        private DialogSet _dialogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoWithCounterBot"/> class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="logger">Logger provided by injection</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public ImageHuntBot(
            ImageHuntBotAccessors accessors,
            IActionWebService actionWebService,
            ITeamWebService teamWebService,
            ICommandRepository commandRepository,
            INodeVisitorHandler nodeVisitorHandler,
            ILogger<ImageHuntBot> logger,
            IStringLocalizer<ImageHuntBot> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _logger.LogTrace("ImageHuntBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));
            _actionWebService = actionWebService;
            _teamWebService = teamWebService;
            _commandRepository = commandRepository;
            _nodeVisitorHandler = nodeVisitorHandler;
            _dialogs = new DialogSet(accessors.ConversationDialogState);
            _nodeVisitorHandler.ConstructDialogSet(_dialogs);
        }

        public async Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Get the conversation state from the turn context.
                var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());
                state.ConversationId = turnContext.Activity.Conversation.Id;
                if (state.Team != null)
                {
                    _localizer = _localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
                }

                switch (turnContext.Activity.Type)
                {
                    case ImageHuntActivityTypes.Image:
                        if (state.Status == Status.Started &&
                            state.GameId.HasValue &&
                            state.TeamId.HasValue)
                        {
                            var gameActionRequest = new GameActionRequest()
                            {
                                Action = (int) Action.SubmitPicture,
                                GameId = state.GameId.Value,
                                TeamId = state.TeamId.Value,
                                Latitude = state.CurrentLocation.Latitude.Value,
                                Longitude = state.CurrentLocation.Longitude.Value,
                                PictureId = (int) turnContext.Activity.Attachments.First().Content,
                            };
                            await _actionWebService.LogAction(gameActionRequest);
                            _logger.LogInformation(
                                "Image {0} had been uploaded", turnContext.Activity.Attachments.First().Name);
                            await turnContext.SendActivityAsync(
                                _localizer["IMAGE_RECEIVED_CONFIRM"], cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await turnContext.SendActivityAsync(
                                _localizer["CANNOT_RECORD_PHOTO"], cancellationToken: cancellationToken);
                        }

                        break;
                    case ActivityTypes.Message:
                        if (_dialogs != null)
                        {
                            var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                            var result = await dialogContext.ContinueDialogAsync(cancellationToken);
                            if (result.Status == DialogTurnStatus.Complete)
                            {
                                break;
                            }
                        }
                        //if (state.CurrentDialog != null)
                        //{
                        //    var dialogContext =
                        //        await state.CurrentDialog.CreateContextAsync(turnContext, cancellationToken);
                        //    var result = await dialogContext.ContinueDialogAsync(cancellationToken);
                        //    if (result.Status == DialogTurnStatus.Complete)
                        //    {
                        //        await turnContext.SendActivityAsync($"Reponse {result.Result}");
                        //    }
                        //    break;
                        //}
                        if (!string.IsNullOrEmpty(turnContext.Activity.Text) &&
                            turnContext.Activity.Text.StartsWith('/'))
                        {
                            await _commandRepository.RefreshAdminsAsync();

                            try
                            {
                                var command = _commandRepository.Get(turnContext, state, turnContext.Activity.Text);
                                await command.ExecuteAsync(turnContext, state);
                            }
                            catch (NotAuthorizedException e)
                            {
                                _logger.LogError(
                                    e,
                                    $"User {turnContext.Activity.From.Name} not authorized to use this command");
                                await turnContext.SendActivityAsync(_localizer["COMMAND_NOT_AUTHORIZED"]);
                            }
                            catch (CommandNotFound e)
                            {
                                _logger.LogError($"Command {e.Command} not found");
                                await turnContext.SendActivityAsync(
                                    _localizer["COMMAND_NOT_FOUND"]);
                            }
                        }

                        break;
                    case ImageHuntActivityTypes.Location:
                        await _nodeVisitorHandler.MatchHiddenNodesLocationAsync(turnContext, state);
                        await _nodeVisitorHandler.MatchLocationAsync(turnContext, state);
                        await _nodeVisitorHandler.MatchLocationDialogAsync(turnContext, state, _dialogs);
                        break;
                }

                // Set the property using the accessor.
                await _accessors.ImageHuntState.SetAsync(turnContext, state);
                // Save the new turn count into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occured while process user message");
            }
        }
    }
}