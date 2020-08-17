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
using System.Diagnostics;
using System.Net.Mime;
using System.IO.Pipelines;
using System.Reflection.Metadata.Ecma335;

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
                    await context.Response.WriteAsync(getQuoteAsync().Result);
                });
            });
            
        }

        //string[] urls = { "https://8a2f59ef9085.ngrok.io", "http://12f1a14e7e50.ngrok.io", "http://4c1449a93861.ngrok.io", "http://7a45a5f78857.ngrok.io", "http://e77fd3b7ed59.ngrok.io", "http://a089177a583a.ngrok.io", "http://aba617d86eae.ngrok.io", "http://26b139b05b0f.ngrok.io", "http://17f7ddd05769.ngrok.io", "http://ef845d6343d7.ngrok.io", "http://5e9e572e07b3.ngrok.io", "http://67e5aa89deb6.ngrok.io"};
        string[] urls = { "http://f53f6340146f.ngrok.io" };
        public string getQuote()
        {
            var watch = Stopwatch.StartNew();

            RandomWord who = getWord(randomUrl() + "/who");
            RandomWord how = getWord(randomUrl() + "/how");
            RandomWord does = getWord(randomUrl() + "/does");
            RandomWord what = getWord(randomUrl() + "/what");

            RandomWord[] temp = { who, how, does, what };
            string outputData = writeDateIntoString(temp);

            watch.Stop();
            outputData += $"Execution Time: {watch.ElapsedMilliseconds} ms <br><br>";

            return outputData;
        }

        public async Task<string> getQuoteAsync()
        {
            var watch = Stopwatch.StartNew();

            Task<RandomWord> who = Task.Run(() => getWord(randomUrl() + "/who"));
            Task<RandomWord> how = Task.Run(() => getWord(randomUrl() + "/how"));
            Task<RandomWord> does = Task.Run(() => getWord(randomUrl() + "/does"));
            Task<RandomWord> what = Task.Run(() => getWord(randomUrl() + "/what"));

            var words = await Task.WhenAll(who, how, does, what);
            string outputData = writeDateIntoString(words);

            watch.Stop();
            outputData += $"Execution Time: {watch.ElapsedMilliseconds} ms ";
            return outputData;
        }

        public RandomWord getWord(string url)
        {
            string word;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) 
            {
                word = stream.ReadToEnd();
            }
            string header = response.Headers["InCamp-Student"];
            RandomWord temp = new RandomWord(word, header);

            response.Close();
            return temp;
        }
        public string randomUrl()
        {
            var rand = new Random();
            return urls[rand.Next(urls.Length)];
        }

        public string writeDateIntoString (RandomWord[] words)
        {
            string quote = words[0].word + " " + words[1].word + " " + words[2].word + " " + words[3].word + "<br>";
            quote += words[0].nameFromHeader + "\t'received from' -> " + words[0].nameFromHeader + "<br>";
            quote += words[1].nameFromHeader + "\t'received from'-> " + words[1].nameFromHeader + "<br>";
            quote += words[2].nameFromHeader + "\t'received from' -> " + words[2].nameFromHeader + "<br>";
            quote += words[3].nameFromHeader + "\t'received from' -> " + words[3].nameFromHeader + "<br>";
            return quote;
        }

        



        


    }
}
