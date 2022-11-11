namespace SELLit.Server.Features;

public static class Routes
{
    private static string GetIdRoute(string route, string id) => route.Replace("{id:hashids}", id);

    private const string RouteId = "/{id:hashids}";
    
    public static class Identity
    {
        private const string Controller = "Identity";
        
        public const string Login = Controller + "/Login";
        public const string Register = Controller + "/Register";
    }

    public static class Categories
    {
        private const string Controller = "Categories";
        
        public const string Get = Controller + RouteId;
        public const string GetAll = Controller;

        public const string Create = Controller;
        public const string Update = Controller;
        public const string Delete = Controller + RouteId;

        public static string DeleteById(string id) => GetIdRoute(Delete, id);
        public static string GetById(string id) => GetIdRoute(Get, id);
    }

    public static class Products
    {
        private const string Controller = "Products";
        
        public const string Get = Controller + RouteId;
        public const string GetAll = Controller;

        public const string Create = Controller;
        public const string Update = Controller;
        public const string Delete = Controller + RouteId;
        
        public static string DeleteById(string id) => GetIdRoute(Delete, id);
        public static string GetById(string id) => GetIdRoute(Get, id);
    }
}