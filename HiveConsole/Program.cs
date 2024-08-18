// See https://aka.ms/new-console-template for more information

using Hive.Console;
using Hive.Core;
using Hive.Core.Models;

Player p1 = new ConsolePlayer("Hans", Color.Black);
Player p2 = new ConsolePlayer("Frans", Color.White);

Match m = new(p1, p2);
m.Start();
