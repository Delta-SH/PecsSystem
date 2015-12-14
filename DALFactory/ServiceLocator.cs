using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace Delta.PECS.WebCSC.DALFactory
{
    public static class ServiceLocator
    {
        private static readonly string dalPath = ConfigurationManager.AppSettings["WebDAL"];

        public static object LocateDALObject(string className) {
            string fullPath = String.Format("Delta.PECS.WebCSC.{0}.{1}", dalPath, className);
            return Assembly.Load(dalPath).CreateInstance(fullPath);
        }
    }
}