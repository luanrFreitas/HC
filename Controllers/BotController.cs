using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HustleCastle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            TelegramBotClient client = new TelegramBotClient("1814580579:AAHYWUhAxWxEmM2h5DFRwsrInxhXBEIUMrM");
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                await client.SendTextMessageAsync(update.Message.From.Id, "Resposta");
            }

            return Ok();
        }
    }
}
