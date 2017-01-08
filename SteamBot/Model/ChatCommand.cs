using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamBot.Model
{
    public class ChatCommand
    {
        //public List<Type> ArgumentTypes;
        public int ArgumentCount;
        public List<string> Arguments;
        string Command;
        public ChatCommand(int argsCount)//params Type[] types)
        {
            //ArgumentTypes = new List<Type>();
            //ArgumentTypes.AddRange(types);
            ArgumentCount = argsCount;
            Arguments = new List<string>();
        }

        public void Parse(string input)
        {
            if (input.ToCharArray()[0] == '/')
            {
                input = input.Substring(1);
                string currentWord = "";
                int parseCounter = -1;
                bool isInMultiword = false;
                foreach (var c in input)
                {
                    switch (c)
                    {
                        case ' ':
                            if (isInMultiword)
                            {
                                currentWord = currentWord + c;
                            }
                            else
                            {
                                if (parseCounter == -1)
                                {
                                    Command = currentWord;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(currentWord))
                                    {
                                        Arguments.Add(currentWord);
                                    }
                                }
                                currentWord = "";
                                parseCounter++;
                            }
                            break;
                        case '"':
                            if (isInMultiword)
                            {
                                if (parseCounter == -1)
                                {
                                    Command = currentWord;
                                }
                                else
                                {
                                    Arguments.Add(currentWord);
                                }
                                currentWord = "";
                                parseCounter++;
                                isInMultiword = false;
                            }
                            else
                            {
                                isInMultiword = true;
                            }
                            break;
                        default:
                            currentWord = currentWord + c;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(currentWord))
                {
                    Arguments.Add(currentWord);
                }
            }
            else
            {
                throw new ArgumentException("Tried parsing a string that isn't a command");
            }
        }

        public List<string> GetArguments()
        {
            return Arguments;
        }

        public string GetArgument(int index)
        {
            return GetArguments().ElementAt(index);
        }
    }
}
