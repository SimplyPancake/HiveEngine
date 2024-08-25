using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Players;

Player p1 = new ConsolePlayer("Hans", Color.Black);
Player p2 = new ConsolePlayer("Frans", Color.White);

Match m = new(p1, p2);
m.Start();
