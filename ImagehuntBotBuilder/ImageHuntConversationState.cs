using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder
{
    public class ImageHuntConversationState : ConversationState
    {
        public ImageHuntConversationState(IStorage storage)
            : base(storage)
        {
        }
        /// <summary>
        /// Create a property definition and register it with this BotState.
        /// </summary>
        /// <typeparam name="T">type of property.</typeparam>
        /// <param name="name">name of the property.</param>
        /// <returns>The created state property accessor.</returns>
        public IStatePropertyAccessorExtended<T> CreateProperty<T>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new BotStatePropertyAccessorExtended<T>(this, name);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }
        public async Task<T> GetPropertyValueAsync<T>(ITurnContext turnContext, string propertyName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.GetPropertyValueAsync<T>(turnContext, propertyName, cancellationToken);
        }

        public async Task DeletePropertyValueAsync(ITurnContext turnContext, string propertyName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.DeletePropertyValueAsync(turnContext, propertyName, cancellationToken);
        }
        public async Task SetPropertyValueAsync(ITurnContext turnContext, string propertyName, object value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SetPropertyValueAsync(turnContext, propertyName, cancellationToken);
        }

    }

    public class BotStatePropertyAccessorExtended<T> : IStatePropertyAccessorExtended<T>
    {
        private readonly ImageHuntConversationState _imageHuntConversationState;

        public BotStatePropertyAccessorExtended(ImageHuntConversationState imageHuntConversationState, string name)
        {
            _imageHuntConversationState = imageHuntConversationState;
            Name = name;
        }

        public async Task<IEnumerable<T>> GetAllAsync(ITurnContext turnContext)
        {
            return await _imageHuntConversationState.GetAllAsync<T>(turnContext);
        }

        public string Name { get; }

        public async Task<T> GetAsync(ITurnContext turnContext, Func<T> defaultValueFactory = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await _imageHuntConversationState.LoadAsync(turnContext, false, cancellationToken).ConfigureAwait(false);
            try
            {
                return await _imageHuntConversationState.GetPropertyValueAsync<T>(turnContext, Name, cancellationToken).ConfigureAwait(false);
            }
            catch (KeyNotFoundException)
            {
                // ask for default value from factory
                if (defaultValueFactory == null)
                {
                    throw new MissingMemberException("Property not set and no default provided.");
                }

                var result = defaultValueFactory();

                // save default value for any further calls
                await SetAsync(turnContext, result, cancellationToken).ConfigureAwait(false);
                return result;
            }
        }

        public async Task DeleteAsync(ITurnContext turnContext, CancellationToken cancellationToken = new CancellationToken())
        {
            await _imageHuntConversationState.LoadAsync(turnContext, false, cancellationToken).ConfigureAwait(false);
            await _imageHuntConversationState.DeletePropertyValueAsync(turnContext, Name, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetAsync(ITurnContext turnContext, T value, CancellationToken cancellationToken = new CancellationToken())
        {
            await _imageHuntConversationState.LoadAsync(turnContext, false, cancellationToken).ConfigureAwait(false);
            await _imageHuntConversationState.SetPropertyValueAsync(turnContext, Name, value, cancellationToken).ConfigureAwait(false);
        }

    }
}
