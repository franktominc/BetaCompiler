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

            transitions.Add(new Tuple<int, string>(2, "<code_zone_statement>"), 4);
            transitions.Add(new Tuple<int, string>(2, "<code_zone_token>"), 5);

            transitions.Add(new Tuple<int, string>(3, "<open_brace>"), 6);

            transitions.Add(new Tuple<int, string>(5, "<open_brace>"), 7);

            transitions.Add(new Tuple<int, string>(6, "<var_declaration>"), 8);
            transitions.Add(new Tuple<int, string>(6, "<assignment>"), 9);
            transitions.Add(new Tuple<int, string>(6, "<type>"), 10);

            transitions.Add(new Tuple<int, string>(7, "<close_brace>"), 11);
            transitions.Add(new Tuple<int, string>(7, "<statement>"), 12);
            transitions.Add(new Tuple<int, string>(7, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(7, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(7, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(7, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(7, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(7, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(7, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(7, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(7, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(7, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(7, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(7, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(7, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(7, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(7, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(7, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(7, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(7, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(7, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(8, "<close_brace>"), 28);

            transitions.Add(new Tuple<int, string>(9, "<var_declaration>"), 29);
            transitions.Add(new Tuple<int, string>(9, "<assignment>"), 9);
            transitions.Add(new Tuple<int, string>(9, "<type>"), 160);

            transitions.Add(new Tuple<int, string>(10, "<identifier>"), 30);

            transitions.Add(new Tuple<int, string>(12, "<close_brace>"), 31);

            transitions.Add(new Tuple<int, string>(13, "<statement>"), 32);
            transitions.Add(new Tuple<int, string>(13, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(13, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(13, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(13, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(13, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(13, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(13, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(13, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(13, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(13, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(13, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(13, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(13, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(13, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(13, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(13, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(13, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(13, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(13, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(17, "<attribution_operator>"), 33);

            transitions.Add(new Tuple<int, string>(20, "<number>"), 34);
            transitions.Add(new Tuple<int, string>(20, "<identifier>"), 35);

            transitions.Add(new Tuple<int, string>(23, "<open_parenthesis>"), 36);

            transitions.Add(new Tuple<int, string>(26, "<open_parenthesis>"), 37);

            transitions.Add(new Tuple<int, string>(27, "<open_parenthesis>"), 38);

            transitions.Add(new Tuple<int, string>(30, "<attribution_operator>"), 39);

            transitions.Add(new Tuple<int, string>(33, "<exp>"), 40);
            transitions.Add(new Tuple<int, string>(33, "<arithmetic_expression>"), 98);
            transitions.Add(new Tuple<int, string>(33, "<logic_expression>"), 99);
            transitions.Add(new Tuple<int, string>(33, "<logic_operand>"), 44);
            transitions.Add(new Tuple<int, string>(33, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(33, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(33, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(33, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(33, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(33, "<number>"), 55);
            transitions.Add(new Tuple<int, string>(33, "<logic_condition>"), 45);
            transitions.Add(new Tuple<int, string>(33, "<relational_expression>"), 46);
            transitions.Add(new Tuple<int, string>(33, "<not_expression>"), 47);
            transitions.Add(new Tuple<int, string>(33, "<not_operator>"), 49);

            transitions.Add(new Tuple<int, string>(34, "<open_brace>"), 41);

            transitions.Add(new Tuple<int, string>(35, "<open_brace>"), 42);

            transitions.Add(new Tuple<int, string>(36, "<logic_expression>"), 43);
            transitions.Add(new Tuple<int, string>(36, "<logic_operand>"), 44);
            transitions.Add(new Tuple<int, string>(36, "<logic_condition>"), 45);
            transitions.Add(new Tuple<int, string>(36, "<relational_expression>"), 46);
            transitions.Add(new Tuple<int, string>(36, "<not_expression>"), 47);
            transitions.Add(new Tuple<int, string>(36, "<arithmetic_expression>"), 48);
            transitions.Add(new Tuple<int, string>(36, "<not_operator>"), 49);
            transitions.Add(new Tuple<int, string>(36, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(36, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(36, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(36, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(36, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(36, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(37, "<atr>"), 56);
            transitions.Add(new Tuple<int, string>(37, "<identifier>"), 57);

            transitions.Add(new Tuple<int, string>(38, "<logic_expression>"), 58);
            transitions.Add(new Tuple<int, string>(38, "<logic_operand>"), 44);
            transitions.Add(new Tuple<int, string>(38, "<logic_condition>"), 45);
            transitions.Add(new Tuple<int, string>(38, "<relational_expression>"), 46);
            transitions.Add(new Tuple<int, string>(38, "<not_expression>"), 47);
            transitions.Add(new Tuple<int, string>(38, "<arithmetic_expression>"), 48);
            transitions.Add(new Tuple<int, string>(38, "<not_operator>"), 49);
            transitions.Add(new Tuple<int, string>(38, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(38, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(38, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(38, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(38, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(38, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(39, "<something>"), 59);
            transitions.Add(new Tuple<int, string>(39, "<number>"), 60);
            transitions.Add(new Tuple<int, string>(39, "<string>"), 61);
            transitions.Add(new Tuple<int, string>(39, "<identifier>"), 62);

            transitions.Add(new Tuple<int, string>(40, "<end_of_statement>"), 63);

            transitions.Add(new Tuple<int, string>(41, "<close_brace>"), 83);
            transitions.Add(new Tuple<int, string>(41, "<case_statement>"), 84);
            transitions.Add(new Tuple<int, string>(41, "<default_statement>"), 85);
            transitions.Add(new Tuple<int, string>(41, "<case>"), 86);
            transitions.Add(new Tuple<int, string>(41, "<default>"), 68);

            transitions.Add(new Tuple<int, string>(42, "<close_brace>"), 64);
            transitions.Add(new Tuple<int, string>(42, "<case_statement>"), 65);
            transitions.Add(new Tuple<int, string>(42, "<default_statement>"), 66);
            transitions.Add(new Tuple<int, string>(42, "<case>"), 67);
            transitions.Add(new Tuple<int, string>(42, "<default>"), 68);

            transitions.Add(new Tuple<int, string>(43, "<close_parenthesis>"), 69);

            transitions.Add(new Tuple<int, string>(48, "<relational_operator>"), 70);

            transitions.Add(new Tuple<int, string>(49, "<logic_condition>"), 71);
            transitions.Add(new Tuple<int, string>(49, "<identifier>"), 72);

            transitions.Add(new Tuple<int, string>(51, "<sum_operator>"), 73);


            transitions.Add(new Tuple<int, string>(52, "<multiplicative_operator>"), 74);

            transitions.Add(new Tuple<int, string>(53, "<sum_expression>"), 75);
            transitions.Add(new Tuple<int, string>(53, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(53, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(53, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(53, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(53, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(56, "<end_of_statement>"), 76);

            transitions.Add(new Tuple<int, string>(57, "<attribution_operator>"), 77);

            transitions.Add(new Tuple<int, string>(58, "<close_parenthesis>"), 78);

            transitions.Add(new Tuple<int, string>(59, "<end_of_statement>"), 79);

            transitions.Add(new Tuple<int, string>(65, "<close_brace>"), 80);
            transitions.Add(new Tuple<int, string>(65, "<default_statement>"), 81);
            transitions.Add(new Tuple<int, string>(65, "<default>"), 68);

            transitions.Add(new Tuple<int, string>(66, "<close_brace>"), 82);

            transitions.Add(new Tuple<int, string>(67, "<number>"), 87);

            transitions.Add(new Tuple<int, string>(68, "<statement>"), 88);
            transitions.Add(new Tuple<int, string>(68, "<break>"), 89);
            transitions.Add(new Tuple<int, string>(68, "<end_of_statement>"), 90);
            transitions.Add(new Tuple<int, string>(68, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(68, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(68, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(68, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(68, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(68, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(68, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(68, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(68, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(68, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(68, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(68, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(68, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(68, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(68, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(68, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(68, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(68, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(68, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(69, "<open_brace>"), 91);

            transitions.Add(new Tuple<int, string>(70, "<arithmetic_expression>"), 92);
            transitions.Add(new Tuple<int, string>(70, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(70, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(70, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(70, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(70, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(70, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(73, "<sum_expression>"), 93);
            transitions.Add(new Tuple<int, string>(73, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(73, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(73, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(73, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(73, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(74, "<multiplicative_expression>"), 94);
            transitions.Add(new Tuple<int, string>(74, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(74, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(74, "<identifier>"), 54);

            transitions.Add(new Tuple<int, string>(75, "<close_parenthesis>"), 95);

            transitions.Add(new Tuple<int, string>(76, "<logic_expression>"), 96);
            transitions.Add(new Tuple<int, string>(76, "<logic_operand>"), 44);
            transitions.Add(new Tuple<int, string>(76, "<logic_condition>"), 45);
            transitions.Add(new Tuple<int, string>(76, "<relational_expression>"), 46);
            transitions.Add(new Tuple<int, string>(76, "<not_expression>"), 47);
            transitions.Add(new Tuple<int, string>(76, "<arithmetic_expression>"), 48);
            transitions.Add(new Tuple<int, string>(76, "<not_operator>"), 49);
            transitions.Add(new Tuple<int, string>(76, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(76, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(76, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(76, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(76, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(76, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(77, "<attribution_operator>"), 97);
            transitions.Add(new Tuple<int, string>(77, "<arithmetic_expression>"), 98);
            transitions.Add(new Tuple<int, string>(77, "<logic_expression>"), 99);
            transitions.Add(new Tuple<int, string>(77, "<logic_operand>"), 44);
            transitions.Add(new Tuple<int, string>(77, "<logic_condition>"), 45);
            transitions.Add(new Tuple<int, string>(77, "<relational_expression>"), 46);
            transitions.Add(new Tuple<int, string>(77, "<not_expression>"), 47);
            transitions.Add(new Tuple<int, string>(77, "<not_operator>"), 49);
            transitions.Add(new Tuple<int, string>(77, "<sum_expression>"), 50);
            transitions.Add(new Tuple<int, string>(77, "<multiplicative_expression>"), 51);
            transitions.Add(new Tuple<int, string>(77, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(77, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(77, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(77, "<number>"), 55);
            transitions.Add(new Tuple<int, string>(77, "<exp>"), 165);

            transitions.Add(new Tuple<int, string>(78, "<open_brace>"), 100);

            transitions.Add(new Tuple<int, string>(81, "<close_brace>"), 101);

            transitions.Add(new Tuple<int, string>(84, "<close_brace>"), 102);
            transitions.Add(new Tuple<int, string>(84, "<default_statement>"), 103);
            transitions.Add(new Tuple<int, string>(84, "<default>"), 68);

            transitions.Add(new Tuple<int, string>(85, "<close_brace>"), 104);

            transitions.Add(new Tuple<int, string>(86, "<number>"), 105);

            transitions.Add(new Tuple<int, string>(87, "<statement>"), 106);
            transitions.Add(new Tuple<int, string>(87, "<break>"), 107);
            transitions.Add(new Tuple<int, string>(87, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(87, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(87, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(87, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(87, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(87, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(87, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(87, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(87, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(87, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(87, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(87, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(87, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(87, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(87, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(87, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(87, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(87, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(87, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(88, "<break>"), 108);
            transitions.Add(new Tuple<int, string>(88, "<end_of_statement>"), 109);

            transitions.Add(new Tuple<int, string>(89, "<end_of_statement>"), 110);

            transitions.Add(new Tuple<int, string>(91, "<statement>"), 111);
            transitions.Add(new Tuple<int, string>(91, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(91, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(91, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(91, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(91, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(91, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(91, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(91, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(91, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(91, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(91, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(91, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(91, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(91, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(91, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(91, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(91, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(91, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(91, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(95, "<multiplicative_operator>"), 112);

            transitions.Add(new Tuple<int, string>(96, "<end_of_statement>"), 113);

            transitions.Add(new Tuple<int, string>(97, "<end_of_statement>"), 114);

            transitions.Add(new Tuple<int, string>(100, "<statement>"), 115);
            transitions.Add(new Tuple<int, string>(100, "<close_brace>"), 116);
            transitions.Add(new Tuple<int, string>(100, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(100, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(100, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(100, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(100, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(100, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(100, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(100, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(100, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(100, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(100, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(100, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(100, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(100, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(100, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(100, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(100, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(100, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(100, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(103, "<close_brace>"), 117);

            transitions.Add(new Tuple<int, string>(105, "<statement>"), 118);
            transitions.Add(new Tuple<int, string>(105, "<break>"), 119);
            transitions.Add(new Tuple<int, string>(105, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(105, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(105, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(105, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(105, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(105, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(105, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(105, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(105, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(105, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(105, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(105, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(105, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(105, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(105, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(105, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(105, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(105, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(105, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(106, "<break>"), 120);

            transitions.Add(new Tuple<int, string>(107, "<end_of_statement>"), 121);

            transitions.Add(new Tuple<int, string>(108, "<end_of_statement>"), 122);

            transitions.Add(new Tuple<int, string>(111, "<close_brace>"), 123);

            transitions.Add(new Tuple<int, string>(112, "<multiplicative_expression>"), 124);
            transitions.Add(new Tuple<int, string>(112, "<operand>"), 52);
            transitions.Add(new Tuple<int, string>(112, "<open_parenthesis>"), 53);
            transitions.Add(new Tuple<int, string>(112, "<identifier>"), 54);
            transitions.Add(new Tuple<int, string>(112, "<number>"), 55);

            transitions.Add(new Tuple<int, string>(113, "<itr>"), 125);
            transitions.Add(new Tuple<int, string>(113, "<identifier>"), 126);

            transitions.Add(new Tuple<int, string>(115, "<close_brace>"), 127);

            transitions.Add(new Tuple<int, string>(118, "<break>"), 128);

            transitions.Add(new Tuple<int, string>(119, "<end_of_statement>"), 129);

            transitions.Add(new Tuple<int, string>(120, "<end_of_statement>"), 130);

            transitions.Add(new Tuple<int, string>(123, "<else_statement>"), 131);
            transitions.Add(new Tuple<int, string>(123, "<else>"), 132);

            transitions.Add(new Tuple<int, string>(125, "<close_parenthesis>"), 133);

            transitions.Add(new Tuple<int, string>(126, "<iteration_operator>"), 134);

            transitions.Add(new Tuple<int, string>(128, "<end_of_statement>"), 135);

            transitions.Add(new Tuple<int, string>(132, "<open_brace>"), 136);
            transitions.Add(new Tuple<int, string>(132, "<if_statement>"), 137);
            transitions.Add(new Tuple<int, string>(132, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(132, "<if_or_else>"), 22);

            transitions.Add(new Tuple<int, string>(133, "<open_brace>"), 138);
            transitions.Add(new Tuple<int, string>(133, "<if>"), 23);

            transitions.Add(new Tuple<int, string>(134, "<number>"), 139);

            transitions.Add(new Tuple<int, string>(136, "<open_brace>"), 140);
            transitions.Add(new Tuple<int, string>(136, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(136, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(136, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(136, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(136, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(136, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(136, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(136, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(136, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(136, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(136, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(136, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(136, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(136, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(136, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(136, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(136, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(136, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(136, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(138, "<statement>"), 141);
            transitions.Add(new Tuple<int, string>(138, "<close_brace>"), 142);
            transitions.Add(new Tuple<int, string>(138, "<statement_type>"), 13);
            transitions.Add(new Tuple<int, string>(138, "<atr>"), 14);
            transitions.Add(new Tuple<int, string>(138, "<conditional>"), 15);
            transitions.Add(new Tuple<int, string>(138, "<rept>"), 16);
            transitions.Add(new Tuple<int, string>(138, "<identifier>"), 17);
            transitions.Add(new Tuple<int, string>(138, "<if_statement>"), 18);
            transitions.Add(new Tuple<int, string>(138, "<switch_statement>"), 19);
            transitions.Add(new Tuple<int, string>(138, "<switch>"), 20);
            transitions.Add(new Tuple<int, string>(138, "<if_conditional>"), 21);
            transitions.Add(new Tuple<int, string>(138, "<if_or_else>"), 22);
            transitions.Add(new Tuple<int, string>(138, "<if>"), 23);
            transitions.Add(new Tuple<int, string>(138, "<for_statement>"), 24);
            transitions.Add(new Tuple<int, string>(138, "<while_statement>"), 25);
            transitions.Add(new Tuple<int, string>(138, "<for>"), 26);
            transitions.Add(new Tuple<int, string>(138, "<while>"), 27);
            transitions.Add(new Tuple<int, string>(138, "<read_statement>"), 145);
            transitions.Add(new Tuple<int, string>(138, "<write_statement>"), 146);
            transitions.Add(new Tuple<int, string>(138, "<read>"), 147);
            transitions.Add(new Tuple<int, string>(138, "<write>"), 148);

            transitions.Add(new Tuple<int, string>(140, "<close_brace>"), 143);

            transitions.Add(new Tuple<int, string>(141, "<close_brace>"), 144);

            transitions.Add(new Tuple<int, string>(147, "<open_parenthesis>"), 149);

            transitions.Add(new Tuple<int, string>(148, "<open_parenthesis>"), 150);

            transitions.Add(new Tuple<int, string>(149, "<identifier>"), 151);

            transitions.Add(new Tuple<int, string>(150, "<string>"), 152);
            transitions.Add(new Tuple<int, string>(150, "<identifier>"), 153);

            transitions.Add(new Tuple<int, string>(151, "<close_parenthesis>"), 154);

            transitions.Add(new Tuple<int, string>(152, "<close_parenthesis>"), 155);

            transitions.Add(new Tuple<int, string>(153, "<close_parenthesis>"), 156);

            transitions.Add(new Tuple<int, string>(154, "<end_of_statement>"), 157);

            transitions.Add(new Tuple<int, string>(155, "<end_of_statement>"), 158);

            transitions.Add(new Tuple<int, string>(156, "<end_of_statement>"), 159);

            transitions.Add(new Tuple<int, string>(160, "<identifier>"), 161);

            transitions.Add(new Tuple<int, string>(161, "<attribution_operator>"), 162);

            transitions.Add(new Tuple<int, string>(162, "<something>"), 163);
            transitions.Add(new Tuple<int, string>(162, "<number>"), 60);
            transitions.Add(new Tuple<int, string>(162, "<string>"), 61);
            transitions.Add(new Tuple<int, string>(162, "<identifier>"), 62);

            transitions.Add(new Tuple<int, string>(163, "<end_of_statement>"), 164);

            transitions.Add(new Tuple<int, string>(165, "<end_of_statement>"), 166);

        }

        private void AddFollow()
        {
            var l = new List<string> {"$"};
            follow.Add("<program>", l);

            l = new List<string> {"<code_zone_token>"};
            follow.Add("<declaration_zone_statement>", l);

            l = new List<string> {"<close_brace>"};
            follow.Add("<var_declaration>", l);

            l = new List<string> {"<type>", "<close_brace>"};
            follow.Add("<assignment>", l);

            l = new List<string> {"<end_of_statement>"};
            follow.Add("<something>", l);

            l = new List<string> {"$"};
            follow.Add("<code_zone_statement>", l);

            l = new List<string> {"<close_brace>", "<case>"};
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
                "<close_brace>",
                "<case>"
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
                "<end_of_statement>",
                "<close_brace>",
                "<case>"
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

            l = new List<string> {"<default_statement>", "<close_brace>", "<case>"};
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
                "<relational_operator>",
                "<logic_operator>",
                "<close_parenthesis>",
                "<end_of_statement>"
            };
            follow.Add("<arithmetic_expression>", l);

            l = new List<string>
            {
                "<multiplicative_operator>",
                "<sum_operator>",
                "<relational_operator>",
                "<logic_operator>",
                "<close_parenthesis>",
                "<end_of_statement>",
                "<close_parenthesis>"
            };
            follow.Add("<operand>", l);

            l = new List<string>
            {
                "<relational_operator>",
                "<logic_operator>",
                "<close_parenthesis>",
                "<end_of_statement>",
                "<close_parenthesis>"
            };
            follow.Add("<sum_expression>", l);

            l = new List<string>
            {
                "<sum_operator>",
                "<relational_operator>",
                "<logic_operator>",
                "<close_parenthesis>",
                "<end_of_statement>",
                "<close_parenthesis>"
            };
            follow.Add("<multiplicative_expression>", l);




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