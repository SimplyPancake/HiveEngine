// See https://aka.ms/new-console-template for more information

using Hive.Console;
using Hive.Core;

Player p1 = new ConsolePlayer("Hans", Color.Black);
Player p2 = new ConsolePlayer("Frans", Color.White);

Match m = new Match(p1, p2);
m.Start();