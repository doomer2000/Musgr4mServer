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
using MServer.Models.TestChat;
using System.Drawing;
using System.Drawing.Imaging;

namespace MServer.Services
{
    public class Server
    {
        private HttpListener server;
        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;
        //Key UserIp
        private Dictionary<int, TcpClient> ClientsConnection = new Dictionary<int, TcpClient>();
        //private TcpClient tcpClient;
        //TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        private readonly string ip;
        private readonly string port;



        public Server(string ip, string port)
        {
            Wheel_Context dbcontext = new Wheel_Context();
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
            server.Prefixes.Add($"http://{ip}:{port}/GetFriends/");
            server.Prefixes.Add($"http://{ip}:{port}/CreateChat/");
        }

        public void StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            Console.WriteLine("Server started...");
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Я зашёл в бесконечный цикл");
                        var client = listener.AcceptTcpClient();
                        ListenClient(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });

        }

        public void start()
        {
            StartServer();
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
                                case "CreateChat":
                                    CreateChat();
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
                    case "GET":

                        //if (request.RawUrl.Contains("/GetProfile/"))
                        //{
                        //    Console.WriteLine("/GetProfile/");
                        //    string login = request.RawUrl.Split('&')[1];
                        //    string password = request.RawUrl.Split('&')[2];
                        //    //using (Stream stream = request.InputStream)
                        //    //{
                        //    //    using (StreamReader reader = new StreamReader(stream))
                        //    //    {
                        //    //        string data = reader.ReadToEnd();
                        //    //        Console.WriteLine(data);
                        //    //    }
                        //    //}
                        //    using (Stream stream = response.OutputStream)
                        //    {
                        //        User user = await userService.TryLogin(login, password);
                        //        if (user != null)
                        //        {
                        //            Console.WriteLine("reg :" + user.Id.ToString() + ' ' + user.Login);
                        //            string jsonObj = JsonConvert.SerializeObject(user);
                        //            byte[] buffer = Encoding.UTF8.GetBytes(jsonObj);
                        //            response.ContentLength64 = buffer.Length;
                        //            stream.Write(buffer, 0, buffer.Length);
                        //            stream.Close();
                        //        }
                        //    }
                        //}
                        break;
                }


            }
        }


        public void ListenClient(TcpClient client)
        {
            string userId = null;
            bool connected = true;
            var reader = new StreamReader(client.GetStream());
            var writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;


            while (connected)
            {
                var data = reader.ReadLine();
                var command = data.Split('~')[1];
                var message = data.Split('~')[0];
                switch (command)
                {
                    case "connect":
                        userId = message;
                        Console.WriteLine($"{userId.ToString()} listening");
                        Console.WriteLine($"{userId} connected!");
                        ClientsConnection.Add(Int32.Parse(userId), client);
                        Console.WriteLine($"{ClientsConnection.Count} users connected");
                        break;
                    case "message":
                        Console.WriteLine($"{userId}: {message}");
                        BroadcastMessage(message);
                        break;
                        //case "end":
                        //    Console.WriteLine($"{username} disconnected!");
                        //    connected = false;
                        //    chatUsers.RemoveAll(x => x.Username == username);
                        //    Console.WriteLine($"{chatUsers.Count} users connected");
                        //    break;
                }
            }

            reader.Close();
            writer.Close();
            client.Close();

        }

        public void BroadcastMessage(string messagejson)
        {
            bool msgCrtd = false;
            Models.TestChat.Message message = JsonConvert.DeserializeObject<Models.TestChat.Message>(messagejson);
            foreach (ChatMember u in message.Chat.ChatMembers)
            {
                if (message.User.Id != u.User.Id)
                {
                    if (u.User.IsOnline)
                    {
                        StreamWriter streamWriter = new StreamWriter(ClientsConnection[u.Id].GetStream());
                        streamWriter.WriteLine($"{message}~new-message");
                    }
                    else
                    {
                        CommunicationService communicationService = new CommunicationService(new Wheel_Context());
                        communicationService.WriteUnreadMessage(message);
                        msgCrtd = true;
                    }
                }
            }
        }

        private void CreateChat()
        {
            using (Stream stream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                  
                    
                        string test = reader.ReadToEnd();
                        //JsonConverter<Chat> jsonConverter = new JsonConverter();
                        
                        Chat mychat = JsonConvert.DeserializeObject<Chat>(reader.ReadToEnd());

                        Console.WriteLine($"{mychat.Title}\n" +
                            $"{mychat.IsPrivate}\n" +
                            $"{mychat.ChatMembers.Count}");
                        Wheel_Context wheel_Context = new Wheel_Context();
                        ChatService chatService = new ChatService(wheel_Context);
                        chatService.CreateChat(mychat);
                    
                    using (Stream outstream = response.OutputStream)
                    {

                        //Bitmap bitmap = new Bitmap(@"C:\Users\qwert\Desktop\1.jpg");
                        //ImageConverter converter = new ImageConverter();
                        //string jsonObj = JsonConvert.SerializeObject(bitmap);
                        //byte[] buffer = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
                        //response.ContentLength64 = buffer.Length;
                        //outstream.Write(buffer, 0, buffer.Length);
                        outstream.Close();



                    }

                    Wheel_Context dbcontext = new Wheel_Context();
                    IUserService userService = new UserService(dbcontext);
                }
            }
        }

        //private bool Numcheck(string str)
        //{
        //    return int.TryParse(str, out int id);
        //}


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
            try
            {
                using (Stream stream = request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = reader.ReadToEnd();
                        string login = data.Split('&')[1];
                        string password = data.Split('&')[2];
                        //string deviceIp = data.Split('&')[3];

                        Wheel_Context dbcontext = new Wheel_Context();
                        IUserService userService = new UserService(dbcontext);


                        User exist = await userService.IsUserExist(login, password);
                        if (exist != null)
                        {
                            //Device device = new Device() { User = exist, IpAdress = deviceIp };

                            //await userService.AddDevice(device);
                            User user = await userService.TryLogin(login, password);
                            userService.SetOnline(user.Id);

                            using (Stream outstream = response.OutputStream)
                            {
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
            catch (Exception ex)
            {

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


