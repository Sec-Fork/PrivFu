﻿using System;
using System.Collections.Generic;
using TrustExec.Handler;

namespace TrustExec
{
    class TrustExec
    {
        static void PrintModules()
        {
            Console.WriteLine("Available Modules:\n");
            Console.WriteLine("    + exec - Run process as \"NT SERVICE\\TrustedInstaller\".");
            Console.WriteLine("    + sid  - Add or remove virtual account's SID.");
            Console.WriteLine();
            Console.WriteLine("[*] To see help for each modules, specify \"-m <Module> -h\" as arguments.\n");
        }


        static void Main(string[] args)
        {
            CommandLineParser options = new CommandLineParser();
            string[] reminder;

            try
            {
                options.SetTitle("TrustExec - Tool to investigate TrustedInstaller capability.");
                options.AddFlag(false, "h", "help", "Displays this help message.");
                options.AddParameter(false, "m", "module", null, "Specifies module name.");
                reminder = options.Parse(args);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);

                return;
            }
            catch (ArgumentException ex)
            {
                options.GetHelp();
                Console.WriteLine(ex.Message);

                return;
            }

            if (options.GetValue("module") != null)
            {
                StringComparison opt = StringComparison.OrdinalIgnoreCase;
                CommandLineParser strippedOptions = new CommandLineParser();
                List<string> exclusive;

                if (string.Compare(options.GetValue("module"), "exec", opt) == 0)
                {
                    exclusive = new List<string> { "shell", "command" };

                    try
                    {
                        strippedOptions.SetTitle("TrustExec - Help for \"exec\" command.");
                        strippedOptions.SetOptionName("-m exec");
                        strippedOptions.AddFlag(false, "h", "help", "Displays this help message.");
                        strippedOptions.AddFlag(false, "s", "shell", "Flag for interactive shell.");
                        strippedOptions.AddFlag(false, "f", "full", "Flag to enable all available privileges.");
                        strippedOptions.AddParameter(false, "c", "command", null, "Specifies command to execute.");
                        strippedOptions.AddParameter(false, "d", "domain", "DefaultDomain", "Specifies domain name to add. Default value is \"DefaultDomain\".");
                        strippedOptions.AddParameter(false, "u", "username", "DefaultUser", "Specifies username to add. Default value is \"DefaultUser\".");
                        strippedOptions.AddParameter(false, "i", "id", "110", "Specifies RID for virtual domain. Default value is \"110\".");
                        strippedOptions.AddExclusive(exclusive);
                        strippedOptions.Parse(reminder);
                        Execute.ExecCommand(strippedOptions);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);

                        return;
                    }
                    catch (ArgumentException ex)
                    {
                        strippedOptions.GetHelp();
                        Console.WriteLine(ex.Message);

                        return;
                    }
                }
                else if (string.Compare(options.GetValue("module"), "sid", opt) == 0)
                {
                    exclusive = new List<string> { "add", "remove", "lookup" };

                    try
                    {
                        strippedOptions.SetTitle("TrustExec - Help for \"sid\" command.");
                        strippedOptions.SetOptionName("-m sid");
                        strippedOptions.AddFlag(false, "h", "help", "Displays this help message.");
                        strippedOptions.AddFlag(false, "a", "add", "Flag to add virtual account's SID.");
                        strippedOptions.AddFlag(false, "r", "remove", "Flag to remove virtual account's SID.");
                        strippedOptions.AddFlag(false, "l", "lookup", "Flag to lookup SID or account name in local system.");
                        strippedOptions.AddParameter(false, "d", "domain", null, "Specifies domain name to add or remove. Default value is null.");
                        strippedOptions.AddParameter(false, "u", "username", null, "Specifies username to add or remove. Default value is null.");
                        strippedOptions.AddParameter(false, "i", "id", "110", "Specifies RID for virtual domain to add. Default value is \"110\".");
                        strippedOptions.AddParameter(false, "s", "sid", null, "Specifies SID to lookup.");
                        strippedOptions.AddExclusive(exclusive);
                        strippedOptions.Parse(reminder);
                        Execute.SidCommand(strippedOptions);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);

                        return;
                    }
                    catch (ArgumentException ex)
                    {
                        strippedOptions.GetHelp();
                        Console.WriteLine(ex.Message);

                        return;
                    }
                }
                else
                {
                    Console.WriteLine("\n[-] {0} command is not implemented.\n", options.GetValue("module"));
                }
            }
            else if (options.GetFlag("help"))
            {
                options.GetHelp();
                PrintModules();
            }
            else
            {
                options.GetHelp();
                PrintModules();
            }
        }
    }
}
