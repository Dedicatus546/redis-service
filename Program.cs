﻿using System.Diagnostics;
using System.ServiceProcess;

namespace RedisService
{
    class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        static void Main()
        {
            ServiceBase.Run(new RedisService());
        }
    }

    partial class RedisService : ServiceBase
    {
        private Process? process = new();

        protected override void OnStart(string[] args)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory).Replace("\\", "/");
            var diskSymbol = basePath[..basePath.IndexOf(":")];
            var confPath = basePath.Replace(diskSymbol + ":", "/cygdrive/" + diskSymbol);
            ProcessStartInfo processStartInfo = new(Path.Combine(basePath, "redis-server.exe"), Path.Combine(confPath, "redis.conf"));
            processStartInfo.WorkingDirectory = basePath;
            process = Process.Start(processStartInfo);
        }

        protected override void OnStop()
        {
            if (process != null)
            {
                process.Kill();
                process.Dispose();
            }
        }
    }
}