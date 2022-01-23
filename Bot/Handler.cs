using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System.Text.RegularExpressions;

namespace Bot
{
    public class Handler
    {

        public static string sex, cousre, distance, time; // Global variables for take user choise

        // Buttons,And massage for Reply Keyboard
        private const string Button1 = "M";
        private const string Button2 = "Ж";

        private const string ButtonChoise = "Выберете пол: ";
        private const string ButtonChoise2 = "Выберете длину бассейна: ";

        private const string Button3 = "25";
        private const string Button4 = "50";
        //  Method for Recive updates any type
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!, update),      // HandleMessage 
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!, update),     // HandleEditedMessage
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),   // HandleCallbackQuery 
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),   // HandleInlineQuery
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),    // HandleChosenInlineResult
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);   // Catch errors
            }

        }
        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message, Update update)
        {
            Console.WriteLine($"Receive message type: {message.Type}");   // Console logs for revive massage
            if (message.Type != MessageType.Text)
                return;
            // Switch Comand handler
            var action = message.Text!.Split(' ')[0] switch
            {
                "/inline" => SendInlineKeyboard(botClient, message),
                "/sex" => SendReplyKeyboard(botClient, message, Button1, Button2, ButtonChoise),
                "/course" => SendReplyKeyboard(botClient, message, Button3, Button4, ButtonChoise2),
                "/distance" => SendReplyKeyboardForDistanse(botClient, message),
                "/remove" => RemoveKeyboard(botClient, message),
                "/result" => WriteResult(botClient, message),
                "/time" => getTime(botClient, message),
                _ => Usage(botClient, message)
            };

            Message sentMessage = await action;
            // Console Logs
            Console.WriteLine($"The message was sent with id: {sentMessage.MessageId}" + $" from: {sentMessage.Chat.Id}" +$"\n With Text: {update.Message.Text}"+ $" \n user: {sentMessage.Chat.FirstName} {sentMessage.Chat.LastName}"
                + " \n Info gender: " + sex + "  Info course: " + cousre + "  Info distance: " + distance + " \n Info Time: " + time);

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("M", "M"),
                        InlineKeyboardButton.WithCallbackData("Ж", "Ж"),
                    }

                    });

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Выберите пол!",
                                                            replyMarkup: inlineKeyboard);
            }
            // Send reply keyboard , pass a method buttons and text. 
            // So we overrloadoing method
            static async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message, String Button1, String Button2, string ButtonChoise)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                        new KeyboardButton[] { Button1, Button2 }, //Buttons First Row

                    })
                {
                    ResizeKeyboard = true // Align keyboard
                };

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: ButtonChoise,
                                                            replyMarkup: replyKeyboardMarkup);
            }
            // Sasme  keyboard but for diatance
            static async Task<Message> SendReplyKeyboardForDistanse(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                            new KeyboardButton[] { "50 в/с", "100 в/с", "200 в/с", "400 в/с", "800 в/с", "1500 в/с" },
                            new KeyboardButton[] { "50 брасс", "100 брасс", "200 брасс" },
                            new KeyboardButton[] { "50 с/п", "100 с/п", "200 с/п" },
                            new KeyboardButton[] { "50 батт", "100 батт", "200 батт" },
                            new KeyboardButton[] { "100 к/п", "200 к/п", "400 к/п" }

                    })
                {
                    ResizeKeyboard = true
                };

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Выбертете дистанцию: ",
                                                            replyMarkup: replyKeyboardMarkup);
            }
            // Method for detele Keyboard
            static async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Удаляю клавиатуру...",
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            // Default message , send if no one standard command wasnt choise 
            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                ;
                const string usage = "Для получения результата необходимо заполнить данные." +
                    " \nПосле чего воспользуйтесь командой /result и бот отправит результат!\n\n" +
                                     "\n/inline - Открыть клавиатуру для заполнения\n" +
                                     "\n Или заполнить командами \n\n" + 
                                     "/sex - Выбрать пол\n" +
                                     "/course - Выбрать длину бассейна\n" +
                                     "/distance - Выбрать дистанцию\n" +
                                     "/time - Ввести свое время\n" +
                                     "/result - Получить результат\n" +

                                     "/remove - Удалить клавиатуру\n";

                // Here we assign values in global variables
                // Using regular expresion 
                if (message.Text!.Split(' ')[0] == "M" || message.Text!.Split(' ')[0] == "Ж") // If contains Word - Write gender
                    sex = message.Text;
                else if (Regex.IsMatch(message.Text, @"\d\s\w"))  // All distance have number after space and words
                    distance = message.Text;
                else if (Regex.IsMatch(message.Text!.Split(' ')[0], @"\d\d.\d\d"))  // All times have numbersDotnumbers
                    time = message.Text;
                else if (message.Text == "25" || message.Text == "50") // And course must be 25 or 50 strictly
                    cousre = message.Text;

                else
                    Console.WriteLine("Error 177 ISNotGlobalVar "+ message.Text);  // Else Write log eror on console

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: usage,
                                                                replyMarkup: new ReplyKeyboardRemove());
            }
            // Take time for user , accept dot and ',' formats
            static async Task<Message> getTime(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Введите ваше время\n" +
                                     "В формате минуты.секунды.милисекунды\n" +
                                     "Пример 2.25.04 или 23,99\n";



                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            // Task write results on Fina poins
            static async Task<Message> WriteResult(ITelegramBotClient botClient, Message message)
            {
                string usage;
                //Check in all variables are full
                if (sex == null || distance == null
                    || time == null || cousre == null)
                    usage = "Вы не заполнили данные!";
                else // If all right call methond who callculate poins from class CallculatePoints
                    usage = CallculatePoints.Callculatorpoints(sex, cousre, distance, time).ToString();

                Console.WriteLine(usage + "  " + sex + distance + cousre); // Console logs

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }

        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            distance = callbackQuery.Data; // Add info to distance
            // Inline Keyboard handler
            var inlineaction = callbackQuery.Data switch
            {
                "M" => SendInlineKeyboard(botClient, callbackQuery),
                "Ж" => SendInlineKeyboard(botClient, callbackQuery),
                "25" => SendInlineKeyboard1(botClient, callbackQuery),
                "50" => SendInlineKeyboard1(botClient, callbackQuery),
                _ => getTime(botClient, callbackQuery)
            };
           
            Message sentMessage = await inlineaction;
            // Sebd inline keyboard With Cource and callbackquery
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                // Wait User move
                await botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);
            
                // Add gender , bcz in buffer is gender choise now
                sex = callbackQuery.Data;

                // Simulate longer running task
                await Task.Delay(500);
                // Send Cource buttons
                InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("50", "50"),
                        InlineKeyboardButton.WithCallbackData("25", "25"),
                    }

                    });

                return await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                            text: "Выберите длину бассейна!",
                                                            replyMarkup: inlineKeyboard);
            }
            // Sebd inline keyboard With Distance 
            static async Task<Message> SendInlineKeyboard1(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                await botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing); 
                // Add cource to var 
                cousre = callbackQuery.Data;

                // Simulate longer running task
                await Task.Delay(500);

                InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("50 в/с","50 в/с"),
                             InlineKeyboardButton.WithCallbackData("100 в/с","100 в/с"),
                                 InlineKeyboardButton.WithCallbackData("200 в/с","200 в/с"),

                    },
                     // second row
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData("400 в/с","400 в/с"),
                             InlineKeyboardButton.WithCallbackData("800 в/с","800 в/с"),
                                  InlineKeyboardButton.WithCallbackData("1500 в/с","1500 в/с"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("50 брасс","50 брасс"),
                             InlineKeyboardButton.WithCallbackData("100 брасс","100 брасс"),
                                 InlineKeyboardButton.WithCallbackData("200 брасс","200 брасс"),
                    },
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData("50 с/п","50 с/п"),
                             InlineKeyboardButton.WithCallbackData("100 с/п","100 с/п"),
                                 InlineKeyboardButton.WithCallbackData("200 с/п","200 с/п"),
                    },
                    new []
                    {
                      InlineKeyboardButton.WithCallbackData("50 батт","50 батт"),
                             InlineKeyboardButton.WithCallbackData("100 батт","100 батт"),
                                 InlineKeyboardButton.WithCallbackData("200 батт","200 батт"),
                    },
                    new []
                    {
                      InlineKeyboardButton.WithCallbackData("100 к/п","100 к/п"),
                             InlineKeyboardButton.WithCallbackData("200 к/п","200 к/п"),
                                 InlineKeyboardButton.WithCallbackData("400 к/п","400 к/п"),
                    }
                    });
                return await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                            text: "Выберите дистанцию!",
                                                            replyMarkup: inlineKeyboard);
            }
            // Same method as Reply Keyboard
            static async Task<Message> WriteResult(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
                string usage;

                if (sex == null || distance == null
                    || time == null || cousre == null)
                    usage = "Вы не заполнили данные!";
                else
                    usage = CallculatePoints.Callculatorpoints(sex, cousre, distance, time).ToString();

                Console.WriteLine(usage + "  " + sex + distance + cousre);

                return await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                            text: usage
                                                          );
            }
            static async Task<Message> getTime(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            {
   
                const string usage = "Введите ваше время\n" +
                                     "В формате минуты.секунды.милисекунды\n" +
                                     "Пример 2.25.04 или 23,99\n";

                return await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
               botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId); // after cholise delete message
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
