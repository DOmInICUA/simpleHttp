using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;

namespace newHttp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            string[] who = { "Люк Скайуокер", "Тайлер Дерден", "Джек Воробей", "Нео", "Т-800", "Чубака" };
            string[] how = { "красиво", "хитро", "виртуозно", "умно", "глупо", "лучше всех", "просто" };
            string[] does = { "пишет", "танцует", "ест", "бросает" };
            string[] what = { "маслину", "код", "брейк-данс", "письмо", "ламбаду"};
            var rand = new Random();
                
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/who", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(who[rand.Next(who.Length)]);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/how", async context =>
                {   
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(how[rand.Next(how.Length)]);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/does", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(does[rand.Next(does.Length)]);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/what", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(what[rand.Next(what.Length)]);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/quote", async context =>
                {
                    context.Response.ContentType = "text/html; charset=UTF-8";
                    
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(who[rand.Next(who.Length)] + " " + how[rand.Next(how.Length)] + " " + does[rand.Next(does.Length)] + " " + what[rand.Next(what.Length)]);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Bogdan Kovalchuk");
                    await context.Response.WriteAsync(getQuote());
                });
            });
            
        }

        string[] urls = {"http://localhost:3030" };
        public Tuple<string, string> getWord(string url)
        {
            string word;
            string header;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) 
            {
                word = stream.ReadToEnd();
            }
            header = response.Headers["InCamp-Student"];

            return Tuple.Create(word, header);
        }
        public string randomUrl()
        {
            var rand = new Random();
            return urls[rand.Next(urls.Length)];
        }
        public string getQuote()
        {
            Tuple<string, string> who = getWord(randomUrl() + "/who");
            Tuple<string, string> how = getWord(randomUrl() + "/how");
            Tuple<string, string> does = getWord(randomUrl() + "/does");
            Tuple<string, string> what = getWord(randomUrl() + "/what");
            string quote = who.Item1 + " " + how.Item1 + " " + does.Item1 + " " + what.Item1 + "\n";
            quote += who.Item1 + "\t'received from' -> " + who.Item2 + "\n";
            quote += how.Item1 + "\t'received from'-> " + how.Item2 + "\n";
            quote += does.Item1 + "\t'received from' -> " + does.Item2 + "\n";
            quote += what.Item1 + "\t'received from' -> " + what.Item2 + "\n";
            return quote;
        }

    }
}
