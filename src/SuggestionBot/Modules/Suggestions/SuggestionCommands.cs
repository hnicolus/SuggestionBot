using Discord.Commands;

namespace SuggestionBot.Modules.Suggestions
{
    public class SuggestionCommands : ModuleBase<SocketCommandContext>
    {
        private readonly SuggestionService _suggestionService;

        public SuggestionCommands(SuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        [Command("suggest something")]
        public async Task SuggestAsync()
        {
            await Context.Channel.TriggerTypingAsync();
            await Context.Channel.SendMessageAsync("...hmmmm let me think of an activity you can do...");

            await Context.Channel.TriggerTypingAsync();
            var suggestion = await _suggestionService.GetSuggestionAsync();

            await ReplyAsync($"You can {suggestion.Activity} ,this is a {suggestion.Type} activity and it is {suggestion.Accessibility * 100}% accessible.");
        }
    }
}