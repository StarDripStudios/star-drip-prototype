using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Framework
{
	public sealed class TemplateSystem
	{
		[Flags]
		private enum State
		{
			None = 0x0,
			ForEach = 0x1,
			ForEvery = 0x2
		}
		
		private readonly string _targetTemplate;
		private readonly Dictionary<string, object> _replaceLookup = new Dictionary<string, object>();
		private State _state = State.None;
		private object[] _iteratee;
		private int _currentIterateeIndex;
		private int _loopStart = -1;
		private bool _isEmpty;

		public TemplateSystem(string text)
		{
			_targetTemplate = text;
		}

		public void AddVariable(string key, object val)
		{
			_replaceLookup.Add(key, val);
		}

		public string Parse()
		{
			var lines = new List<string>(_targetTemplate.Replace("\r\n", "\n").Split('\n'));
			var finalLines = new List<string>(lines.Count);

			var offset = 0;
			var foundState = State.None;
			for (var i = 0; i < lines.Count; i++)
			{
				var parsed = false;
				var skipLine = false;
				var sb = new StringBuilder(lines[i]);

				while (true)
				{
					var current = sb.ToString();
					var parseStart = current.IndexOf(">:", offset, StringComparison.Ordinal);
					if (parseStart < 0)
						break;

					parseStart += 2;
					var parseEnd = current.IndexOf(":<", offset + parseStart, StringComparison.Ordinal);

					if (parseEnd < 0)
						throw new Exception("There was a parse start but no end on line " + (i + 1));

					var contents = current.Substring(parseStart, parseEnd - parseStart);

					sb.Remove(parseStart - 2, parseEnd - parseStart + 4);

					if (CheckState(contents, ref foundState))
					{
						skipLine = true;

						// If we have left the loop
						if (foundState == State.None)
						{
							if (_loopStart == -1)
								continue;

							if (++_currentIterateeIndex >= _iteratee.Length)
							{
								_state &= ~(State.ForEach | State.ForEvery);
								_iteratee = null;
								_loopStart = -1;
								_isEmpty = false;
							}
							else
							{
								i = _loopStart - 1;
								break;
							}
						}
						else if (foundState == State.ForEach || foundState == State.ForEvery)
							_loopStart = i + 1;

						continue;
					}

					if (!_isEmpty)
						sb.Insert(parseStart - 2, ParseLine(contents));

					parsed = true;
				}

				var built = sb.ToString();

				if (parsed && built.Trim().Length == 0)
					lines.RemoveAt(i--);
				else if (!skipLine && !_isEmpty)
					finalLines.Add(built);
			}

			return string.Join(Environment.NewLine, finalLines.ToArray());
		}

		private bool CheckState(string contents, ref State foundState)
		{
			if (contents.StartsWith("ENDFOREACH"))
			{
				if ((_state & State.ForEach) == 0)
					throw new Exception("A foreach has ended before the start of the loop");

				foundState = State.None;
				return true;
			}

			if (contents.StartsWith("ENDFOREVERY"))
			{
				if ((_state & State.ForEvery) == 0)
					throw new Exception("A foreach has ended before the start of the loop");

				foundState = State.None;
				return true;
			}
			
			if (contents.StartsWith("FOREACH"))
			{
				if ((_state & State.ForEach) != 0 || (_state & State.ForEvery) != 0)
					throw new Exception(
						"A loop is already in execution and in this version nested foreach loops are not allowed");

				_state |= State.ForEach;

				var iterateeName = contents.TrimStart("FOREACH ".ToCharArray());

				if (!_replaceLookup.ContainsKey(iterateeName))
					throw new Exception("No variable with the name " + iterateeName + " could be located");

				_iteratee = (object[]) _replaceLookup[iterateeName];

				if (_iteratee.Length == 0)
					_isEmpty = true;

				_currentIterateeIndex = 0;
				foundState = State.ForEach;
				return true;
			}
			
			if (contents.StartsWith("FOREVERY"))
			{
				if ((_state & State.ForEach) != 0 ||
				    (_state & State.ForEvery) != 0)
				{
					throw new Exception(
						"A loop is already in execution and in this version nested foreach loops are not allowed");
				}

				_state |= State.ForEvery;

				var iterateeName = contents.TrimStart("FOREVERY ".ToCharArray());

				if (!_replaceLookup.ContainsKey(iterateeName))
				{
					throw new Exception("No variable with the name " + iterateeName + " could be located");
				}

				_iteratee = (object[]) _replaceLookup[iterateeName];

				if (_iteratee.Length == 0 || ((object[]) _iteratee[0]).Length == 0)
					_isEmpty = true;

				_currentIterateeIndex = 0;
				foundState = State.ForEvery;
				return true;
			}

			return false;
		}

		private string ParseLine(string contents)
		{
			if (contents.StartsWith("[") && contents.EndsWith("]"))
			{
				switch (contents)
				{
					case "[i]" when _iteratee != null:
						return FormatReturn(_iteratee[_currentIterateeIndex]);
					case "[idx]" when _iteratee != null:
						return FormatReturn(_currentIterateeIndex);
				}

				var idxStr = contents.TrimStart('[').TrimEnd(']');
				if (int.TryParse(idxStr, out var idx))
					return FormatReturn(((object[]) _iteratee[_currentIterateeIndex])[idx]);
				throw new Exception("The index " + idxStr + " is not an integer");
			}
			
			if (contents == "ELSEIF")
			{
				return _currentIterateeIndex == 0 ? "if" : "else if";
			}

			return _replaceLookup.ContainsKey(contents) ? FormatReturn(_replaceLookup[contents]) : string.Empty;
		}

		private string FormatReturn(object data)
		{
			switch (data)
			{
				case bool b :
					return b.ToString().ToLower();
				case float fl:
					return fl.ToString(CultureInfo.InvariantCulture) + "f";
			}
			
			return data.ToString();
		}
	}
}
