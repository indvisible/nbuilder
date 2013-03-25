﻿using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Indvisible.RealisticData.Extensions;

namespace Indvisible.RealisticData
{
    public static class Internet
    {
        static Internet()
        {
            Byte = 0.To(255).Select(item => item.ToString(CultureInfo.InvariantCulture)).ToArray();
        }

        public static string GetEmail(string name = null)
        {
            return GetUserName(name) + '@' + GetDomainName();
        }

        /// <summary>
        /// Returns an email address of an online disposable email service (like tempinbox.com).
        /// you can really send an email to these addresses an access it by going to the service web pages.
        /// <param name="name">User Name initial value.</param>
        /// </summary>
        public static string GetDisposableEmail(string name = null)
        {
            return GetUserName(name) + '@' + DisposableHosts.Rand();
        }

        public static string GetFreeEmail(string name = null)
        {
            return GetUserName(name) + "@" + Hosts.Rand();
        }

        public static string GetUserName(string name = null)
        {
            if (name != null)
            {
                var parts = name.Split(' ').Join(new[] { ".", "_" }.Rand());
                return parts.ToLower();
            }

            switch (DataRandom.Rand.Next(2))
            {
                case 0:
                    return new Regex(@"\W").Replace(Name.GetFirstName(), "").ToLower();
                case 1:
                    var parts = new[] { Name.GetFirstName(), Name.GetLastName() }.Select(n => new Regex(@"\W").Replace(n, ""));
                    return parts.Join(new[] { ".", "_" }.Rand()).ToLower();
                default: throw new ApplicationException();
            }
        }

        public static string GetDomainName()
        {
            return GetDomainWord() + "." + GetDomainSuffix();
        }

        public static string GetDomainWord()
        {
            string dw = Company.GetName().Split(' ').First();
            dw = new Regex(@"\W").Replace(dw, "");
            dw = dw.ToLower();
            return dw;
        }

        public static string GetDomainSuffix()
        {
            return DomainSuffixes.Rand();
        }

        public static string GetUri(string protocol)
        {
            return protocol + "://" + GetDomainName();
        }

        public static string GetHttpUrl()
        {
            return GetUri("http");
        }

        public static string GetIP_V4_Address()
        {
            return Byte.RandPick(4).Join(".");
        }


        private static readonly string[] Byte; //new [] { ((0..255).to_a.map { |n| n.to_s })
        static readonly string[] Hosts = new[]
            {
                "gmail.com", "yahoo.com", "hotmail.com"
            };
        static readonly string[] DisposableHosts = new[]
            {
                "mailinator.com", "suremail.info", "spamherelots.com", "binkmail.com", "safetymail.info", "tempinbox.com"
            };
        static readonly string[] DomainSuffixes = new[]
            {
                "co.uk", "com", "us", "uk", "ca", "biz", "info", "name"
            };
    }
}