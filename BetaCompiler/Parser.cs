using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Lexer
{

    public class Parser
    {
        private List<string> input;
        private int estadoAtual = 0;
        private Stack<Tuple<string, int>> stack;
        private readonly Dictionary<Tuple<int, string>, int> transitions;
        private readonly Dictionary<Tuple<int, string>, Tuple<string, int>> reduces;
        private string line = "";

        private Dictionary<string, List<string>> follow;

        public Parser(List<string> input)
        {
            this.input = input;
            transitions = new Dictionary<Tuple<int, string>, int>();
            reduces = new Dictionary<Tuple<int, string>, Tuple<string, int>>();
            follow = new Dictionary<string, List<string>>();
            AddTransitions();
            AddFollow();
            AddReduces();
            stack = new Stack<Tuple<string, int>>();
            stack.Push(new Tuple<string, int>("", 0));
        }

        private void AddTransitions()
        {
            transitions.Add(new Tuple<int, string>(0, "<program>"), 1);
            transitions.Add(new Tuple<int, string>(0, "<declaration_zone_statement>"), 2);
            transitions.Add(new Tuple<int, string>(0, "<declaration_zone_token>"), 3);

            transitions.Add(new Tuple<int, string>(2, "<code_zone_statement"), 4);
            transitions.Add(new Tuple<int, string>(2, "<code_zone_token>"), 5);

            transitions.Add(new Tuple<int, string>(3, "<open_brace>"), 6);

            transitions.Add(new Tuple<int, string>(5, "<open_brace>"), 7);

            transitions.Add(new Tuple<int, string>(6, "<var_declaration>"), 8);
            transitions.Add(new Tuple<int, string>(6, "<assignment>"), 9);
            transitions.Add(new Tuple<int, string>(6, "<type>"), 10);

            transitions.Add(new Tuple<int, string>(7, "<statement>"), 11);
            transitions.Add(new Tuple<int, string>(7, "<statement_type>"), 12);
            transitions.Add(new Tuple<int, string>(7, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(7, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(7, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(7, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(7, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(7, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(7, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(7, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(7, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(7, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(7, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(7, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(7, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(7, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(7, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(7, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(7, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(7, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(8, "<close_brace>"), 31);
            transitions.Add(new Tuple<int, string>(8, "<assignment>"), 32);
            transitions.Add(new Tuple<int, string>(8, "<type>"), 33);

            transitions.Add(new Tuple<int, string>(10, "<identifier>"), 34);

            transitions.Add(new Tuple<int, string>(11, "<close_brace>"), 35);
            transitions.Add(new Tuple<int, string>(11, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(11, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(11, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(11, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(11, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(11, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(11, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(11, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(11, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(11, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(11, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(11, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(11, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(11, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(11, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(11, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(11, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(11, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(11, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(22, "<open_parenthesis>"), 37);

            transitions.Add(new Tuple<int, string>(23, "<operand>"), 38);
            transitions.Add(new Tuple<int, string>(23, "<identifier>"), 39);
            transitions.Add(new Tuple<int, string>(23, "<number>"), 40);

            transitions.Add(new Tuple<int, string>(24, "<atribution_operator>"), 41);

            transitions.Add(new Tuple<int, string>(27, "<open_parenthesis>"), 42);

            transitions.Add(new Tuple<int, string>(28, "<open_parenthesis>"), 43);

            transitions.Add(new Tuple<int, string>(29, "<open_parenthesis>"), 44);

            transitions.Add(new Tuple<int, string>(30, "<open_parenthesis>"), 45);

            transitions.Add(new Tuple<int, string>(33, "<identifier>"), 46);

            transitions.Add(new Tuple<int, string>(34, "<atribution_operator>"), 47);

            transitions.Add(new Tuple<int, string>(37, "<logic_expression>"), 48);
            transitions.Add(new Tuple<int, string>(37, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(37, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(37, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(37, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(37, "<arithmetic_expression>"), 53);
            transitions.Add(new Tuple<int, string>(37, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(37, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(37, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(37, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(37, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(37, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(37, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(38, "<open_brace>"), 61);

            transitions.Add(new Tuple<int, string>(41, "<exp>"), 62);
            transitions.Add(new Tuple<int, string>(41, "<arithmetic_expression>"), 63);
            transitions.Add(new Tuple<int, string>(41, "<logic_expression>"), 64);
            transitions.Add(new Tuple<int, string>(41, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(41, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(41, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(41, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(41, "<arithmetic_expression>"), 53);
            transitions.Add(new Tuple<int, string>(41, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(41, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(41, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(41, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(41, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(41, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(41, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(42, "<atr>"), 65);
            transitions.Add(new Tuple<int, string>(42, "<identifier>"), 66);

            transitions.Add(new Tuple<int, string>(43, "<logic_expression>"), 67);
            transitions.Add(new Tuple<int, string>(43, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(43, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(43, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(43, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(43, "<arithmetic_expression>"), 53);
            transitions.Add(new Tuple<int, string>(43, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(43, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(43, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(43, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(43, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(43, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(43, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(44, "<identifier>"), 68);

            transitions.Add(new Tuple<int, string>(45, "<write_operator>"), 69);
            transitions.Add(new Tuple<int, string>(45, "<string>"), 70);
            transitions.Add(new Tuple<int, string>(45, "<identifier>"), 71);

            transitions.Add(new Tuple<int, string>(46, "<atribution_operator>"), 72);

            transitions.Add(new Tuple<int, string>(47, "<something>"), 73);
            transitions.Add(new Tuple<int, string>(47, "<number>"), 74);
            transitions.Add(new Tuple<int, string>(47, "<string>"), 75);
            transitions.Add(new Tuple<int, string>(47, "<identifier>"), 76);
            transitions.Add(new Tuple<int, string>(47, "<logic_condition>"), 77);

            transitions.Add(new Tuple<int, string>(48, "<close_parenthesis>"), 78);

            transitions.Add(new Tuple<int, string>(49, "<logic_operator>"), 79);

            transitions.Add(new Tuple<int, string>(53, "<relational_operator>"), 80);

            transitions.Add(new Tuple<int, string>(54, "<logic_condition>"), 81);
            transitions.Add(new Tuple<int, string>(54, "<identifier>"), 82);

            transitions.Add(new Tuple<int, string>(55, "<sum_operator>"), 83);

            transitions.Add(new Tuple<int, string>(56, "<multiplicative_operator>"), 84);

            transitions.Add(new Tuple<int, string>(57, "<sum_expression>"), 85);
            transitions.Add(new Tuple<int, string>(57, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(57, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(57, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(57, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(57, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(61, "<case_block>"), 86);
            transitions.Add(new Tuple<int, string>(61, "<case_statement>"), 87);
            transitions.Add(new Tuple<int, string>(61, "<case>"), 88);

            transitions.Add(new Tuple<int, string>(62, "<end_of_statement>"), 89);

            transitions.Add(new Tuple<int, string>(65, "<logic_expression>"), 90);
            transitions.Add(new Tuple<int, string>(43, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(43, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(43, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(43, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(43, "<arithmetic_expression>"), 53);
            transitions.Add(new Tuple<int, string>(43, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(43, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(43, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(43, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(43, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(43, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(43, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(66, "<atribution_operator>"), 91);

            transitions.Add(new Tuple<int, string>(67, "<close_parenthesis>"), 92);

            transitions.Add(new Tuple<int, string>(68, "<close_parenthesis>"), 93);

            transitions.Add(new Tuple<int, string>(69, "<close_parenthesis>"), 94);

            transitions.Add(new Tuple<int, string>(72, "<something>"), 95);
            transitions.Add(new Tuple<int, string>(72, "<number>"), 74);
            transitions.Add(new Tuple<int, string>(72, "<string>"), 75);
            transitions.Add(new Tuple<int, string>(72, "<identifier>"), 76);
            transitions.Add(new Tuple<int, string>(72, "<logic_condition>"), 77);

            transitions.Add(new Tuple<int, string>(73, "<end_of_statement>"), 96);

            transitions.Add(new Tuple<int, string>(78, "<open_brace>"), 97);

            transitions.Add(new Tuple<int, string>(79, "<logic_expression>"), 98);
            transitions.Add(new Tuple<int, string>(79, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(79, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(79, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(79, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(79, "<arithmetic_expression>"), 53);
            transitions.Add(new Tuple<int, string>(79, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(79, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(79, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(79, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(79, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(79, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(79, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(80, "<arithmetic_expression>"), 99);
            transitions.Add(new Tuple<int, string>(80, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(80, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(80, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(80, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(80, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(80, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(83, "<multiplicative_expression>"), 100);
            transitions.Add(new Tuple<int, string>(83, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(83, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(83, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(83, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(84, "<operand>"), 101);
            transitions.Add(new Tuple<int, string>(84, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(84, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(85, "<close_parenthesis>"), 102);

            transitions.Add(new Tuple<int, string>(86, "<default_statement>"), 103);
            transitions.Add(new Tuple<int, string>(86, "<case_statement>"), 104);
            transitions.Add(new Tuple<int, string>(86, "<default>"), 105);
            transitions.Add(new Tuple<int, string>(86, "<case>"), 88);

            transitions.Add(new Tuple<int, string>(88, "<number>"), 106);

            transitions.Add(new Tuple<int, string>(90, "<end_of_statement>"), 107);

            transitions.Add(new Tuple<int, string>(91, "<exp>"), 108);
            transitions.Add(new Tuple<int, string>(91, "<arithmetic_expression>"), 109);
            transitions.Add(new Tuple<int, string>(91, "<logic_expression>"), 64);
            transitions.Add(new Tuple<int, string>(91, "<logic_operand>"), 49);
            transitions.Add(new Tuple<int, string>(91, "<logic_condition>"), 50);
            transitions.Add(new Tuple<int, string>(91, "<relational_expression>"), 51);
            transitions.Add(new Tuple<int, string>(91, "<not_expression>"), 52);
            transitions.Add(new Tuple<int, string>(91, "<not_operator>"), 54);
            transitions.Add(new Tuple<int, string>(91, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(91, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(91, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(91, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(91, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(91, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(92, "<open_brace>"), 110);

            transitions.Add(new Tuple<int, string>(93, "<end_of_statement>"), 111);

            transitions.Add(new Tuple<int, string>(94, "<end_of_statement>"), 112);

            transitions.Add(new Tuple<int, string>(95, "<end_of_statement>"), 113);

            transitions.Add(new Tuple<int, string>(97, "<statement>"), 114);
            transitions.Add(new Tuple<int, string>(97, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(97, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(97, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(97, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(97, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(97, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(97, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(97, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(97, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(97, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(97, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(97, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(97, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(97, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(97, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(97, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(97, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(97, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(97, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(102, "<multiplicative_operator>"), 115);

            transitions.Add(new Tuple<int, string>(103, "<close_brace>"), 116);

            transitions.Add(new Tuple<int, string>(105, "<open_brace>"), 117);

            transitions.Add(new Tuple<int, string>(106, "<open_brace>"), 118);

            transitions.Add(new Tuple<int, string>(107, "<itr>"), 119);
            transitions.Add(new Tuple<int, string>(107, "<identifier>"), 120);

            transitions.Add(new Tuple<int, string>(108, "<end_of_statement>"), 121);

            transitions.Add(new Tuple<int, string>(109, "<relational_operator>"), 122);

            transitions.Add(new Tuple<int, string>(110, "<statement>"), 123);
            transitions.Add(new Tuple<int, string>(110, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(110, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(110, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(110, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(110, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(110, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(110, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(110, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(110, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(110, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(110, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(110, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(110, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(110, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(110, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(110, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(110, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(110, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(110, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(114, "<close_brace>"), 125);

            transitions.Add(new Tuple<int, string>(115, "<multiplicative_expression>"), 126);
            transitions.Add(new Tuple<int, string>(115, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(115, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(115, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(115, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(117, "<statement>"), 127);
            transitions.Add(new Tuple<int, string>(117, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(117, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(117, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(117, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(117, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(117, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(117, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(117, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(117, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(117, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(117, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(117, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(117, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(117, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(117, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(117, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(117, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(117, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(117, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(118, "<statement>"), 128);
            transitions.Add(new Tuple<int, string>(118, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(118, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(118, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(118, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(118, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(118, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(118, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(118, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(118, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(118, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(118, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(118, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(118, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(118, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(118, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(118, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(118, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(118, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(118, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(119, "<close_parenthesis>"), 129);

            transitions.Add(new Tuple<int, string>(120, "<iteration_operator>"), 130);

            transitions.Add(new Tuple<int, string>(122, "<arithmetic_expression>"), 131);
            transitions.Add(new Tuple<int, string>(122, "<sum_expression>"), 55);
            transitions.Add(new Tuple<int, string>(122, "<multiplicative_expression>"), 56);
            transitions.Add(new Tuple<int, string>(122, "<open_parenthesis>"), 57);
            transitions.Add(new Tuple<int, string>(122, "<operand>"), 58);
            transitions.Add(new Tuple<int, string>(122, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(122, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(123, "<close_brace>"), 132);

            transitions.Add(new Tuple<int, string>(125, "<else_statement>"), 133);
            transitions.Add(new Tuple<int, string>(125, "<else>"), 134);

            transitions.Add(new Tuple<int, string>(126, "<multiplicative_operator>"), 135);

            transitions.Add(new Tuple<int, string>(127, "<break>"), 136);

            transitions.Add(new Tuple<int, string>(128, "<break>"), 137);

            transitions.Add(new Tuple<int, string>(129, "<open_brace>"), 138);

            transitions.Add(new Tuple<int, string>(130, "<number>"), 139);

            transitions.Add(new Tuple<int, string>(134, "<open_brace>"), 140);
            transitions.Add(new Tuple<int, string>(134, "<if_statement>"), 141);
            transitions.Add(new Tuple<int, string>(134, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(134, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(134, "<if>"), 22);

            transitions.Add(new Tuple<int, string>(135, "<operand>"), 142);
            transitions.Add(new Tuple<int, string>(135, "<identifier>"), 59);
            transitions.Add(new Tuple<int, string>(135, "<number>"), 60);

            transitions.Add(new Tuple<int, string>(136, "<operand>"), 143);

            transitions.Add(new Tuple<int, string>(137, "<end_of_statement>"), 144);

            transitions.Add(new Tuple<int, string>(138, "<statement>"), 145);
            transitions.Add(new Tuple<int, string>(138, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(138, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(138, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(138, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(138, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(138, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(138, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(138, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(138, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(138, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(138, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(138, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(138, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(138, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(138, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(138, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(138, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(138, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(138, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(140, "<statement>"), 146);
            transitions.Add(new Tuple<int, string>(140, "<statement_type>"), 36);
            transitions.Add(new Tuple<int, string>(140, "<conditional>"), 13);
            transitions.Add(new Tuple<int, string>(140, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(140, "<rept>"), 15);
            transitions.Add(new Tuple<int, string>(140, "<read_statement>"), 16);
            transitions.Add(new Tuple<int, string>(140, "<write_statement>"), 17);
            transitions.Add(new Tuple<int, string>(140, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(140, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(140, "<if_conditional>"), 20);
            transitions.Add(new Tuple<int, string>(140, "<if_or_else>"), 21);
            transitions.Add(new Tuple<int, string>(140, "<if>"), 22);
            transitions.Add(new Tuple<int, string>(140, "<switch>"), 23);
            transitions.Add(new Tuple<int, string>(140, "<identifier>"), 24);
            transitions.Add(new Tuple<int, string>(140, "<for_statement>"), 25);
            transitions.Add(new Tuple<int, string>(140, "<while_statement>"), 26);
            transitions.Add(new Tuple<int, string>(140, "<for>"), 27);
            transitions.Add(new Tuple<int, string>(140, "<while>"), 28);
            transitions.Add(new Tuple<int, string>(140, "<read>"), 29);
            transitions.Add(new Tuple<int, string>(140, "<write>"), 30);

            transitions.Add(new Tuple<int, string>(144, "<close_brace>"), 147);

            transitions.Add(new Tuple<int, string>(145, "<close_brace>"), 148);

            transitions.Add(new Tuple<int, string>(146, "<close_brace>"), 149);
        }

        private void AddFollow()
        {
            var l = new List<string> {"$"};
            follow.Add("<program>", l);

            l = new List<string> {"<code_zone_token>"};
            follow.Add("<declaration_zone_statement>", l);

            l = new List<string> {"< type >", "<close_brace>"};
            follow.Add("<var_declaration>", l);

            l = new List<string> {"<close_brace>"};
            follow.Add("<assignment>", l);

            l = new List<string> {"<end_of_statement>"};
            follow.Add("<something>", l);

            l = new List<string> {"$"};
            follow.Add("<code_zone_statement>", l);

            l = new List<string> 
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<statement_type>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<atr>", l);

            l = new List<string> {"<end_of_statement>"};
            follow.Add("<exp>", l);

            l = new List<string> {"<close_parenthesis>"};
            follow.Add("<itr>", l);

            l = new List<string> {"<logic_operator>", "<close_parenthesis>", "<end_of_statement>"};
            follow.Add("<relational_expression>", l);

            l = new List<string> {"<logic_operator>", "<close_parenthesis>", "<end_of_statement>"};
            follow.Add("<not_expression>", l);

            l = new List<string> {"<logic_operator>", "<close_parenthesis>", "<end_of_statement>"};
            follow.Add("<logic_operand>", l);

            l = new List<string> {"<close_parenthesis>", "<end_of_statement>"};
            follow.Add("<logic_expression>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<conditional>", l);

            l = new List<string> {"<if>", "<switch>", "<identifier>", "<for>", "<while>", "<read>", "<write>"};
            follow.Add("<switch_statement>", l);

            l = new List<string> { "<default_statement>", "<case_statement>" };
            follow.Add("<case_block>", l);

            l = new List<string> {"<default_statement>", "<case_statement>"};
            follow.Add("<case_statement>", l);

            l = new List<string> {"<close_brace>"};
            follow.Add("<default_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<if_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<if_conditional>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<if_or_else>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<else_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<rept>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<for_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<while_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<read_statement>", l);

            l = new List<string>
            {
                "<if>",
                "<switch>",
                "<identifier>",
                "<for>",
                "<while>",
                "<read>",
                "<write>",
                "<close_brace>"
            };
            follow.Add("<write_statement>", l);

            l = new List<string>
            {
                "<logic_operator>",
                "<close_parenthesis>",
                "<end_of_statement>"
            };
            follow.Add("<arithmetic_expression>", l);

            l = new List<string>
            {
                "<multiplicative_operator>",
                "<sum_operator>",
                "<close_parenthesis>",
                "<open_brace>"
            };
            follow.Add("<operand>", l);

            l = new List<string>
            {
                "<sum_operator>",
                "<close_parenthesis>"
            };
            follow.Add("<sum_expression>", l);

            l = new List<string>
            {
                "<sum_operator>",
                "<multiplicative_operator",
                "<close_parenthesis>"
            };
            follow.Add("<multiplicative_expression>", l);

            l = new List<string>{"<close_parenthesis>"};
            follow.Add("<write_operator>", l);
        }

        private void AddReduces()
        {
            reduces.Add(new Tuple<int, string>(1, "<program>"), new Tuple<string, int>("<program'>", 1));

            reduces.Add(new Tuple<int, string>(9, "<assignment>"), new Tuple<string, int>("<var_declaration>", 1));

            reduces.Add(new Tuple<int, string>(4, "<code_zone_statement>"), new Tuple<string, int>("<program>", 2));

            reduces.Add(new Tuple<int, string>(11, "<close_brace>"), new Tuple<string, int>("<code_zone_statement>", 3));

            reduces.Add(new Tuple<int, string>(13, "<statement_type>"), new Tuple<string, int>("<statement>", 1));

            reduces.Add(new Tuple<int, string>(14, "<atr>"), new Tuple<string, int>("<statement_type>", 1));

            reduces.Add(new Tuple<int, string>(15, "<conditional>"), new Tuple<string, int>("<statement_type>", 1));

            reduces.Add(new Tuple<int, string>(16, "<rept>"), new Tuple<string, int>("<statement_type>", 1));

            reduces.Add(new Tuple<int, string>(18, "<if_statement>"), new Tuple<string, int>("<conditional>", 1));

            reduces.Add(new Tuple<int, string>(19, "<switch_statement>"), new Tuple<string, int>("<conditional>", 1));

            reduces.Add(new Tuple<int, string>(21, "<if_conditional>"), new Tuple<string, int>("<if_statement>", 1));

            reduces.Add(new Tuple<int, string>(22, "<if_or_else>"), new Tuple<string, int>("<if_statement>", 1));

            reduces.Add(new Tuple<int, string>(24, "<for_statement>"), new Tuple<string, int>("<rept>", 1));

            reduces.Add(new Tuple<int, string>(25, "<while_statement>"), new Tuple<string, int>("<rept>", 1));

            reduces.Add(new Tuple<int, string>(28, "<close_brace>"),
                new Tuple<string, int>("<declaration_zone_statement>", 4));
            reduces.Add(new Tuple<int, string>(29, "<var_declaration>"), new Tuple<string, int>("<var_declaration>", 2));
            reduces.Add(new Tuple<int, string>(31, "<close_brace>"), new Tuple<string, int>("<code_zone_statement>", 4));
            reduces.Add(new Tuple<int, string>(32, "<statement>"), new Tuple<string, int>("<statement>", 2));
            reduces.Add(new Tuple<int, string>(44, "<logic_operand>"), new Tuple<string, int>("<logic_expression>", 1));
            reduces.Add(new Tuple<int, string>(45, "<logic_condition>"), new Tuple<string, int>("<logic_operand>", 1));
            reduces.Add(new Tuple<int, string>(46, "<relational_expression>"),
                new Tuple<string, int>("<logic_operand>", 1));
            reduces.Add(new Tuple<int, string>(47, "<not_expression>"), new Tuple<string, int>("<logic_operand>", 1));
            reduces.Add(new Tuple<int, string>(50, "<sum_expression>"),
                new Tuple<string, int>("<arithmetic_expression>", 1));
            reduces.Add(new Tuple<int, string>(51, "<multiplicative_expression>"),
                new Tuple<string, int>("<sum_expression>", 1));
            reduces.Add(new Tuple<int, string>(52, "<operand>"),
                new Tuple<string, int>("<multiplicative_expression>", 1));
            reduces.Add(new Tuple<int, string>(54, "<identifier>"), new Tuple<string, int>("<operand>", 1));

            reduces.Add(new Tuple<int, string>(55, "<number>"), new Tuple<string, int>("<operand>", 1));

            reduces.Add(new Tuple<int, string>(60, "<number>"), new Tuple<string, int>("<something>", 1));
            reduces.Add(new Tuple<int, string>(61, "<string>"), new Tuple<string, int>("<something>", 1));
            reduces.Add(new Tuple<int, string>(62, "<identifier>"), new Tuple<string, int>("<something>", 1));
            reduces.Add(new Tuple<int, string>(63, "<end_of_statement>"), new Tuple<string, int>("<atr>", 4));
            reduces.Add(new Tuple<int, string>(64, "<close_brace>"), new Tuple<string, int>("<switch_statement>", 4));
            reduces.Add(new Tuple<int, string>(71, "<logic_condition>"), new Tuple<string, int>("<not_expression>", 2));
            reduces.Add(new Tuple<int, string>(72, "<identifier>"), new Tuple<string, int>("<not_expression>", 2));
            reduces.Add(new Tuple<int, string>(79, "<end_of_statement>"), new Tuple<string, int>("<assignment>", 5));
            reduces.Add(new Tuple<int, string>(80, "<close_brace>"), new Tuple<string, int>("<switch_statement>", 5));
            reduces.Add(new Tuple<int, string>(82, "<close_brace>"), new Tuple<string, int>("<switch_statement>", 5));
            reduces.Add(new Tuple<int, string>(83, "<close_brace>"), new Tuple<string, int>("<switch_statement>", 4));
            reduces.Add(new Tuple<int, string>(90, "<end_of_statement>"),
                new Tuple<string, int>("<default_statement>", 2));
            reduces.Add(new Tuple<int, string>(92, "<arithmetic_expression>"),
                new Tuple<string, int>("<relational_expression>", 3));
            reduces.Add(new Tuple<int, string>(93, "<sum_expression>"), new Tuple<string, int>("<sum_expression>", 3));
            reduces.Add(new Tuple<int, string>(94, "<multiplicative_expression>"),
                new Tuple<string, int>("<multiplicative_expression>", 3));
            reduces.Add(new Tuple<int, string>(95, "<close_parenthesis>"),
                new Tuple<string, int>("<multiplicative_expression>", 3));
            reduces.Add(new Tuple<int, string>(98, "<arithmetic_expression>"), new Tuple<string, int>("<exp>", 1));
            reduces.Add(new Tuple<int, string>(99, "<logic_expression>"), new Tuple<string, int>("<exp>", 1));
            reduces.Add(new Tuple<int, string>(101, "<switch_statement>"), new Tuple<string, int>("<close_brace>", 6));
            reduces.Add(new Tuple<int, string>(102, "<close_brace>"), new Tuple<string, int>("<switch_statement>", 5));
            reduces.Add(new Tuple<int, string>(104, "<close_brace>"), new Tuple<string, int>("<swich_statement>", 5));
            reduces.Add(new Tuple<int, string>(105, "<number>"), new Tuple<string, int>("<case_statement>", 2));
            reduces.Add(new Tuple<int, string>(106, "<statement>"), new Tuple<string, int>("<case_statement>", 3));
            reduces.Add(new Tuple<int, string>(109, "<end_of_statement>"),
                new Tuple<string, int>("<default_statement>", 3));
            reduces.Add(new Tuple<int, string>(110, "<end_of_statement>"),
                new Tuple<string, int>("<default_statement>", 3));
            reduces.Add(new Tuple<int, string>(114, "<end_of_statement>"), new Tuple<string, int>("<atr>", 4));
            reduces.Add(new Tuple<int, string>(116, "<close_brace>"), new Tuple<string, int>("<while_statement>", 6));
            reduces.Add(new Tuple<int, string>(117, "<statement>"), new Tuple<string, int>("<case_statement>", 3));
            reduces.Add(new Tuple<int, string>(121, "<end_of_statement>"), new Tuple<string, int>("<case_statement>", 3));
            reduces.Add(new Tuple<int, string>(122, "<end_of_statement>"),
                new Tuple<string, int>("<default_statement>", 4));
            reduces.Add(new Tuple<int, string>(123, "<close_brace>"), new Tuple<string, int>("<if_conditional>", 7));
            reduces.Add(new Tuple<int, string>(124, "<multiplicative_expression>"),
                new Tuple<string, int>("<multiplicative_expression>", 5));
            reduces.Add(new Tuple<int, string>(127, "<close_brace>"), new Tuple<string, int>("<while_statement>", 7));
            reduces.Add(new Tuple<int, string>(129, "<end_of_statement>"), new Tuple<string, int>("<case_statement>", 4));
            reduces.Add(new Tuple<int, string>(130, "<end_of_statement>"), new Tuple<string, int>("<case_statement>", 5));
            reduces.Add(new Tuple<int, string>(131, "<else_statement>"), new Tuple<string, int>("<if_or_else>", 8));
            reduces.Add(new Tuple<int, string>(135, "<end_of_statement>"), new Tuple<string, int>("<case_statement>", 5));
            reduces.Add(new Tuple<int, string>(137, "<if_statement>"), new Tuple<string, int>("<else_statement>", 2));
            reduces.Add(new Tuple<int, string>(139, "<number>"), new Tuple<string, int>("<itr>", 3));
            reduces.Add(new Tuple<int, string>(142, "<close_brace>"), new Tuple<string, int>("<for_statement>", 4));
            reduces.Add(new Tuple<int, string>(143, "<close_brace>"), new Tuple<string, int>("<else_statement>", 4));
            reduces.Add(new Tuple<int, string>(144, "<close_brace>"), new Tuple<string, int>("<for_statement>", 11));
            reduces.Add(new Tuple<int, string>(145, "<read_statement>"), new Tuple<string, int>("<statement_type>", 1));
            reduces.Add(new Tuple<int, string>(146, "<write_statement>"), new Tuple<string, int>("<statement_type>", 1));
            reduces.Add(new Tuple<int, string>(157, "<end_of_statement>"), new Tuple<string, int>("<read_statement>", 5));
            reduces.Add(new Tuple<int, string>(158, "<end_of_statement>"),
                new Tuple<string, int>("<write_statement>", 5));
            reduces.Add(new Tuple<int, string>(159, "<end_of_statement>"),
                new Tuple<string, int>("<write_statement>", 5));
            reduces.Add(new Tuple<int, string>(164, "<end_of_statement>"), new Tuple<string, int>("<assignment>", 5));
            reduces.Add(new Tuple<int, string>(166, "<end_of_statement>"), new Tuple<string, int>("<atr>", 4));
        }

        public void Parse()
        {
            Console.WriteLine(input.Count);

            while (input.Count != 0)
            {
                try
                {
                    if (input[0].StartsWith("<line")) {
                        line = input[0];
                        input.RemoveAt(0);
                        continue;
                    }

                    Console.WriteLine("Stack.Peek = " + stack.Peek());
                    Console.WriteLine("Input = " + input[0]);
                    var transicao = new Tuple<int, string>(estadoAtual, input[0]);
                    var reduceTransition = new Tuple<int, string>(estadoAtual, stack.Peek().Item1);
                    if ("$".Equals(input[0]) && "<program>".Equals(stack.Peek().Item1)) {
                        Console.WriteLine("Parabens, voce nao eh gordo");
                        return;
                    }

                    if (reduces.ContainsKey(reduceTransition)) {
                        Console.WriteLine($"Estado Atual = {estadoAtual}");
                        Console.WriteLine("Follow??");
                        Console.WriteLine("Follow {0} = ", reduces[reduceTransition].Item1);
                        foreach (var item in follow[reduces[reduceTransition].Item1]) {
                            Console.Write(item + ",");
                        }
                        Console.WriteLine("Follow Contains?? " + follow[reduces[reduceTransition].Item1].Contains(input[0]));
                        if (follow[reduces[reduceTransition].Item1].Contains(input[0])) {
                            Console.WriteLine("Reducing {0} on state {1}", stack.Peek(), estadoAtual);
                            var qnt = reduces[reduceTransition].Item2;
                            for (var i = 0; i < qnt; i++) {
                                stack.Pop();
                            }
                            var newTuple = new Tuple<int, string>(stack.Peek().Item2, reduces[reduceTransition].Item1);
                            var empilhar = new Tuple<string, int>(newTuple.Item2, transitions[newTuple]);
                            stack.Push(empilhar);
                            estadoAtual = transitions[newTuple];
                            Console.WriteLine("Stack.Peek after reducing = " + stack.Peek());
                            //estadoAtual = 
                        }
                    }
                    if (transitions.ContainsKey(transicao))
                    {
                        Console.WriteLine($"Shifting {input[0]} on state {estadoAtual}");
                        var empilhar = new Tuple<string, int>(input[0], transitions[transicao]);
                        stack.Push(empilhar);
                        input.RemoveAt(0);
                        estadoAtual = transitions[transicao];
                    }
                   

                } catch (Exception)
                {
                    Console.WriteLine($"FODEU NA LINHA {line}");
                    return;
                }
          
            }
            Console.WriteLine("Oh, fuck");
            
        }

}
}