using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator
{
    internal class ClassBuilder
    {
        private string _indent = "";
        private const int _indentSpaces = 4;
        private int _indentLevel = 0;
        private bool _wasLastCallAppendLine = true;
        private bool _isFirstMember = true;
        private readonly StringBuilder _builder;
        public ClassBuilder()
        {
            _builder = new StringBuilder();
        }

        public int IndentLevel => _indentLevel;

        public void IncreaseIndent()
        {
            _indentLevel++;
            _indent += new string(' ', _indentSpaces);
        }

        public bool DecreaseIndent()
        {
            if (_indent.Length >= _indentSpaces)
            {
                _indentLevel--;
                _indent = _indent.Substring(_indentSpaces);
                return true;
            }

            return false;
        }

        public void AppendLineBeforeMember()
        {
            if (!_isFirstMember)
            {
                _builder.AppendLine();
            }

            _isFirstMember = false;
        }

        public void AppendLine(string line)
        {
            if (_wasLastCallAppendLine) // If last call was only Append, you shouldn't add the indent
            {
                _builder.Append(_indent);
            }

            _builder.AppendLine($"{line}");
            _wasLastCallAppendLine = true;
        }

        public void AppendLine()
        {
            _builder.AppendLine();
            _wasLastCallAppendLine = true;
        }

        public void Append(string stringToAppend)
        {
            if (_wasLastCallAppendLine)
            {
                _builder.Append(_indent);
                _wasLastCallAppendLine = false;
            }

            _builder.Append(stringToAppend);
        }

        public override string ToString() => _builder.ToString();
    }
}
