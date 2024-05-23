using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using tgbot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Update = Telegram.Bot.Types.Update;

internal class Program
{

    static CancellationToken token = new CancellationToken();
    static Host kissabot;
    private static bool flagAskPhone = false;
    private static bool flagAskYesNo = false;
    private static bool flagAskCom = false;
    private static bool flagAskTime = false;
    private static bool flagOrderClose = false;



    private static void Main()
    {
        kissabot = new Host("6888221290:AAH4uGzCC-9enq2KpdS0Iaigdiot4UHDv-Y");
        kissabot.Start();
        kissabot.OnMessage += OnMessage;
        Console.ReadLine();
    }

    static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "новый заказ" }, new KeyboardButton[] { "меню" }, new KeyboardButton[] { "статус заказа" }, new KeyboardButton[] { "завершить заказ" } })
    {
        ResizeKeyboard = true
    };

    private static async void OnMessage(ITelegramBotClient client, Update update)

    {

        int order = 0;
        string connection = "server=localhost;port=3306;user=root;password=Masha0325;database=coffeeshop;Character Set=utf8mb4;";
        MySqlConnection connect = new MySqlConnection(connection);
        OpenConnection();

        bool OpenConnection()
        {
            try
            {
                connect.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        //инлайн клавиатуры кнопочек
        InlineKeyboardMarkup newOrderM = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "база", callbackData: "base_menuM"),
                InlineKeyboardButton.WithCallbackData(text: "спец/гик/артерское", callbackData: "special_menuM"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "добавки", callbackData: "adds"),

            },
        });
        InlineKeyboardMarkup newOrderS = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "база", callbackData: "base_menuS"),
                InlineKeyboardButton.WithCallbackData(text: "спец/гик/артерское", callbackData: "special_menuS"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "добавки", callbackData: "adds"),

            },
        });

        InlineKeyboardMarkup adds = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "альтернативное молоко", callbackData: "alternative_milk"),
                InlineKeyboardButton.WithCallbackData(text: "сироп", callbackData: "syrup"),
                InlineKeyboardButton.WithCallbackData(text: "корица", callbackData: "cinnamon"),
            },

            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "сахар", callbackData: "sugar"),
                InlineKeyboardButton.WithCallbackData(text: "маршмеллоу", callbackData: "marshmallow"),

            },
        });

        InlineKeyboardMarkup base_menuM = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "кофя", callbackData: "coffee"),
                InlineKeyboardButton.WithCallbackData(text: "иное", callbackData: "another_coffeeM"),
            },
        });

        InlineKeyboardMarkup special_menuM = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "кофя", callbackData: "special_coffeeM"),
                InlineKeyboardButton.WithCallbackData(text: "не кофя", callbackData: "another_special_coffeeM"),
            },
        });
        InlineKeyboardMarkup base_menuS = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "иное", callbackData: "another_coffeeS"),
            },
        });

        InlineKeyboardMarkup special_menuS = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "кофя", callbackData: "special_coffeeS"),
                InlineKeyboardButton.WithCallbackData(text: "не кофя", callbackData: "another_special_coffeeS"),
            },
        });
        InlineKeyboardMarkup coffee = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Американо", callbackData: "americano"),
                InlineKeyboardButton.WithCallbackData(text: "Капучино", callbackData: "cappuchino"),
                InlineKeyboardButton.WithCallbackData(text: "Латте", callbackData: "latte"),
                InlineKeyboardButton.WithCallbackData(text: "Флэт Уайт", callbackData: "flatwhite"),
            },
        });
        InlineKeyboardMarkup another_coffeeM = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Матча Латте", callbackData: "matchalatte"),
            },
        });
        InlineKeyboardMarkup special_coffeeM = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Хината Шоё", callbackData: "hinatashoe"),
                InlineKeyboardButton.WithCallbackData(text: "Kissa", callbackData: "kissa"),
            },
        });
        InlineKeyboardMarkup another_special_coffeeM = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Хакку", callbackData: "hakku"),
                InlineKeyboardButton.WithCallbackData(text: "Сяо", callbackData: "xiao"),

            },

        });
        ;
        InlineKeyboardMarkup another_coffeeS = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Чай", callbackData: "tea"),
                InlineKeyboardButton.WithCallbackData(text: "Какао", callbackData: "cacao"),
                InlineKeyboardButton.WithCallbackData(text: "Кисель", callbackData: "kissel"),
            },
        });
        InlineKeyboardMarkup special_coffeeS = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Для гуля", callbackData: "dlyagyliya"),
                InlineKeyboardButton.WithCallbackData(text: "Очень странные дела", callbackData: "ochenstranniedela"),
                InlineKeyboardButton.WithCallbackData(text: "Беннет", callbackData: "bennet"),

            },
        });
        InlineKeyboardMarkup another_special_coffeeS = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Барби стайл", callbackData: "barbiestyle"),
                InlineKeyboardButton.WithCallbackData(text: "Рэй", callbackData: "rei"),
                InlineKeyboardButton.WithCallbackData(text: "Mood L", callbackData: "moodl"),
            },
             new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Мята и шоколад", callbackData: "myataishocolad"),
                InlineKeyboardButton.WithCallbackData(text: "Ведьмачий сбор", callbackData: "vedmachiysbor"),
            },
        });
        InlineKeyboardMarkup size = new(new[]
{
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "M", callbackData: "max"),
                InlineKeyboardButton.WithCallbackData(text: "S", callbackData: "small"),
            },
        });

        //кнопочки меню
        switch (update.CallbackQuery?.Data)
        {
            case "base_menuM":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/base_menu.jpg"), caption: "супер! что дальше?", replyMarkup: base_menuM, cancellationToken: token);
                break;
            case "special_menuM":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/special.png"), caption: "супер! что дальше?", replyMarkup: special_menuM, cancellationToken: token);
                break;
            case "base_menuS":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/base_menu.jpg"), caption: "супер! что дальше?", replyMarkup: base_menuS, cancellationToken: token);
                break;
            case "special_menuS":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/special.png"), caption: "супер! что дальше?", replyMarkup: special_menuS, cancellationToken: token);
                break;
        }

        //кнопочки заказиков напитков
        switch (update.CallbackQuery?.Data)
        {
            case "coffee":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: coffee, cancellationToken: token);
                break;
            case "another_coffeeM":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: another_coffeeM, cancellationToken: token);
                break;
            case "special_coffeeM":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: special_coffeeM, cancellationToken: token);
                break;
            case "another_special_coffeeM":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: another_special_coffeeM, cancellationToken: token);
                break;
            case "another_coffeeS":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: another_coffeeS, cancellationToken: token);
                break;
            case "special_coffeeS":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: special_coffeeS, cancellationToken: token);
                break;
            case "another_special_coffeeS":
                await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "скорее выбирай желаемое!", replyMarkup: another_special_coffeeS, cancellationToken: token);
                break;
        }

        //добавки
        switch (update.CallbackQuery?.Data)
        {
            case "adds":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/adds.png"), caption: "супер! что дальше?", replyMarkup: adds, cancellationToken: token);
                break;

            case "alternative_milk":
                int idmilk = 0;
                int truemilk = 0;
                string getautomilk = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautomilk, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            idmilk = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemilk = idmilk - 1;
                        readeridorderdrink.Close();
                    }
                }
                string descMilk = "SELECT description FROM adds WHERE id = 1";
                using (MySqlCommand searchMilk = new MySqlCommand(descMilk, connect))
                {
                    using (MySqlDataReader ReadMilk = searchMilk.ExecuteReader())
                    {
                        if (ReadMilk.Read())
                        {

                            string desc = ReadMilk.GetString("description");
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, $"в наличии: {desc}", cancellationToken: token);
                            ReadMilk.Close();
                        }

                    }
                }
                string sqlmilk = "SELECT id_adds, id_drinksOrder, amount FROM crossaddsdrinkorder WHERE id_drinksOrder = @id_drinksOrder AND id_adds = 1";
                using (MySqlCommand checkorder = new MySqlCommand(sqlmilk, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_adds", 1);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truemilk);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossaddsdrinkorder SET amount =  amount + 1  WHERE id_adds = 1 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_adds", 1);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truemilk);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertmilk = "INSERT INTO crossaddsdrinkorder (id_adds, id_drinksOrder, amount) " + "VALUES (1, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertmilk, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_adds", 1);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truemilk);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!!\nесли напитков несколько, укажите в комментариях к заказу к какому напитку нужно добавить", cancellationToken: token);
                        }

                    }
                }
                break;

            case "cinnamon":
                int idcinnamon = 0;
                int truecinnamon = 0;
                string getautocinnamon = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautocinnamon, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            idcinnamon = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truecinnamon = idcinnamon - 1;
                        readeridorderdrink.Close();
                    }
                }
                string sqlcinnamon = "SELECT id_adds, id_drinksOrder, amount FROM crossaddsdrinkorder WHERE id_drinksOrder = @id_drinksOrder AND id_adds = 2";
                using (MySqlCommand checkorder = new MySqlCommand(sqlcinnamon, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_adds", 2);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truecinnamon);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossaddsdrinkorder SET amount =  amount + 1  WHERE id_adds = 2 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_adds", 2);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truecinnamon);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertcinnamon = "INSERT INTO crossaddsdrinkorder (id_adds, id_drinksOrder, amount) " + "VALUES (2, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertcinnamon, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_adds", 2);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truecinnamon);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!\nесли напитков несколько, укажите в комментариях к заказу к какому напитку нужно добавить", cancellationToken: token);
                        }
                    }
                }
                break;

            case "sugar":
                int idsugar = 0;
                int truesugar = 0;
                string getautosugar = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautosugar, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            idsugar = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truesugar = idsugar - 1;
                        readeridorderdrink.Close();
                    }
                }
                string sqlsugar = "SELECT id_adds, id_drinksOrder, amount FROM crossaddsdrinkorder WHERE id_drinksOrder = @id_drinksOrder AND id_adds = 3";
                using (MySqlCommand checkorder = new MySqlCommand(sqlsugar, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_adds", 3);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truesugar);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossaddsdrinkorder SET amount =  amount + 1  WHERE id_adds = 3 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_adds", 3);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truesugar);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertsugar = "INSERT INTO crossaddsdrinkorder (id_adds, id_drinksOrder, amount) " + "VALUES (3, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertsugar, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_adds", 3);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truesugar);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!\nесли напитков несколько, укажите в комментариях к заказу к какому напитку нужно добавить", cancellationToken: token);
                        }
                    }
                }
                break;

            case "marshmallow":
                int idmarsh = 0;
                int truemarsh = 0;
                string getautomarsh = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautomarsh, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            idmarsh = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemarsh = idmarsh - 1;
                        readeridorderdrink.Close();
                    }
                }
                string sqlmarsh = "SELECT id_adds, id_drinksOrder, amount FROM crossaddsdrinkorder WHERE id_drinksOrder = @id_drinksOrder AND id_adds = 4";
                using (MySqlCommand checkorder = new MySqlCommand(sqlmarsh, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_adds", 4);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truemarsh);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossaddsdrinkorder SET amount =  amount + 1  WHERE id_adds = 4 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_adds", 4);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truemarsh);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertmarsh = "INSERT INTO crossaddsdrinkorder (id_adds, id_drinksOrder, amount) " + "VALUES (4, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertmarsh, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_adds", 4);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truemarsh);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!\nесли напитков несколько, укажите в комментариях к заказу к какому напитку нужно добавить", cancellationToken: token);
                        }
                    }
                }
                break;

            case "syrup":
                int idsyrup = 0;
                int truesyrup = 0;
                string getautosyrup = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautosyrup, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            idmilk = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemilk = idsyrup - 1;
                        readeridorderdrink.Close();
                    }
                }
                string descsyrup = "SELECT description FROM adds WHERE id = 5";
                using (MySqlCommand searchsyrup = new MySqlCommand(descsyrup, connect))
                {
                    using (MySqlDataReader Readsyrup = searchsyrup.ExecuteReader())
                    {
                        if (Readsyrup.Read())
                        {

                            string desc = Readsyrup.GetString("description");
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, $"в наличии: {desc}", cancellationToken: token);
                            Readsyrup.Close();
                        }

                    }
                }
                string sqlsyrup = "SELECT id_adds, id_drinksOrder, amount FROM crossaddsdrinkorder WHERE id_drinksOrder = @id_drinksOrder AND id_adds = 5";
                using (MySqlCommand checkorder = new MySqlCommand(sqlsyrup, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_adds", 5);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truesyrup);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossaddsdrinkorder SET amount =  amount + 1 WHERE id_adds = 5 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_adds", 5);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truesyrup);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertsyrup = "INSERT INTO crossaddsdrinkorder (id_adds, id_drinksOrder, amount) " + "VALUES (5, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertsyrup, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_adds", 5);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truesyrup);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!!\nесли напитков несколько, укажите в комментариях к заказу к какому напитку нужно добавить", cancellationToken: token);
                        }
                    }
                }
                break;
        }

        //напитки - добавление напитков и добавление их количества
        switch (update.CallbackQuery?.Data)
        {
            case "americano":
                int id1 = 0;
                int trueam = 0;
                string getautoam = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoam, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id1 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueam = id1 - 1;
                    }
                }
                string sqlam = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 1";
                using (MySqlCommand checkorder = new MySqlCommand(sqlam, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 1);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", trueam);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 1 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 1);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", trueam);

                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertamericano = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (1, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertamericano, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 1);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueam);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "latte":
                int id2 = 0;
                int truelat = 0;
                string getautolat = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautolat, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id2 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truelat = id2 - 1;
                    }
                }
                string sqllat = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 2";
                using (MySqlCommand checkorder = new MySqlCommand(sqllat, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 2);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truelat);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 2 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 2);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truelat);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertlatte = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (2, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertlatte, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 2);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truelat);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "cappuchino":
                int id3 = 0;
                int truecap = 0;
                string getautocap = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautocap, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id3 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truecap = id3 - 1;
                    }
                }
                string sqlcap = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 3";
                using (MySqlCommand checkorder = new MySqlCommand(sqlcap, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 3);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truecap);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 3 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 3);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truecap);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");

                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertcappuchino = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (3, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertcappuchino, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 3);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truecap);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;


            case "matchalatte":
                int id4 = 0;
                int truemlat = 0;
                string getautomlat = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautomlat, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id4 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemlat = id4 - 1;
                    }
                }
                string sqlmlat = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 4";
                using (MySqlCommand checkorder = new MySqlCommand(sqlmlat, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 4);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truemlat);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 4 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 4);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truemlat);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertmatchalatte = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (4, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertmatchalatte, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 4);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truemlat);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "tea":
                int id5 = 0;
                int truetea = 0;
                string getautotea = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautotea, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id5 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truetea = id5 - 1;
                        readeridorderdrink.Close();
                    }
                }
                string sqltea = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 5";
                using (MySqlCommand checkorder = new MySqlCommand(sqltea, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 5);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truetea);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 5 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 5);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truetea);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string inserttea = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (5, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(inserttea, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 5);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truetea);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                        string descTea = "SELECT description FROM drinks WHERE id = 5";
                        using (MySqlCommand searchtea = new MySqlCommand(descTea, connect))
                        {
                            using (MySqlDataReader ReadTea = searchtea.ExecuteReader())
                            {
                                if (ReadTea.Read())
                                {
                                    string desc = ReadTea.GetString("description");
                                    await client.SendTextMessageAsync(update.CallbackQuery.From.Id, $"в наличии: {desc}", cancellationToken: token);

                                }
                                ReadTea.Close();
                            }
                        }
                    }
                }
                break;
            case "cacao":
                int id6 = 0;
                int trueca = 0;
                string getautoca = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoca, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id6 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueca = id6 - 1;
                    }
                }
                string sqlca = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 6";
                using (MySqlCommand checkorder = new MySqlCommand(sqlca, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 6);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", trueca);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 6 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 6);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", trueca);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertcacao = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (6, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertcacao, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 6);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueca);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;
            case "kissel":
                int id7 = 0;
                int truekissel = 0;
                string getautokissel = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautokissel, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id7 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truekissel = id7 - 1;
                    }
                }
                string sqlkissel = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 7";
                using (MySqlCommand checkorder = new MySqlCommand(sqlkissel, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 7);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truekissel);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 7 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 7);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truekissel);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertkissel = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (7, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertkissel, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 7);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truekissel);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "dlyagyliya":
                int id8 = 0;
                int truegyl = 0;
                string getautogyl = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautogyl, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id8 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truegyl = id8 - 1;
                    }
                }
                string sqlgyl = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 8";
                using (MySqlCommand checkorder = new MySqlCommand(sqlgyl, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 8);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truegyl);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 8 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 8);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truegyl);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertdlyagyliya = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (8, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertdlyagyliya, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 8);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truegyl);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "ochenstranniedela":
                int id9 = 0;
                int trueosd = 0;
                string getautoosd = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoosd, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id9 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueosd = id9 - 1;
                    }
                }
                string sqlosd = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 9";
                using (MySqlCommand checkorder = new MySqlCommand(sqlosd, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 9);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", trueosd);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 9 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 9);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", trueosd);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertochenstranniedela = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (9, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertochenstranniedela, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 9);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueosd);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;
            case "bennet":
                int id10 = 0;
                int trueben = 0;
                string getautoben = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoben, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id10 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueben = id10 - 1;
                    }
                }
                string sqlben = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 10";
                using (MySqlCommand checkorder = new MySqlCommand(sqlben, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 10);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", trueben);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 10 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 10);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", trueben);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertbennet = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (10, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertbennet, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 10);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueben);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;
            case "hinatashoe":
                int id11 = 0;
                int truehs = 0;
                string getautohs = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautohs, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {

                        if (readeridorderdrink.Read())
                        {
                            id11 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truehs = id11 - 1;
                    }
                }
                string sqlhs = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 11";
                using (MySqlCommand checkorder = new MySqlCommand(sqlhs, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 11);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truehs);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 11 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 11);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truehs);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string inserthinatashoe = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (11, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(inserthinatashoe, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 11);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truehs);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "поизция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;
            case "kissa":
                int id12 = 0;
                int truekis = 0;
                string getautokis = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautokis, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id12 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truekis = id12 - 1;
                    }
                }
                string sqlkis = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 12";
                using (MySqlCommand checkorder = new MySqlCommand(sqlkis, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 12);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truekis);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 12 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 12);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truekis);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertkissa = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (12, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertkissa, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 12);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truekis);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "hakku":
                int id13 = 0;
                int truehak = 0;
                string getautohak = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautohak, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id13 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truehak = id13 - 1;
                    }
                }
                string sqlhak = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 13";
                using (MySqlCommand checkorder = new MySqlCommand(sqlhak, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 13);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truehak);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 13 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 13);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truehak);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string inserthakku = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (13, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(inserthakku, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 13);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truehak);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "xiao":
                int id14 = 0;
                int truexiao = 0;
                string getautoxiao = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoxiao, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id14 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truexiao = id14 - 1;
                    }
                }
                string sqlxiao = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 14";
                using (MySqlCommand checkorder = new MySqlCommand(sqlxiao, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 14);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truexiao);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 14 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 14);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truexiao);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertxiao = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (14, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertxiao, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 14);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truexiao);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "rei":
                int id15 = 0;
                int truerei = 0;
                string getautorei = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautorei, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id15 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truerei = id15 - 1;
                    }
                }
                string sqlrei = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 15";
                using (MySqlCommand checkorder = new MySqlCommand(sqlrei, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 15);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truerei);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 15 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 15);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truerei);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertrei = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (15, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertrei, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 15);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truerei);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "moodl":
                int id16 = 0;
                int truemoodl = 0;
                string getautomoodl = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautomoodl, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id16 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemoodl = id16 - 1;
                    }
                }
                string sqlmoodl = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 16";
                using (MySqlCommand checkorder = new MySqlCommand(sqlmoodl, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 16);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truemoodl);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 16 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 16);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truemoodl);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertmoodl = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (16, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertmoodl, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 16);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truemoodl);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "barbiestyle":
                int id17 = 0;
                int truebs = 0;
                string getautobs = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautobs, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id17 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truebs = id17 - 1;
                    }
                }
                string sqlbs = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 17";
                using (MySqlCommand checkorder = new MySqlCommand(sqlbs, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 17);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truebs);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 17 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 17);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truebs);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertbarbiestyle = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (17, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertbarbiestyle, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 17);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truebs);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);

                        }
                    }
                }
                break;

            case "myataishocolad":
                int id18 = 0;
                int truemintchoko = 0;
                string getautomintchoko = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautomintchoko, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id18 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truemintchoko = id18 - 1;
                    }
                }
                string sqlmintchoko = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 18";
                using (MySqlCommand checkorder = new MySqlCommand(sqlmintchoko, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 18);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truemintchoko);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 18 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 18);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truemintchoko);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertmyataishocolad = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (18, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertmyataishocolad, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 18);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truemintchoko);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "vedmachiysbor":
                int id19 = 0;
                int truevs = 0;
                string getautovs = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautovs, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id19 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        truevs = id19 - 1;
                    }
                }
                string sqlvs = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 19";
                using (MySqlCommand checkorder = new MySqlCommand(sqlvs, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 19);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", truevs);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 19 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 19);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", truevs);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertvedmachiysbor = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (19, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertvedmachiysbor, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 19);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", truevs);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;

            case "flatwhite":
                int id20 = 0;
                int trueflwh = 0;
                string getautoflwh = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand addidorderdrink = new MySqlCommand(getautoflwh, connect))
                {
                    using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id20 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueflwh = id20 - 1;
                    }
                }
                string sqlflwh = "SELECT id_drinks, id_drinksOrder, amount FROM crossdrinksorderdrinks WHERE id_drinksOrder = @id_drinksOrder AND id_drinks = 20";
                using (MySqlCommand checkorder = new MySqlCommand(sqlflwh, connect))
                {
                    checkorder.Parameters.AddWithValue("@id_drinks", 20);
                    checkorder.Parameters.AddWithValue("@id_drinksOrder", trueflwh);
                    checkorder.Parameters.AddWithValue("@amount", 1);
                    using (MySqlDataReader orders = checkorder.ExecuteReader())
                    {
                        if (orders.Read())
                        {
                            orders.Close();
                            string changeamount = "UPDATE crossdrinksorderdrinks SET amount =  amount + 1  WHERE id_drinks = 20 AND id_drinksOrder = @id_drinksOrder;";
                            using (MySqlCommand ChangeAmount = new MySqlCommand(changeamount, connect))
                            {
                                ChangeAmount.Parameters.AddWithValue("@id_drinks", 20);
                                ChangeAmount.Parameters.AddWithValue("@id_drinksOrder", trueflwh);
                                using (MySqlDataReader ReadOrder = ChangeAmount.ExecuteReader())
                                {
                                    ReadOrder.Read();
                                    ReadOrder.Close();
                                }
                                Console.WriteLine("количество успешно изменено!");
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "количество изменено!", cancellationToken: token);
                        }
                        else
                        {
                            orders.Close();
                            string insertflatwhite = "INSERT INTO crossdrinksorderdrinks (id_drinks, id_drinksOrder, amount) " + "VALUES (20, @id_drinksOrder, 1);";
                            using (MySqlCommand adddrinkorderdrink = new MySqlCommand(insertflatwhite, connect))
                            {
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinks", 20);
                                adddrinkorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueflwh);
                                adddrinkorderdrink.Parameters.AddWithValue("@amount", 1);
                                adddrinkorderdrink.ExecuteReader();
                            }
                            await client.SendTextMessageAsync(update.CallbackQuery.From.Id, "позиция добавлена!", cancellationToken: token);
                        }
                    }
                }
                break;
        }

        switch (update.CallbackQuery?.Data)
        {
            case "max":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/menu.jpeg"), caption: "глянь меню и выбери категорию", replyMarkup: newOrderM, cancellationToken: token);
                break;
            case "small":
                await client.SendPhotoAsync(update.CallbackQuery.From.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/menu.jpeg"), caption: "глянь меню и выбери категорию", replyMarkup: newOrderS, cancellationToken: token);
                break;
        }

        switch (update.Message?.Text?.ToLower())
        {
            case "/start":
                await client.SendPhotoAsync(update.Message.Chat.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/greeting_photo.jpeg"), caption: "На связи Кисса-бот!\nВыбери нужную команду;)", replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                string sql = "SELECT id, phone_number FROM customer WHERE id = @id";
                using (MySqlCommand checkcustomers = new MySqlCommand(sql, connect))
                {
                    checkcustomers.Parameters.AddWithValue("@id", update.Message.Chat.Id);
                    using (MySqlDataReader customers = checkcustomers.ExecuteReader())
                    {
                        if (customers.Read())
                        {
                            long Id = customers.GetInt64("id");
                            string phonenumber = customers.GetString("phone_number");
                            Console.WriteLine($"пользователь: {Id} - номер {phonenumber}");
                        }
                        else
                        {
                            customers.Close();
                            string insertcustomer = "INSERT INTO customer (id, phone_number)" + "VALUES (@id, '/start')";
                            using (MySqlCommand addcustomers = new MySqlCommand(insertcustomer, connect))
                            {
                                addcustomers.Parameters.AddWithValue("@id", update.Message.Chat.Id);
                                Console.WriteLine($"успешно добавлен пользователь с айди: {update.Message.Chat.Id}, НОМЕР МОЖНО ИЗМЕНИТЬ");
                                using (MySqlDataReader readcustomers = addcustomers.ExecuteReader())
                                    readcustomers.Read();
                            }
                        }
                    }
                }
                update.Message.Text = null;
                break;

            //завершение заказа, поиск последнего заказа, проверка его существования, включение флажка подверждения ответа завершения заказа
            case "завершить заказ":
                int id = 0;
                string searchorder = "SELECT id FROM drinkorder WHERE id_customer = @id_customer";
                using (MySqlCommand searchorders = new MySqlCommand(searchorder, connect))
                {
                    searchorders.Parameters.AddWithValue("@id_customer", update.Message.Chat.Id);
                    using (MySqlDataReader readeridorderdrink = searchorders.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id = readeridorderdrink.GetInt32("id");
                        }
                    }
                    if (id != 0)
                    {
                        int newid1 = 0;
                        string newsearchorder1 = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                        using (MySqlCommand searchorders1 = new MySqlCommand(newsearchorder1, connect))
                        {
                            using (MySqlDataReader readeridorderdrink = searchorders1.ExecuteReader())
                            {
                                if (readeridorderdrink.Read())
                                {
                                    newid1 = readeridorderdrink.GetInt32("Auto_increment");
                                    string checkstatus = "SELECT status FROM drinkorder d WHERE id = @id_drinksOrder";
                                    using (MySqlCommand searchorderstatus = new MySqlCommand(checkstatus, connect))
                                    {
                                        searchorderstatus.Parameters.AddWithValue("@id_drinksOrder", newid1 - 1);
                                        readeridorderdrink.Close();
                                        using (MySqlDataReader Readorder = searchorderstatus.ExecuteReader())
                                        {
                                            if (Readorder.Read())
                                            {
                                                string status = Readorder.GetString("status");
                                                if (status == "Не готов")
                                                {
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "вы уверены, что хотите завершить заказ?", cancellationToken: token);
                                                    flagAskYesNo = true;
                                                }
                                                if (status == "Готов" || status == "Готовится")
                                                {
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас нет активных заказов", cancellationToken: token);
                                                }
                                                Readorder.Close();
                                            }
                                            else
                                            {
                                                await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас нет активных заказов", cancellationToken: token);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас нет активных заказов", cancellationToken: token);
                                }
                            }
                        }
                    }
                    if (id == 0)
                    {
                        flagAskYesNo = false;
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас нет активных заказов", cancellationToken: token);
                    }
                }
                break;

            //создание нового заказа, включение флажка добавление номера, метод создания заказа
            case "новый заказ":
                int newid = 0;
                string newsearchorder = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand searchorders = new MySqlCommand(newsearchorder, connect))
                {
                    using (MySqlDataReader readeridorderdrink = searchorders.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            newid = readeridorderdrink.GetInt32("Auto_increment");
                            string checkstatus = "SELECT status FROM drinkorder d WHERE id = @id_drinksOrder";
                            using (MySqlCommand searchorderstatus = new MySqlCommand(checkstatus, connect))
                            {
                                searchorderstatus.Parameters.AddWithValue("@id_drinksOrder", newid - 1);
                                readeridorderdrink.Close();
                                using (MySqlDataReader Readorder = searchorderstatus.ExecuteReader())
                                {
                                    if (Readorder.Read())
                                    {

                                        string status = Readorder.GetString("status");
                                        if (status == "Не готов")
                                        {
                                            await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас уже есть активный заказ!", cancellationToken: token);

                                        }
                                        Readorder.Close();
                                    }
                                    else
                                    {
                                        Readorder.Close();
                                        await client.SendTextMessageAsync(update.Message.Chat.Id, "чтобы продолжить оформлять заказ, пожалуйста, укажите ваш номер телефона для связи", cancellationToken: token);
                                        flagAskPhone = true;
                                        MakeNewOrder(connect, update);
                                        update.Message.Text = null;
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case "меню":
                await client.SendPhotoAsync(update.Message.Chat.Id, InputFile.FromUri("https://raw.githubusercontent.com/r0zmarin1/tgbot-console-/master/tgbot/docs/menu.jpeg"), replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                update.Message.Text = null;
                break;

            case "статус заказа":
                int id1 = 0;
                int trueid1 = 0;
                string searchorder1 = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                using (MySqlCommand searchorders = new MySqlCommand(searchorder1, connect))
                {
                    using (MySqlDataReader readeridorderdrink = searchorders.ExecuteReader())
                    {
                        if (readeridorderdrink.Read())
                        {
                            id1 = readeridorderdrink.GetInt32("Auto_increment");
                        }
                        trueid1 = id1 - 1;
                        readeridorderdrink.Close();
                    }
                    if (id1 != 0)
                    {
                        //запрос всего заказа...
                        string sqlorder = "SELECT status, date, time, description FROM drinkorder d WHERE id = @id_drinksOrder";
                        using (MySqlCommand searchorderdrink = new MySqlCommand(sqlorder, connect))
                        {
                            searchorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueid1);
                            using (MySqlDataReader Readorder = searchorderdrink.ExecuteReader())
                            {
                                if (Readorder.Read())
                                {
                                    string status = Readorder.GetString("status");
                                    DateTime date = Readorder.GetDateTime("date");
                                    string time = Readorder.GetString("time");
                                    string desc = Readorder.GetString("description");
                                    decimal totalPrice = 0;
                                    decimal totalPricea = 0;
                                    decimal Price = 0;
                                    decimal pricedrink = 0;
                                    decimal pricewithadds = 0;
                                    //var result = new List<string>();
                                    string sqldrinkorder = "SELECT c.amount, d.title, d.price, d.size FROM crossdrinksorderdrinks c LEFT outer JOIN drinks d ON c.id_drinks = d.id WHERE c.id_drinksOrder = @id_drinksOrder;";
                                    using (MySqlCommand searchdrinkorder = new MySqlCommand(sqldrinkorder, connect))
                                    {
                                        searchdrinkorder.Parameters.AddWithValue("@id_drinksOrder", trueid1);
                                        Readorder.Close();
                                        using (MySqlDataReader Readdrinkorder = searchdrinkorder.ExecuteReader())
                                        {
                                            await client.SendTextMessageAsync(update.Message.Chat.Id, $"ваш заказ:\nномер: {trueid1}\nстатус: {status}\nдата и время заказа: {date}\nваше желаемое время: {time}\nваш комментарий к заказу: {desc}", cancellationToken: token);

                                            while (Readdrinkorder.Read())
                                            {
                                                int amountd = Readdrinkorder.GetInt32("amount");
                                                string titled = Readdrinkorder.GetString("title");
                                                Price = Readdrinkorder.GetDecimal("price") * Readdrinkorder.GetInt32("amount");
                                                string sized = Readdrinkorder.GetString("size");
                                                totalPrice += Price;
                                                //result.Add($"ваш заказ:\nномер: {trueid1}\nстатус: {status}\nдата и время заказа: {date}\nваше желаемое время: {time}\nваш комментарий к заказу: {desc}\n\nнапиток: {titled}, {amountd} шт., {sized}");
                                                await client.SendTextMessageAsync(update.Message.Chat.Id, $"напиток: {titled}, {amountd} шт., {sized}", cancellationToken: token);
                                            }
                                            //await client.SendTextMessageAsync(update.Message.Chat.Id, $"напиток: {titled}, {amountd} шт., {sized}", cancellationToken: token);
                                            Readdrinkorder.Close();
                                        }
                                    }
                                    pricedrink = totalPrice;
                                    string sqladdsdrinkorder = "SELECT c.amount, a.title, a.price, c.id_drinksOrder  FROM crossaddsdrinkorder c LEFT outer JOIN adds a ON c.id_adds = a.id WHERE c.id_drinksOrder = @id_drinksOrder;";
                                    using (MySqlCommand searchorderadds = new MySqlCommand(sqladdsdrinkorder, connect))
                                    {
                                        if (searchorderadds != null)
                                        {
                                            searchorderadds.Parameters.AddWithValue("@id_drinksOrder", trueid1);
                                            using (MySqlDataReader Readaddsorder = searchorderadds.ExecuteReader())
                                            {
                                                if (Readaddsorder.Read())
                                                {
                                                    int amounta = Readaddsorder.GetInt32("amount");
                                                    string titlea = Readaddsorder.GetString("title");
                                                    totalPricea += Readaddsorder.GetDecimal("price");
                                                    pricewithadds = totalPricea + pricedrink;
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"добавки: {titlea}, {amounta} шт.", cancellationToken: token);
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"всего к оплате: {pricewithadds}", cancellationToken: token);
                                                }
                                                Readaddsorder.Close();
                                            }
                                        }
                                        else
                                            await client.SendTextMessageAsync(update.Message.Chat.Id, $"всего к оплате: {pricedrink}", cancellationToken: token);

                                    }
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(update.Message.Chat.Id, "у вас нет активных заказов", cancellationToken: token);
                                }
                            }
                        }
                    }
                }
                break;

            default:
                {
                    //флажок на добавление номера в покупателя
                    if (flagAskPhone == true)
                    {

                        string changecustomer = "UPDATE customer SET `id` = @id, `phone_number` =  @phone_number WHERE id = @id;";
                        using (MySqlCommand ChangeCustomer = new MySqlCommand(changecustomer, connect))
                        {
                            ChangeCustomer.Parameters.AddWithValue("@phone_number", update.Message.Text);
                            ChangeCustomer.Parameters.AddWithValue("@id", update.Message.Chat.Id);
                            using (MySqlDataReader ReadNowCustomer = ChangeCustomer.ExecuteReader())
                            {
                                ReadNowCustomer.Read();
                                ReadNowCustomer.Close();

                            }
                            Console.WriteLine("номер телефона успешно изменен!");
                            flagAskPhone = false;

                        }
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "выбери размер!", replyMarkup: size, cancellationToken: token);
                    }
                    // флажок на добавление времени к заказу
                    if (flagAskTime == true)
                    {
                        int idod = 0;
                        int trueid = 0;
                        string getautoid = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                        using (MySqlCommand addidorderdrink = new MySqlCommand(getautoid, connect))
                        {
                            using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                            {
                                if (readeridorderdrink.Read())
                                {
                                    idod = readeridorderdrink.GetInt32("Auto_increment");
                                }
                                trueid = idod - 1;
                                readeridorderdrink.Close();
                            }
                        }
                        string changetimeandstatus = "UPDATE drinkorder SET time = @time, status = 'Готовится' WHERE id = @id;";
                        using (MySqlCommand ChangeTime = new MySqlCommand(changetimeandstatus, connect))
                        {
                            ChangeTime.Parameters.AddWithValue("@id", trueid);
                            ChangeTime.Parameters.AddWithValue("@time", update.Message?.Text);
                            using (MySqlDataReader ReadNowTime = ChangeTime.ExecuteReader())
                            {
                                ReadNowTime.Read();
                                ReadNowTime.Close();
                            }
                            Console.WriteLine("время успешно изменено!");
                            flagAskTime = false;

                        }
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "готово! заказ создан^^ ", cancellationToken: token);

                        //запрос всего заказа...
                        string sqlorder = "SELECT status, date, time, description FROM drinkorder d WHERE id = @id_drinksOrder";
                        using (MySqlCommand searchorderdrink = new MySqlCommand(sqlorder, connect))
                        {
                            searchorderdrink.Parameters.AddWithValue("@id_drinksOrder", trueid);
                            using (MySqlDataReader Readorder = searchorderdrink.ExecuteReader())
                            {
                                if (Readorder.Read())
                                {
                                    //int idd = Readorder.GetInt32("id");
                                    string status = Readorder.GetString("status");
                                    DateTime date = Readorder.GetDateTime("date");
                                    string time = Readorder.GetString("time");
                                    string desc = Readorder.GetString("description");
                                    Readorder.Close();
                                    decimal totalPrice = 0;
                                    decimal totalPricea = 0;
                                    decimal Price = 0;
                                    decimal pricedrink = 0;
                                    decimal pricewithadds = 0;
                                    string sqldrinkorder = "SELECT c.amount, d.title, d.price, d.size FROM crossdrinksorderdrinks c LEFT outer JOIN drinks d ON c.id_drinks = d.id WHERE c.id_drinksOrder = @id_drinksOrder;";
                                    using (MySqlCommand searchdrinkorder = new MySqlCommand(sqldrinkorder, connect))
                                    {
                                        searchdrinkorder.Parameters.AddWithValue("@id_drinksOrder", trueid);
                                        using (MySqlDataReader Readdrinkorder = searchdrinkorder.ExecuteReader())
                                        {
                                            if (Readdrinkorder.Read())
                                            {
                                                int amountd = Readdrinkorder.GetInt32("amount");
                                                string titled = Readdrinkorder.GetString("title");
                                                Price = Readdrinkorder.GetDecimal("price") * Readdrinkorder.GetInt32("amount");
                                                string sized = Readdrinkorder.GetString("size");
                                                totalPrice += Price;
                                                Readdrinkorder.Close();
                                                await client.SendTextMessageAsync(update.Message.Chat.Id, $"ваш заказ:\nномер: {trueid}\nстатус: {status}\nдата и время заказа: {date}\nваше желаемое время: {time}\nваш комментарий к заказу: {desc}\n\nнапиток: {titled}, {amountd} шт., {sized}", cancellationToken: token);
                                            }
                                            else
                                            {
                                                await client.SendTextMessageAsync(update.Message.Chat.Id, "помогите?", cancellationToken: token);

                                            }
                                        }
                                    }
                                    pricedrink = totalPrice;
                                    string sqladdsdrinkorder = "SELECT c.amount, a.title, a.price, c.id_drinksOrder  FROM crossaddsdrinkorder c LEFT outer JOIN adds a ON c.id_adds = a.id WHERE c.id_drinksOrder = @id_drinksOrder;";
                                    using (MySqlCommand searchorderadds = new MySqlCommand(sqladdsdrinkorder, connect))
                                    {
                                        if (searchorderadds != null)
                                        {
                                            searchorderadds.Parameters.AddWithValue("@id_drinksOrder", trueid);
                                            using (MySqlDataReader Readaddsorder = searchorderadds.ExecuteReader())
                                            {
                                                if (Readaddsorder.Read())
                                                {
                                                    int amounta = Readaddsorder.GetInt32("amount");
                                                    string titlea = Readaddsorder.GetString("title");
                                                    totalPricea += Readaddsorder.GetDecimal("price");
                                                    pricewithadds = totalPricea + pricedrink;
                                                    Readaddsorder.Close();
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"добавки: {titlea}, {amounta} шт.", cancellationToken: token);
                                                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"всего к оплате: {pricewithadds}", cancellationToken: token);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            await client.SendTextMessageAsync(update.Message.Chat.Id, $"всего к оплате: {pricedrink}", cancellationToken: token);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    // флажок на добавление комментария к заказу
                    if (flagAskCom == true)
                    {
                        int idod = 0;
                        int trueid = 0;
                        string getautoid = "SHOW TABLE STATUS WHERE `Name` = '" + "drinkorder" + "'";
                        using (MySqlCommand addidorderdrink = new MySqlCommand(getautoid, connect))
                        {
                            using (MySqlDataReader readeridorderdrink = addidorderdrink.ExecuteReader())
                            {
                                if (readeridorderdrink.Read())
                                {
                                    idod = readeridorderdrink.GetInt32("Auto_increment");
                                }
                                trueid = idod - 1;
                                readeridorderdrink.Close();
                            }
                        }
                        string changedescription = "UPDATE drinkorder SET description = @description WHERE id = @id;";
                        using (MySqlCommand ChangeDesc = new MySqlCommand(changedescription, connect))
                        {
                            ChangeDesc.Parameters.AddWithValue("@id", trueid);
                            ChangeDesc.Parameters.AddWithValue("@description", update.Message?.Text);
                            using (MySqlDataReader ReadNowDesc = ChangeDesc.ExecuteReader())
                            {
                                ReadNowDesc.Read();
                                ReadNowDesc.Close();
                            }
                            Console.WriteLine("описание успешно изменено!");
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "описание добавлено!", cancellationToken: token);
                            flagAskCom = false;
                            update.Message.Text = null;
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "добавь время к заказу!", cancellationToken: token);
                            flagAskTime = true;
                        }


                    }
                    //флажок ожидания ответа на вопрос о завершении заказа
                    if (flagAskYesNo == true)
                    {
                        if (update.Message?.Text == "да")
                        {
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "идем дальше!", cancellationToken: token);
                            flagAskYesNo = false;
                            update.Message.Text = null;
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "добавь комментарий к заказу!", cancellationToken: token);
                            flagAskCom = true;
                        }
                    }
                    if (update.Message?.Text == "нет")
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "выбери размер!", replyMarkup: size, cancellationToken: token);
                        flagAskYesNo = false;
                        update.Message.Text = null;
                    }
                }
                break;
        }
    }

    //метод добавления нового заказа
    private static void MakeNewOrder(MySqlConnection connect, Update update)
    {
        string insertintodrinkorder = "INSERT INTO DrinkOrder (status, id_customer, date, time, description) " + "VALUES (@status, @id_customer, @date, @time, @description);";
        using (MySqlCommand addorderdrink = new MySqlCommand(insertintodrinkorder, connect))
        {
            addorderdrink.Parameters.AddWithValue("@status", "Не готов");
            addorderdrink.Parameters.AddWithValue("@id_customer", update.Message.Chat.Id);
            addorderdrink.Parameters.AddWithValue("@date", DateTime.Now);
            addorderdrink.Parameters.AddWithValue("@time", "не указано");
            addorderdrink.Parameters.AddWithValue("@description", $"не указано");
            Console.WriteLine($"успешно добавлен заказ пользователя с айди: {update.Message.Chat.Id}");
            using (MySqlDataReader readorderdrink = addorderdrink.ExecuteReader())
            {
                readorderdrink.Read();
                readorderdrink.Close();
            }
        }
    }
}



