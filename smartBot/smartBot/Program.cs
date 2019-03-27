using System;
using ApiAiSDK;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace smartBot
{
    class Program
    {
        private static TelegramBotClient Bot;
        static ApiAi apiAi;
        private const string KEY = "800246490:AAEHF0OEn3wva-7zfOZBHYpRXwWUu_NOrj0";
        private const string DIALOG_KEY = "17e30d465e0f4c478e6b7d5407f89d7a";

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient(KEY);//remove before push
            AIConfiguration configuration = new AIConfiguration(DIALOG_KEY, SupportedLanguage.Russian);
            apiAi = new ApiAi(configuration);

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallBackQueryReceived;

            var me = Bot.GetMeAsync().Result;
            Console.Out.WriteLine("Name = {0}", me.FirstName);

            Bot.StartReceiving();
            Console.Read();
            Bot.StopReceiving();
        }

        private static void BotOnCallBackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)//пуши по пунктам меню
        {
            var buttonText = e.CallbackQuery.Data;
            var name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.Out.WriteLine($"{name} нажал кнопку {buttonText}");
            Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }
            var name = $"{message.From.FirstName} {message.From.LastName}";
            Console.Out.WriteLine($"{name}: {message.Text}");

            switch (message.Text)
            {
                case "/start":
                    string text =
@"Приветствую странник! Этот бот умеет многое..
Вот список доступных комманд:
/start - Запуск бота
/menu - Вывод меню
/keyboard - Вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Привет.."),
                            new KeyboardButton("Hello!")
                        },
                        new[]
                        {
                            new KeyboardButton("Контакт") {RequestContact = true},
                            new KeyboardButton("Геолокация") {RequestLocation = true}
                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбор невелик..", replyMarkup: replyKeyboard);
                    break;
                case "/menu":
                    var menuKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/des1nteresado"),
                            InlineKeyboardButton.WithUrl("Github", "https://github.com/des1nteresado"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2"),
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню", replyMarkup: menuKeyboard);
                    break;
                default:
                    var response = apiAi.TextRequest(message.Text);
                    string answer = response.Result.Fulfillment.Speech == "" ? "Прости, я тебя не понял.." : response.Result.Fulfillment.Speech; //ответ от dialogflow
                    await Bot.SendTextMessageAsync(message.Chat.Id, answer);
                    break;
            }
        }
    }
}
