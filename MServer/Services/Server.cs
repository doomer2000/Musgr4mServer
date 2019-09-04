using Newtonsoft.Json;
using MServer.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MServer.Models;
using MServer.Repositoies;
using MServer.Repositoies.Interfaces;

namespace MServer.Services
{
    public class Server
    {
        private Wheel_Context dbcontext;
        private IUserManipulationService userManipulationService { get; set; }
        private IUserService userService { get; set; }
        private HttpListener server;
        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;

        private readonly string ip;
        private readonly string port;

        public Server(string ip,string port)
        {
            this.ip = ip;
            this.port = port;
            Console.OutputEncoding = Encoding.UTF8;
            dbcontext = new Wheel_Context();
            userManipulationService = new UserManipulationService(dbcontext);
            userService = new UserService(dbcontext);
            server = new HttpListener();
            ServerConfiguration();
            start();
        }


        private void ServerConfiguration()
        {
            server.Prefixes.Add($"http://{ip}:{port}/GetProfile/");
            server.Prefixes.Add($"http://{ip}:{port}/GetPlaylist/");
            server.Prefixes.Add($"http://{ip}:{port}/Registration/");
        }


        public void start()
        {
            server.Start();
            Console.WriteLine("Ожидание подключений...");
            while (true)
            {
                context = server.GetContext();
                request = context.Request;
                if (request.HttpMethod == "GET")
                {
                    GetMyProfile();
                }
                response = context.Response;
                switch (request.HttpMethod)
                {
                    case "POST":
                        switch (request.Url.ToString().Split('/')[4])
                        {
                            case "/GetProfile/":
                                Task.Run(() => GetMyProfile());
                                break;
                            case "/GetPlaylist/":
                                GetPlaylist();
                                break;
                            case "/Registration/":
                                Registration();
                                break;
                        }
                        break;
                    case "GET":
                        if (request.RawUrl.Contains("/GetProfile/"))
                        {
                            Console.WriteLine("/GetProfile/");
                            string login = request.RawUrl.Split('&')[1];
                            string password = request.RawUrl.Split('&')[2];
                            //using (Stream stream = request.InputStream)
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string data = reader.ReadToEnd();
                            //        Console.WriteLine(data);
                            //    }
                            //}
                            using (Stream stream = response.OutputStream)
                            {
                                User user = userService.GetUser(login, password);
                                if (user != null)
                                {
                                    Console.WriteLine($"{user.Login} loggined");
                                    Console.WriteLine(user.Id.ToString() + ' ' + user.Login);
                                    string jsonObj = JsonConvert.SerializeObject(user);
                                    byte[] buffer = Encoding.UTF8.GetBytes(jsonObj);
                                    response.ContentLength64 = buffer.Length;
                                    stream.Write(buffer, 0, buffer.Length);
                                    stream.Close();
                                }
                            }
                        }
                        break;
                }
            }
        }



        public void GetMyProfile()
        {
            Console.WriteLine("www");
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    Console.WriteLine(data);
                    string login = data.Split('&')[1];
                    string password = data.Split('&')[2];

                    Console.WriteLine(login);
                    Console.WriteLine(password);

                    using (Stream outstream = response.OutputStream)
                    {
                        User user = userService.GetUser(login, password);
                        if (user != null)
                        {
                            Console.WriteLine(user.Id.ToString() + ' ' + user.Login);
                            string jsonObj = JsonConvert.SerializeObject(user);
                            byte[] buffer = Encoding.UTF8.GetBytes(jsonObj);
                            response.ContentLength64 = buffer.Length;
                            outstream.Write(buffer, 0, buffer.Length);
                            outstream.Close();
                        }
                    }
                }
            }
        }


        public void GetPlaylist()
        {
        }

        public void Registration()
        {
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dynamic json = reader.ReadToEnd();
                    User user = JsonConvert.DeserializeObject<User>(json);
                    Console.WriteLine(user.Id.ToString() + ' ' + user.Login);
                    user = userManipulationService.Registration(user);
                    Console.WriteLine(user.Id.ToString() + ' ' + user.Login);
                }
            }
        }

        private dynamic json()
        {
            return null;
        }
    }
}
