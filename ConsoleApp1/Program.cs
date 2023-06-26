using Telegram.Bot;
using Telegram.Bot.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using ConsoleApp1.Models;
using System.Linq;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    class Program
    {
        public static dbConnect dbconnect;
        public static List<Client> clients;
        public static List<Products> products;
        public static List<History> histories;
        static int state;
        static int DelState;

        static void Main(string[] args)
        {
            dbconnect = new dbConnect();
            var client = new TelegramBotClient("5727159643:AAGVNHJ07N2FWbUPQaGDSHiGGtsG80ZJf3o");
             clients= new List<Client>();   
            products= new List<Products>();
            histories= new List<History>();
            histories = dbconnect.History.ToList();
            products = dbconnect.Products.ToList();
             clients = dbconnect.Client.ToList();  

            client.StartReceiving(Update2, Error2);
       
            Console.ReadLine();
            
        }

        async static  Task Error2(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.ToString());
            return;
                }

        async static  Task Update2(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message != null)
            {


                var massage = update.Message;
                string TextMassege = update.Message.Text ?? "";
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Записати", "Показати записи","Видалити запис" }, })
                {
                    ResizeKeyboard = true
                };
                ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[] { new KeyboardButton[] { "Картопля", "Цибуля", "Морква", "Головна" }, })
                {
                    ResizeKeyboard = true
                };

                ReplyKeyboardMarkup potato = new(new[] { new KeyboardButton[] { "Головна" }, })
                {
                    ResizeKeyboard = true
                };
                ReplyKeyboardMarkup onion = new(new[] { new KeyboardButton[] { "Головна" }, })
                {
                    ResizeKeyboard = true
                };
                ReplyKeyboardMarkup carrot = new(new[] { new KeyboardButton[] { "Головна" }, })
                {
                    ResizeKeyboard = true
                };
                ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[] { new KeyboardButton[] { "Записи по картоплі", "Записи по цибулі", "Записи по моркві", "Головна" }, })
                {
                    ResizeKeyboard = true
                };


                if (TextMassege != null)
                {
                    Console.WriteLine($"{massage.Chat.FirstName} | {massage.Text}");
                    if (massage.Text.Contains("/start"))
                    {
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "ХАЙ " + massage.Chat.FirstName, replyMarkup: replyKeyboardMarkup);
                        return;
                    }
                    if (massage.Text.Contains("Записати"))
                    {
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Записати", replyMarkup: replyKeyboardMarkup2);
                        return;
                    }
                    if (massage.Text.Contains("Картопля"))
                    {
                        state = 1;
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Форма запису\nІмя:\nКількість:\nЦіна за КГ:\nЗагальна сума:\n+", replyMarkup: potato);
                        return;
                    }
                    if (massage.Text.Contains("Цибуля"))
                    {
                        state = 2;
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Форма запису\nІмя:\nКількість:\nЦіна за КГ:\nЗагальна сума:\n+", replyMarkup: onion);
                        return;
                    }
                    if (massage.Text.Contains("Морква"))
                    {
                        state = 3;
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Форма запису\nІмя:\nКількість:\nЦіна за КГ:\nЗагальна сума:\n+", replyMarkup: carrot);
                        return;
                    }
                    if (massage.Text.Contains("Головна"))
                    {
                        state = 0;
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Головна", replyMarkup: replyKeyboardMarkup);
                        return;
                    }
                    if (massage.Text.Contains("+"))
                    {

                        if (state > 0)
                        {
                            dataConvert dataConvert = new dataConvert(massage, state);
                            dbconnect.History.Add(dataConvert.History);
                            dbconnect.SaveChangesAsync();
                            await botClient.SendTextMessageAsync(massage.Chat.Id, "Готово", replyMarkup: replyKeyboardMarkup);
                            return;
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(massage.Chat.Id, "Error", replyMarkup: replyKeyboardMarkup);
                            return;
                        }

                    }
                    if (massage.Text.Contains("Показати записи"))
                    {
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Виберіть продукт", replyMarkup: replyKeyboardMarkup3);
                        return;
                    }
                    if (massage.Text.Contains("Записи по картоплі"))
                    {
                        List<History> sortHistory = new List<History>();
                        List<Client> clients = new List<Client>();
                        clients = dbconnect.Client.ToList();
                        List<History> sort = new List<History>();
                        sort = dbconnect.History.ToList();
                        sortHistory = (from a in sort where a.ProductId == products[0].id select a).ToList();
                        if (sortHistory.Count == 0)
                        {
                            await botClient.SendTextMessageAsync(massage.Chat.Id, "Немає данів", replyMarkup: replyKeyboardMarkup);
                            return;
                        }
                        else
                        {
                            for (int i = 0; i < sortHistory.Count; i++)
                            {
                                List<Client> Sortclients = new List<Client>();
                                Sortclients = (from a in clients where a.id == sortHistory[i].ClientId select a).ToList();
                                await botClient.SendTextMessageAsync(massage.Chat.Id, i +
                                    "\nІмя: " + Sortclients[0].Name +
                                    "\nДата: " + sortHistory[i].DateParches +
                                    "\nЦіна за кг: " + sortHistory[i].ProductPrice +
                                    "\nВага: " + sortHistory[i].Totalweight +
                                    "\nЗагалбна сума: " + sortHistory[i].TotalAmoung +
                                    "\nID = " + sortHistory[i].id, replyMarkup: replyKeyboardMarkup);
                            }
                            return;
                        }
                    }
                    if (massage.Text.Contains("Записи по цибулі"))
                    {
                        List<History> sortHistory = new List<History>();
                        List<Client> clients = new List<Client>();
                        clients = dbconnect.Client.ToList();
                        List<History> sort = new List<History>();
                        sort = dbconnect.History.ToList();
                        sortHistory = (from a in sort where a.ProductId == products[1].id select a).ToList();
                        if (sortHistory.Count == 0)
                        {
                            await botClient.SendTextMessageAsync(massage.Chat.Id, "Немає данів", replyMarkup: replyKeyboardMarkup);
                            return;
                        }
                        else
                        {
                            for (int i = 0; i < sortHistory.Count; i++)
                            {
                                List<Client> Sortclients = new List<Client>();
                                Sortclients = (from a in clients where a.id == sortHistory[i].ClientId select a).ToList();
                                await botClient.SendTextMessageAsync(massage.Chat.Id, i +
                                    "\nІмя: " + Sortclients[0].Name +
                                    "\nДата: " + sortHistory[i].DateParches +
                                    "\nЦіна за кг: " + sortHistory[i].ProductPrice +
                                    "\nВага: " + sortHistory[i].Totalweight +
                                    "\nЗагалбна сума: " + sortHistory[i].TotalAmoung, replyMarkup: replyKeyboardMarkup);
                            }
                            return;
                        }
                    }
                    if (massage.Text.Contains("Записи по моркві"))
                    {
                        List<History> sortHistory = new List<History>();
                        List<Client> clients = new List<Client>();
                        clients = dbconnect.Client.ToList();
                        List<History> sort = new List<History>();
                        sort = dbconnect.History.ToList();
                        sortHistory = (from a in sort where a.ProductId == products[2].id select a).ToList();
                        if (sortHistory.Count == 0)
                        {
                            await botClient.SendTextMessageAsync(massage.Chat.Id, "Немає данів", replyMarkup: replyKeyboardMarkup);
                            return;
                        }
                        else { 
                        for (int i = 0; i < sortHistory.Count; i++)
                        {
                            List<Client> Sortclients = new List<Client>();
                            Sortclients = (from a in clients where a.id == sortHistory[i].ClientId select a).ToList();
                            await botClient.SendTextMessageAsync(massage.Chat.Id, i +
                                "\nІмя: " + Sortclients[0].Name +
                                "\nДата: " + sortHistory[i].DateParches +
                                "\nЦіна за кг: " + sortHistory[i].ProductPrice +
                                "\nВага: " + sortHistory[i].Totalweight +
                                "\nЗагалбна сума: " + sortHistory[i].TotalAmoung, replyMarkup: replyKeyboardMarkup);
                        }
                        return;
                        }
                    }
                    if (massage.Text.Contains("Видалити запис"))
                    { 
                        await botClient.SendTextMessageAsync(massage.Chat.Id, "Введіть Id накладної\n____________________ \n Приклад введення:\nID = 13",null);
                        return; 
                    }
                    if (massage.Text.ToLower().Contains("id"))
                    {
                       
                                List<History> PH = new List<History>();
                                PH = dbconnect.History.ToList();
                                string[] arr = massage.Text.Split('=');
                                PH = (from a in PH where a.id == int.Parse(arr[1]) select a).ToList();

                                dbconnect.History.Remove(PH[0]);
                                dbconnect.SaveChangesAsync();
                                await botClient.SendTextMessageAsync(massage.Chat.Id, "Накладна була видаленна", replyMarkup: replyKeyboardMarkup);
                                return;
                             
                                
                        
                    }
                }
                else return;
            }
        }
    }
}