using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

var botClient = new TelegramBotClient("1566931087:AAGPFKM_yWvap828FbVe-3pgY642T5gkpZM"); // Bot Token , create client

using var cts = new CancellationTokenSource();
// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};

botClient.StartReceiving(
   Bot.Handler.HandleUpdateAsync,
    Bot.Handler.HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync(); // Take bot info about

Console.WriteLine($"Start listening for @{me.Username}"); // Console logs
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

