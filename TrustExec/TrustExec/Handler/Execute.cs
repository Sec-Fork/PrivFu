﻿using System;
using TrustExec.Library;

namespace TrustExec.Handler
{
    class Execute
    {
        public static void ExecCommand(CommandLineParser options)
        {
            int domainRid;

            if (options.GetFlag("help"))
            {
                options.GetHelp();

                return;
            }

            try
            {
                domainRid = Convert.ToInt32(options.GetValue("id"));
            }
            catch
            {
                Console.WriteLine("\n[-] Failed to parse RID for virtual domain.\n");

                return;
            }
            
            if (options.GetFlag("shell"))
            {
                if (Modules.RunTrustedInstallerProcess(
                    options.GetValue("domain"),
                    options.GetValue("username"),
                    domainRid,
                    null,
                    options.GetFlag("full")))
                {
                    Console.WriteLine();
                    Console.WriteLine("[>] Exit.");
                }

                Console.WriteLine("[!] Added virtual domain and user are not removed automatically.");
                Console.WriteLine("    |-> To remove added virtual user SID   : {0} -m sid -r -d {1} -u {2}",
                    AppDomain.CurrentDomain.FriendlyName,
                    options.GetValue("domain"),
                    options.GetValue("username"));
                Console.WriteLine("    |-> To remove added virtual domain SID : {0} -m sid -r -d {1}",
                    AppDomain.CurrentDomain.FriendlyName,
                    options.GetValue("domain"));
                Console.WriteLine();
            }
            else if (options.GetValue("command") != null)
            {
                if (Modules.RunTrustedInstallerProcess(
                    options.GetValue("domain"),
                    options.GetValue("username"),
                    domainRid,
                    options.GetValue("command"),
                    options.GetFlag("full")))
                {
                    Console.WriteLine();
                    Console.WriteLine("[>] Exit.");
                }

                Console.WriteLine("[!] Added virtual domain and user are not removed automatically.");
                Console.WriteLine("    |-> To remove added virtual user SID   : {0} -m sid -r -d {1} -u {2}",
                    AppDomain.CurrentDomain.FriendlyName,
                    options.GetValue("domain"),
                    options.GetValue("username"));
                Console.WriteLine("    |-> To remove added virtual domain SID : {0} -m sid -r -d {1}",
                    AppDomain.CurrentDomain.FriendlyName,
                    options.GetValue("domain"));
                Console.WriteLine();
            }
            else
            {
                options.GetHelp();
            }
        }

        public static void SidCommand(CommandLineParser options)
        {
            int domainRid;

            if (options.GetFlag("help"))
            {
                options.GetHelp();

                return;
            }

            try
            {
                domainRid = Convert.ToInt32(options.GetValue("id"));
            }
            catch
            {
                Console.WriteLine("\n[-] Failed to parse RID for virtual domain.\n");

                return;
            }

            if (options.GetFlag("lookup"))
            {
                Modules.LookupSid(
                    options.GetValue("domain"),
                    options.GetValue("username"),
                    options.GetValue("sid"));
            }
            else if (options.GetFlag("remove"))
            {
                if (string.IsNullOrEmpty(options.GetValue("domain")))
                {
                    Console.WriteLine("\n[-] Domain name is not specified.\n");

                    return;
                }

                Modules.RemoveVirtualAccount(options.GetValue("domain"), options.GetValue("username"));
            }
            else if (options.GetFlag("add"))
            {
                if (string.IsNullOrEmpty(options.GetValue("domain")))
                {
                    Console.WriteLine("\n[-] Domain name is not specified.\n");
                    
                    return;
                }

                Modules.AddVirtualAccount(
                    options.GetValue("domain"),
                    options.GetValue("username"),
                    domainRid);
            }
            else
            {
                options.GetHelp();
            }
        }
    }
}
