﻿namespace Identity.Web.PermissionRoot
{
    public static class Permissions
    {
        public static class Stock
        {
            public const string Read = "Permission.Stock.Read";
            public const string Add = "Permission.Stock.Add";
            public const string Update = "Permission.Stock.Update";
            public const string Delete = "Permission.Stock.Delete";
        }

        public static class Order
        {
            public const string Read = "Permission.Order.Read";
            public const string Add = "Permission.Order.Add";
            public const string Update = "Permission.Order.Update";
            public const string Delete = "Permission.Order.Delete";
        }

        public static class Catalog
        {
            public const string Read = "Permission.Catalog.Read";
            public const string Add = "Permission.Catalog.Add";
            public const string Update = "Permission.Catalog.Update";
            public const string Delete = "Permission.Catalog.Delete";
        }
    }
}
