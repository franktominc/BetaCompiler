using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Lexer
{
    public class Lexer
    {
        private string FilePath { get; set; }

        public Lexer(string filePath)
        {
            FilePath = filePath;
        }

        public static string IsPontuation(string s, int startPosition, int endPosition)
        {
            var check = "";
            if (Math.Max(startPosition, endPosition) <= s.Length)
                check = s.Substring(startPosition, endPosition - startPosition);
            switch (check)
            {
                case "{":
                    return "<open_brace>";
                case "}":
                    return "<close_brace>";
                case "(":
                    return "<open_parenthesis>";
                case ")":
                    return "<close_parenthesis>";
                case ";":
                    return "<end_of_statement>";
            }
            return ("<Erro Detectado>");

        }

        public static string IsOperator(string s, int startPosition, int endPosition)
        {
            string check = "";
            if (Math.Max(startPosition, endPosition) <= s.Length)
            {
                check = s.Substring(startPosition, endPosition - startPosition);
            }
            switch (check)
            {
                case "+":
                    return "<sum_operator>";
                case "+=":
                    return "<iteration_operator>";
                case "-":
                    return "<sum_operator>";
                case "*":
                    return "<multiplicative_operator>";
                case "/":
                    return "<multiplicative_operator>";
                case "%":
                    return "<multiplicative_operator>";
                case "!":
                    return "<not_operator>";
                case "&&":
                    return "<logic_operator>";
                case "||":
                    return "<logic_operator>";
                case ">":
                    return "<relational_operator>";
                case "<":
                    return "<relational_operator>";
                case ">=":
                    return "<relational_operator>";
                case "<=":
                    return "<relational_operator>";
                case "!=":
                    return "<relational_operator>";
                case "==":
                    return "<relational_operator>";
                case "=":
                    return "<attribution_operator>";
            }

            return "<Erro Detectado>";
        }

        public static string IsReserved(string s, int startPosition, int endPosition)
        {
            string check = "";
            if (Math.Max(startPosition, endPosition) <= s.Length)
                check = s.Substring(startPosition, endPosition - startPosition);
            switch (check)
            {
                case "DeclarationZone":
                    return "<declaration_zone_token>";
                case "if":
                    return "<if>";
                case "else":
                    return "<else>";
                case "for":
                    return "<for>";
                case "while":
                    return "<while>";
                case "case":
                    return "<case>";
                case "switch":
                    return "<switch>";
                case "default":
                    return "<default>";
                case "break":
                    return "<break>";
                case "int":
                    return "<type>";
                case "float":
                    return "<type>";
                case "char":
                    return "<type>";
                case "bool":
                    return "<type>";
                case "true":
                    return "<logic_condition>";
                case "false":
                    return "<logic_condition>";
                case "CodeZone":
                    return "<code_zone_token>";
                case "read":
                    return "<read>";
                case "write":
                    return "<write>";

            }
            return ("<identifier>");

        }

        public List<string> Parse()
        {
            var tokens = new List<Tuple<string, string>>();
            var y = 0;
            using (StreamReader sr = File.OpenText(FilePath))
            {
                var s = "";
                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    var startPosition = 0;
                    y++;
                    var i = 0;
                    //s = Regex.Replace(s, @"\s+", string.Empty);
                    tokens.Add(new Tuple<string, string>($"<line {y}>", $"<line {y}>"));
                    STATE_0:
                    startPosition = i;
                    if (i == s.Length)
                    {
                        goto FINNISH;
                    }
                    if (char.IsLetter(s[i]))
                    {
                        goto STATE_1;
                    }
                    if (char.IsDigit(s[i]))
                    {
                        goto STATE_14;
                    }
                    if (char.IsWhiteSpace(s[i]))
                    {
                        i++;
                        goto STATE_0;
                    }

                    switch (s[i])
                    {
                        case '=':
                        case '<':
                        case '>':
                        case '!':
                            goto STATE_3;
                        case '+':
                            goto STATE_13;
                        case '-':
                            goto STATE_7;
                        case '/':
                            goto STATE_6;
                        case '*':
                            goto STATE_8;
                        case '&':
                            goto STATE_9;
                        case '|':
                            goto STATE_11;
                        case '"':
                            goto STATE_17;
                        case '(':
                            goto STATE_19;
                        case ')':
                            goto STATE_20;
                        case '{':
                            goto STATE_21;
                        case '}':
                            goto STATE_22;
                        case ';':
                            goto STATE_23;
                        case '%':
                            goto STATE_24;
                        default:
                            Console.WriteLine("Failing Character {0} on state 1", s[i]);
                            goto FAILED;
                    }
                    STATE_1:
                    i++;
                    if (char.IsLetterOrDigit(s[i]))
                    {
                        goto STATE_1;
                    }
                    tokens.Add(new Tuple<string, string>(IsReserved(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_3:
                    i++;
                    if (s[i] == '=')
                        goto STATE_5;
                    goto STATE_4;
                    STATE_4: //Reconhece o token
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));

                    goto STATE_0;
                    STATE_5:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_6:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_7:
                    i++;
                    if (char.IsDigit(s[i]))
                    {
                        goto STATE_14;
                    }
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_8:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_9:
                    i++;
                    if (s[i] == '&')
                        goto STATE_10;
                    Console.WriteLine("Failing Character {0} on state 9", s[i]);
                    goto FAILED;
                    STATE_10:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_11:
                    i++;
                    if (s[i] == '|')
                        goto STATE_12;
                    Console.WriteLine("Failing Character {0} on state 12", s[i]);
                    goto FAILED;
                    STATE_12:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_13:
                    i++;
                    if (char.IsDigit(s[i]))
                        goto STATE_14;
                    if (s[i] == '=')
                        goto STATE_5;
                    goto STATE_4;
                    STATE_14:
                    i++;
                    if (char.IsDigit(s[i]))
                        goto STATE_14;
                    if (s[i] == '.')
                        goto STATE_15;
                    tokens.Add(new Tuple<string, string>("<number>",
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_15:
                    i++;
                    if (char.IsDigit(s[i]))
                    {
                        goto STATE_16;
                    }
                    Console.WriteLine($"Failing Character {s[i]} on state 16");
                    goto FAILED;
                    STATE_16:
                    i++;
                    if (char.IsDigit(s[i]))
                    {
                        goto STATE_16;
                    }
                    tokens.Add(new Tuple<string, string>("<number>",
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_17:
                    i++;
                    if (s[i] == '\\')
                    {
                        i += 2;
                        goto STATE_17;
                    }
                    if (s[i] == '"')
                        goto STATE_18;
                    goto STATE_17;
                    STATE_18:
                    i++;
                    tokens.Add(new Tuple<string, string>("<string>",
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_19:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsPontuation(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_20:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsPontuation(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_21:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsPontuation(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_22:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsPontuation(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_23:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsPontuation(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    STATE_24:
                    i++;
                    tokens.Add(new Tuple<string, string>(IsOperator(s, startPosition, i),
                        s.Substring(startPosition, i - startPosition)));
                    goto STATE_0;
                    FAILED:
                    Console.WriteLine("Caracter inesperado encontrado");
                    throw new Exception("Falha ao tentar resolver alguns simbolos");
                    FINNISH:
                    Console.Write("");
                }
            }
            return tokens.Select(x=>x.Item1).ToArray().ToList();

        }
    }
}