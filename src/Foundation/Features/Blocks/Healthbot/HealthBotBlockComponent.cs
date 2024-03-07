﻿namespace Foundation.Features.Blocks.Healthbot
{
    public class HealthChatBotBlockController : AsyncBlockComponent<HealthChatbotBlock>
    {
        private readonly IRequiredClientResourceList _requiredClientResourceList;

        public HealthChatBotBlockController(IRequiredClientResourceList requiredClientResourceList)
        {
            _requiredClientResourceList = requiredClientResourceList;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(HealthChatbotBlock currentBlock)
        {
            _requiredClientResourceList.Require(HealthBotClientResourceProvider.BotJs).AtHeader();
            var model = new BlockViewModel<HealthChatbotBlock>(currentBlock);
            return await Task.FromResult(View("/Features/Blocks/HealthBot/HealthChatBotBlock.cshtml", model));
        }
    }

    [ClientResourceProvider]
    public class HealthBotClientResourceProvider : IClientResourceProvider
    {
        public static string BotJs = "healthbot.webchat";

        public IEnumerable<ClientResource> GetClientResources()
        {
            return new[]
            {
                new ClientResource
                {
                    Name = BotJs,
                    ResourceType = ClientResourceType.Html,
                    InlineContent = @"<script crossorigin=""anonymous"" src=""https://cdn.botframework.com/botframework-webchat/latest/webchat.js""></script>"
                }
            };
        }
    }
}
