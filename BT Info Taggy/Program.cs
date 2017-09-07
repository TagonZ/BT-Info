using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT_Info_Taggy
{
    class Program
    {
        static void Main(string[] args) => new Program().Run();

        private DiscordClient _client;

        public void Run()
        {
            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            //Status btuut not working yet
            //Game g = new Game("!help");
            //_client.SetGame(g);
            
            /* Set ups */
            #region Set ups

            _client.UsingCommands(x => {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });
            
            var commands = _client.GetService<CommandService>();
            #endregion

            commands.CreateCommand("help")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(@"Commands for true admin:

!setdoodle <link or something> - saving doodle
!setdoodle2 <link or something> - saving doodle
!setsheet <link or something> - saving sheet
!setsheet2 <link or something> - saving sheet2

                                               
If you want to get what you setted up, just follow those commands:

!doodle - return what you set with !setdoodle
!doodle2 - return what you set with !setdoodle2
!sheet - return what you set with !setsheet
!sheet2 - return what you set with !setsheet2


You can set any text with those commands, but I created that for some help. Hope you like it.

Regards,
Tagon :confused:");
                });

            commands.CreateCommand("setdoodle").Parameter("message", ParameterType.Multiple)
                .AddCheck((cmd, user, channel) => user.ServerPermissions.Administrator)
                .Do(async (e) =>
                {
                    await DoSet(e, "doodle");   
                });
            commands.CreateCommand("setdoodle2").Parameter("message", ParameterType.Multiple)
                .AddCheck((cmd, user, channel) => user.ServerPermissions.Administrator)
                .Do(async (e) =>
                {
                    await DoSet(e, "doodle2");
                });
            commands.CreateCommand("setsheet").Parameter("message", ParameterType.Multiple)
                .AddCheck((cmd, user, channel) => user.ServerPermissions.Administrator)
                .Do(async (e) =>
                {
                    await DoSet(e, "sheet");
                });
            commands.CreateCommand("setsheet2").Parameter("message", ParameterType.Multiple)
                .AddCheck((cmd, user, channel) => user.ServerPermissions.Administrator)
                .Do(async (e) =>
                {
                    await DoSet(e, "sheet2");
                });

            commands.CreateCommand("doodle")
                .Description("Return what you set with !setdoodle")
                .Do(async (e) =>
                {
                    string sv = e.Server.Name;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\discord_" + sv + "-doodle.dbdc";
                    var message = "Something went weird :frowning:";

                    if (File.Exists(path))
                    {
                        try
                        {
                            message = File.ReadAllText(path);
                        } catch (Exception ex) { message = "I couldn't get that what you wanted :sob: You can slap me. Sorry.."; }
                    }
                    else if (!File.Exists(path))
                    {
                        message = "Nothing was set up..";
                    }
                    ConsoleWrite("!doodle was typed");
                    await e.Channel.SendMessage(message);
                });
            commands.CreateCommand("doodle2")
                .Description("Return what you set with !setdoodle2")
                .Do(async (e) =>
                {
                    string sv = e.Server.Name;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\discord_" + sv + "-doodle2.dbdc";
                    var message = "Something went weird :frowning:";

                    if (File.Exists(path))
                    {
                        try
                        {
                            message = File.ReadAllText(path);
                        }
                        catch (Exception ex) { message = "I couldn't get that what you wanted :sob: You can slap me. Sorry.."; }
                    }
                    else if (!File.Exists(path))
                    {
                        message = "Nothing was set up..";
                    }
                    ConsoleWrite("!doodle2 was typed");
                    await e.Channel.SendMessage(message);
                });
            commands.CreateCommand("sheet")
                .Description("Return what you set with !setsheet")
                .Do(async (e) =>
                {
                    string sv = e.Server.Name;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\discord_" + sv + "-sheet.dbdc";
                    var message = "Something went weird :frowning:";

                    if (File.Exists(path))
                    {
                        try
                        {
                            message = File.ReadAllText(path);
                        }
                        catch (Exception ex) { message = "I couldn't get that what you wanted :sob: You can slap me. Sorry.."; }
                    }
                    else if (!File.Exists(path))
                    {
                        message = "Nothing was set up..";
                    }
                    ConsoleWrite("!sheet was typed");
                    await e.Channel.SendMessage(message);
                });
            commands.CreateCommand("sheet2")
                .Description("Return what you set with !setsheet")
                .Do(async (e) =>
                {
                    string sv = e.Server.Name;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\discord_" + sv + "-sheet2.dbdc";
                    var message = "Something went weird :frowning:";

                    if (File.Exists(path))
                    {
                        try
                        {
                            message = File.ReadAllText(path);
                        }
                        catch (Exception ex) { message = "I couldn't get that what you wanted :sob: You can slap me. Sorry.."; }
                    }
                    else if (!File.Exists(path))
                    {
                        message = "Nothing was set up..";
                    }
                    ConsoleWrite("!sheet2 was typed");
                    await e.Channel.SendMessage(message);
                });

            _client.ExecuteAndWait(async () => {
                await _client.Connect("<your_token>", TokenType.Bot);
            });
        }

        private async Task DoSet(CommandEventArgs e, string what)
        {
            var servername = e.Server.Name;
            var channel = e.Server.FindChannels(e.Args[0], ChannelType.Text).FirstOrDefault();
            var message = ConstructMessage(e, channel != null, what, servername);


            if (channel != null)
            {
                await channel.SendMessage(message);
            }
            else
            {
                await e.Channel.SendMessage(message);
            }
        }

        private string ConstructMessage(CommandEventArgs e, bool firstArgIsChannel, string what, string servername)
        {
            string message = "";
            var name = e.User.Nickname != null ? e.User.Nickname : e.User.Name;
            var startIndex = firstArgIsChannel ? 1 : 0;

            for (int i = startIndex; i < e.Args.Length; i++)
            {
                message += e.Args[i].ToString() + " ";
            }

            string result = "Something went weird :frowning:";
            bool flag = saveNewSet(message, what, servername);

            if (flag)
            {
                result = "Thank you " + name + " for " + what + " :ok_hand:";
            }
            else
            {
                result = "Sorry.. I couldn't set new " + what + " :sob:";
            }
            return result;
        }

        private bool saveNewSet(string s, string what, string servername)
        {
            bool result = false;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\discord_" + servername + "-" + what + ".dbdc";
                if (!File.Exists(path))
                {
                    try
                    {
                        File.Create(path).Dispose();
                        ConsoleWrite("File created: " + path);
                    } catch (Exception ex) { ConsoleWrite(ex.Message); return result; }

                    using (var sw = new StreamWriter(path))
                    {
                        sw.WriteLine(s);
                        sw.Close();
                        result = true;
                    }
                }
                else if (File.Exists(path))
                {
                    using (var sw = new StreamWriter(path))
                    {
                        sw.WriteLine(s);
                        sw.Close();
                        ConsoleWrite("File modified: "+ path);
                        result = true;
                    }
                }
            } catch(Exception e) { ConsoleWrite(e.ToString()); return result; }
            return result;
        }

        public void ConsoleWrite(string s)
        {
            DateTime localDate = DateTime.Now;
            Console.WriteLine("[{0}] {1}", localDate, s);
        }

        public void Log(object sender, LogMessageEventArgs e)
        {
            ConsoleWrite(e.Message);
        }
    }
}