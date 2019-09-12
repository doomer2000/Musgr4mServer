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
using System.Threading;
using System.Windows.Forms;
using MServer.Models;
using MServer.Repositoies;
using MServer.Repositoies.Interfaces;
using System.Text.RegularExpressions;

namespace MServer.Services
{
    public class Server
    {
        private HttpListener server;
        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;
        private readonly string ip;
        private readonly string port;

        public Server(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
            Console.OutputEncoding = Encoding.UTF8;
            server = new HttpListener();

            ServerConfiguration();
            server.Start();
            start();
        }


        private void ServerConfiguration()
        {
            server.Prefixes.Add($"http://{ip}:{port}/TryLogin/");
            server.Prefixes.Add($"http://{ip}:{port}/GetPlaylist/");
            server.Prefixes.Add($"http://{ip}:{port}/Registration/");
            server.Prefixes.Add($"http://{ip}:{port}/");
        }


        public void start()
        {
            Console.WriteLine("Ожидание подключений...");
            while (true)
            {
                context = server.GetContext();
                request = context.Request;
                response = context.Response;
                switch (request.HttpMethod)
                {
                    case "POST":
                        if (request.RawUrl.ToString().Split('/')[2] == "")
                        {
                            switch (request.RawUrl.ToString().Split('/')[1])
                            {
                                case "Registration":
                                    Console.WriteLine($"Darova Bandit");
                                    Registration();
                                    break;
                                case "ChangePassword":
                                    break;
                                case "TryLogin":
                                    Task.Run(() => TryLogin());
                                    // TryLogin();
                                    break;
                            }
                        }
                        else
                        {
                            switch (request.RawUrl.ToString().Split('/')[2])
                            {
                                case "GetPlaylist":
                                    GetPlaylist();
                                    break;

                                case "":
                                    string[] test = request.RawUrl.Split('/');
                                    int id;
                                    int.TryParse(request.RawUrl.Split('/')[1].ToString(), out id);
                                    Console.WriteLine(id);
                                    GetProfile(id);
                                    break;
                            }
                        }
                        break;
                        //case "GET":
                        //    if (request.RawUrl.Contains("/GetProfile/"))
                        //    {
                        //        Console.WriteLine("/GetProfile/");
                        //        string login = request.RawUrl.Split('&')[1];
                        //        string password = request.RawUrl.Split('&')[2];
                        //        //using (Stream stream = request.InputStream)
                        //        //{
                        //        //    using (StreamReader reader = new StreamReader(stream))
                        //        //    {
                        //        //        string data = reader.ReadToEnd();
                        //        //        Console.WriteLine(data);
                        //        //    }
                        //        //}
                        //        using (Stream stream = response.OutputStream)
                        //        {
                        //            User user = await userService.TryLogin(login, password);
                        //            if (user != null)
                        //            {
                        //                Console.WriteLine("reg :" + user.Id.ToString() + ' ' + user.Login);
                        //                string jsonObj = JsonConvert.SerializeObject(user);
                        //                byte[] buffer = Encoding.UTF8.GetBytes(jsonObj);
                        //                response.ContentLength64 = buffer.Length;
                        //                stream.Write(buffer, 0, buffer.Length);
                        //                stream.Close();
                        //            }
                        //        }
                        //    }
                        //break;
                }


            }
        }


        private bool Numcheck(string str)
        {
            return int.TryParse(str, out int id);
        }


        private async void GetProfile(int id)
        {

            Wheel_Context dbcontext = new Wheel_Context();
            IUserManipulationService userManipulationService = new UserManipulationService(dbcontext);
            using (Stream stream = request.InputStream)
            {
                using (Stream outstream = response.OutputStream)
                {
                    User user = await userManipulationService.GetUserByID(id);
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

        public async void TryLogin()
        {
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    string login = data.Split('&')[1];
                    string password = data.Split('&')[2];
                    string deviceIp = data.Split('&')[3];

                    Wheel_Context dbcontext = new Wheel_Context();
                    IUserService userService = new UserService(dbcontext);


                    User exist = await userService.IsUserExist(login, password);
                    if (exist != null)
                    {
                        Device device = new Device() { User = exist, IpAdress = deviceIp };
                        await userService.AddDevice(device);
                        User user = await userService.TryLogin(login, password);

                        using (Stream outstream = response.OutputStream)
                        {
                            Console.WriteLine("1111");
                            string jsonObj = JsonConvert.SerializeObject(user);
                            byte[] buffer = Encoding.UTF8.GetBytes(jsonObj);
                            response.ContentLength64 = buffer.Length;
                            outstream.Write(buffer, 0, buffer.Length);
                            outstream.Close();

                        }
                    }
                    else
                    {
                        using (Stream outstream = response.OutputStream)
                        {
                            string jsonObj = JsonConvert.SerializeObject(null);
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
            Wheel_Context dbcontext = new Wheel_Context();
            IUserManipulationService userManipulationService = new UserManipulationService(dbcontext);
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dynamic json = reader.ReadToEnd();
                    User user = JsonConvert.DeserializeObject<User>(json);
                    user = userManipulationService.Registration(user);
                    Console.WriteLine("reged: " + user.Id.ToString() + ' ' + user.Login);

                }
            }
        }



        public static void GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                }
            }
        }

    }
}


