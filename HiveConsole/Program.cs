// See https://aka.ms/new-console-template for more information

using Hive.Console;
using Hive.Console.Visualiser;
using Hive.Console.Visualiser.Printers;
using Hive.Core;
using Hive.Core.Models;

// Player p1 = new ConsolePlayer("Hans", Color.Black);
// Player p2 = new ConsolePlayer("Frans", Color.White);

// Match m = new Match(p1, p2);
// m.Start();


// Create an instance of AsciiBoard with the specified parameters
AsciiBoard board = new AsciiBoard(0, 2, 0, 1, new SmallFlatAsciiHexPrinter());

// Add hexagons to the board with specified text, filler character, and coordinates
board.AddHex("HX1", "-B-", '#', 0, 0);
board.AddHex("HX2", "-W-", '+', 1, 0);
board.AddHex("HX3", "-W-", 'x', 2, 0);
board.AddHex("HX3", "-B-", '•', 2, 1);

// Print the board with a bounding box around it
string result = board.PrettyPrint(true);

// Display the result
Console.WriteLine(result);
