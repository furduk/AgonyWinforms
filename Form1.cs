using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgonyWinforms
{
    public partial class Form1 : Form
    {
        private Random RandomWhatUsedOnlyOnce = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> varInt = new Dictionary<string, int> {};
            Dictionary<string, string> varStr = new Dictionary<string, string> { };
            StringBuilder output = new StringBuilder();
            long ifTrue = 9223372036854775807;
            // -9223372036854775808 = true ; 9223372036854775807 = false ;
            string[] lines = inputbox.Lines;
            int i = 0;
            
                while (i < lines.Length)
            {
                try
                {
                    string line = lines[i];
                char[] chars;
                string command;
            retry_label:

                if (line.Length < 4)
                {
                    i++;
                    continue;
                }

                chars = line.Take(4).ToArray();
                command = new string(chars);

                switch (command)
                {

                        case "rnd>":
                            string[] rndCmd = line.Remove(0, 5).Split(' ');
                            varInt[rndCmd[0]] = RandomWhatUsedOnlyOnce.Next(int.Parse(rndCmd[1]));
                            break;
                        case "inp>":
                            string[] inpCmd = line.Remove(0, 5).Split(' ');
                            if (inpCmd[0] == "sstr")
                            {
                                string inputString = Interaction.InputBox("Введите значение", $"enter value for {inpCmd[0]} {inpCmd[1]}");
                                varStr[inpCmd[1]] = inputString;
                            }
                            else if (inpCmd[0] == "sint")
                            {
                                string inputString = Interaction.InputBox("Введите значение", $"enter value for {inpCmd[0]} {inpCmd[1]}");
                                varInt[inpCmd[1]] = int.Parse(inputString);
                            }
                                break;
                        case "clr>":
                            outputBox.Text = "";
                            output.Clear();
                            break;
                    case "jmp>":
                        string targetFlag = line.Remove(0, 5).Trim();
                        for (int j = 0; j < lines.Length; j++)
                        {
                            if (lines[j].Trim().StartsWith("flag " + targetFlag))
                            {
                                i = j;
                                goto end_of_line;
                            }
                        }
                        output.Append("ERROR: Flag " + targetFlag + " not found\n");
                        break;
                    case "msg>":
                        string msg = line.Remove(0, 5);
                        string msgcaption = msg.Split(' ')[0];
                        string msgText = msg.Remove(0, msgcaption.Length + 1);
                        MessageBox.Show(msgText,msgcaption,MessageBoxButtons.OK);
                        break;
                    case "flag":
                        break;
                        case "ifvv": // IF Var Var
                        string[] ifCon = line.Remove(0, 5).Split(' ');
                        if (ifCon[0] == "sstr")
                        {
                            if (ifCon[2] == "==") 
                            {
                                if (varStr[ifCon[1]] == varStr[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == "!=")
                            {
                                if (varStr[ifCon[1]] != varStr[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                        }
                        else if (ifCon[0] == "sint")
                        {
                            if (ifCon[2] == "==")
                            {
                                if (varInt[ifCon[1]] == varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == "!=")
                            {
                                if (varInt[ifCon[1]] != varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == ">")
                            {
                                if (varInt[ifCon[1]] > varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == "<")
                            {
                                if (varInt[ifCon[1]] < varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == ">=")
                            {
                                if (varInt[ifCon[1]] >= varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                            else if (ifCon[2] == "<=")
                            {
                                if (varInt[ifCon[1]] <= varInt[ifCon[3]]) { ifTrue = -9223372036854775808; } else { ifTrue = 9223372036854775807; }
                            }
                        }
                        break;
                    case "tcon":
                        if (ifTrue == -9223372036854775808)
                        {
                            if (line.Length > 5)
                            {
                                line = line.Substring(5);
                                goto retry_label;
                            }
                        }
                        
                        break;
                    case "fcon":
                        if (ifTrue == 9223372036854775807)
                        {
                            if (line.Length > 5)
                            {
                                line = line.Substring(5);
                                goto retry_label;
                            }
                        }
                        break;
                    case "prt>": // print
                        output.Append(line.Length > 4 ? line.Substring(5) : "");
                        break;
                    case "prtv": // print var
                        string[] prtv = line.Remove(0, 5).Split(' ');
                        if (prtv[0] == "sint")
                        {
                            if (varInt.ContainsKey(prtv[1]))
                            {
                                output.Append(varInt[prtv[1]]);
                            }
                        }
                        else if (prtv[0] == "sstr")
                        {
                            if (varStr.ContainsKey(prtv[1]))
                            {
                                output.Append(varStr[prtv[1]]);
                            }
                        }
                        break;
                    case "nl<>": // next line
                        output.Append("\n");
                        ifTrue = 9223372036854775807;
                        break;
                    case "sint": // set int
                        {
                            var parts = line.Remove(0, 5).Split(new[] { ' ' }, 2);
                            if (parts.Length < 2) break;
                            varInt[parts[0]] = int.Parse(parts[1].Split(' ')[0]);
                            break;
                        }
                    case "sstr": // set string
                        string sstr = line.Remove(0, 5);
                        string sstrKey = sstr.Split(' ')[0];
                        string sstrValue = sstr.Remove(0, sstrKey.Length + 1);
                        if (!varStr.ContainsKey(sstrKey))
                        {
                            varStr.Add(sstrKey, sstrValue);
                        }
                        else
                        {
                            varStr[sstrKey] = sstrValue;
                        }
                        break;
                    case "incr":
                        string incVar = line.Remove(0, 5).Trim();
                        if (varInt.ContainsKey(incVar)) varInt[incVar]++;
                        break;

                    case "decr":
                        string decVar = line.Remove(0, 5).Trim();
                        if (varInt.ContainsKey(decVar)) varInt[decVar]--;
                        break;
                }
            end_of_line:
                i++;
                } catch (Exception ex) { output.Append("ERROR: " + ex.Message); }
        }
        
    outputBox.Text = output.ToString();
        }
    }
}
