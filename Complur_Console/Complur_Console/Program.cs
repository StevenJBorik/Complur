using System;
using System.Collections.Generic;
using System.Linq;

//29:09
namespace Complur_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                System.Console.WriteLine("> ");
                var line = Console.ReadLine(); 
                if (string.IsNullOrWhiteSpace(line))
                    return;
                
                var lexer = new Lexer(line);
                while(true)
                {
                    var token = lexer.NextToken(); 
                    if (token.Kind == SyntaxKind.EndofFileToken) {
                        break; 
                    }
                    System.Console.WriteLine($"{token.Kind}: '{token.Text}'");
                    if (token.Value != null) {
                        System.Console.WriteLine($"{token.Value}");
                    }
                    System.Console.WriteLine();
                }
            }
        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        MultiplyToken,
        DivideToken,
        OpenParenToken,
        ClosedParenToken,
        BadToken,
        EndofFileToken
    }
    class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;

            Position = position;

            Text = text; 
            
            Value = value; 
        }

        public SyntaxKind Kind {get; }
        public int Position {get; }
        public string Text {get; }
        public object Value { get; }
    }
    class Lexer 
    {
        private readonly string _text;
        private int _position;
        public Lexer(string text)
        {
            _text = text; 
        }
        // Current - get current character 
        private char Current
        {
            get {
                if (_position >= _text.Length)
                    return '\0'; 
                return _text[_position]; 
            }
        }

        private void Next()
        {
            _position++; 
        }

        public SyntaxToken NextToken()
        {   

            // EOF
            if (_position >= _text.Length) {
                return new SyntaxToken(SyntaxKind.EndofFileToken, _position, "\0", null); 
            }
            // <numbers>
            if (char.IsDigit(Current))
            {
                var start = _position; 
                while (char.IsDigit(Current))
                    Next(); 
                var length = _position - start; 
                var text = _text.Substring(start, length); 
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value); 
            }
            // + - * /  ( )
            // WhiteSpace
            if (char.IsWhiteSpace(Current)) 
            {
                var start = _position; 
                while (char.IsWhiteSpace(Current))
                    Next(); 
                var length = _position - start; 
                var text = _text.Substring(start, length); 
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, value); 
            }

            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            else if (Current =='-')
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            else if (Current =='*')
                return new SyntaxToken(SyntaxKind.MultiplyToken, _position++, "*", null); 
            else if (Current =='/')
                 return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null); 
            else if (Current =='(')
                return new SyntaxToken(SyntaxKind.OpenParenToken, _position++, "(", null); 
            else if (Current ==')')
                return new SyntaxToken(SyntaxKind.ClosedParenToken, _position++, ")", null); 

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position -1, -1), null); 
        }
    }
}
