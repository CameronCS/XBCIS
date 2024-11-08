#define c
using SG_Server_Interface.Net;

namespace SG_Server_Interface {
    public class APIInterface {
#if c
        private readonly string ADDR = "10.200.229.1";
#elif m
        private readonly string ADDR = "192.168.0.164";
#endif
        private readonly string PORT = "3000";
        private readonly string API_URL;

        // Routes
        public readonly UserRouteHandler        UserRoute;
        public readonly ChildRouteHandler       ChildRoute;
        public readonly EmailRouteHandler       EmailRoute;
        public readonly ResourceRouteHandler    ResourceRoute;
        public readonly GalleryRouteHandler     GalleryRoute;
        public readonly CalendarRouteHandler    CallendarRoute;
        public readonly NewsletterRouteHandler  NewsletterRoute;
        public APIInterface() {
            this.API_URL = $"http://{this.ADDR}:{this.PORT}";
            // Routes 
            this.UserRoute =        new(this.API_URL, "/users");
            this.ChildRoute =       new(this.API_URL, "/child");
            this.EmailRoute =       new(this.API_URL, "/emails");
            this.ResourceRoute =    new(this.API_URL, "/resources");
            this.GalleryRoute =     new(this.API_URL, "/gallery");
            this.CallendarRoute =   new(this.API_URL, "/calendar");
            this.NewsletterRoute =  new(this.API_URL, "/newsletters");
        }
    }
}
