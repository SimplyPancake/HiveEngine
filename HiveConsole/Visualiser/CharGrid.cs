using System;
using System.Text;

namespace Hive.Console
{
	/// <summary>
	/// 2D representation of a char grid of fixed size. Useful for eg. creating ASCII art.
	/// (Top,Left) has coordinates (0,0).
	/// </summary>
	public class CharGrid
	{
		private readonly int _width;
		private readonly int _height;
		private readonly char[,] _grid;

		public CharGrid(int width, int height)
		{
			_width = width;
			_height = height;
			_grid = new char[_height, _width];
			PrefillGrid();
		}

		/// <summary>
		/// Prefill grid with spaces.
		/// </summary>
		private void PrefillGrid()
		{
			for (int i = 0; i < _height; i++)
			{
				for (int j = 0; j < _width; j++)
				{
					AddChar(j, i, ' ');
				}
			}
		}

		/// <summary>
		/// Add a string to the grid.
		///
		/// <param name="x">Starting x coordinate.</param>
		/// <param name="y">Starting y coordinate.</param>
		/// <param name="input">String put input. String will not wrap, but throws IndexOutOfBounds if too long.</param>
		/// </summary>
		public void AddString(int x, int y, string input)
		{
			if (string.IsNullOrEmpty(input)) return;

			for (int i = 0; i < input.Length; i++)
			{
				AddChar(x + i, y, input[i]);
			}
		}

		/// <summary>
		/// Add a character to the grid.
		///
		/// <param name="x">Starting x coordinate.</param>
		/// <param name="y">Starting y coordinate.</param>
		/// <param name="input">Char to insert. Throws IndexOutOfBounds if outside grid.</param>
		/// </summary>
		public void AddChar(int x, int y, char input)
		{
			if (x < 0 || x >= _width || y < 0 || y >= _height)
			{
				int maxWidth = _width - 1;
				int maxHeight = _height - 1;
				throw new IndexOutOfRangeException($"({x},{y}) is outside ({maxWidth},{maxHeight})");
			}
			_grid[y, x] = input;
		}

		/// <summary>
		/// Returns a char from the grid
		/// </summary>
		public char GetChar(int x, int y)
		{
			return _grid[y, x];
		}

		/// <summary>
		/// Returns the char grid as a string, ready for output.
		///
		/// <param name="trimToBoundingBox">If true, the grid is trimmed to its contents bounding box. If not, grid is printed as is.</param>
		/// </summary>
		public string Print(bool trimToBoundingBox)
		{
			int leftBound = trimToBoundingBox ? _width - 1 : 0;
			int rightBound = trimToBoundingBox ? 0 : _width - 1;
			int topBound = trimToBoundingBox ? _height - 1 : 0;
			int bottomBound = trimToBoundingBox ? 0 : _height - 1;

			// Find bounding box
			if (trimToBoundingBox)
			{
				for (int i = 0; i < _height; i++)
				{
					for (int j = 0; j < _width; j++)
					{
						char c = _grid[i, j];
						if (c != ' ')
						{
							leftBound = Math.Min(leftBound, j);
							rightBound = Math.Max(rightBound, j);
							topBound = Math.Min(topBound, i);
							bottomBound = Math.Max(bottomBound, i);
						}
					}
				}
			}

			// Print grid
			StringBuilder builder = new StringBuilder((_width + LINE_BREAK.Length) * _height);
			for (int i = topBound; i <= bottomBound; i++)
			{
				for (int j = leftBound; j <= rightBound; j++)
				{
					builder.Append(_grid[i, j]);
				}
				builder.Append(LINE_BREAK);
			}
			return builder.ToString();
		}

		private const string LINE_BREAK = "\n";
	}
}
