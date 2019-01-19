using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace LoginSecurity
{
    class Program
    {
        private static string filename;
        private static FilterInfoCollection WebcamColl;
        private static VideoCaptureDevice Device;
        private static System.Drawing.Image img;
        static void Main(string[] args)
        {
            TakePhoto();
            Thread.Sleep(1000);
            SendEmail();
        }

        private static void TakePhoto()
        {
            filename = @"D:\OneDrive\Pictures\Login\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".jpg";
            WebcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //if you have connected with one more camera choose index as you want 
            Device = new VideoCaptureDevice(WebcamColl[0].MonikerString);
            Device.Start();
            Device.NewFrame += new NewFrameEventHandler(Device_NewFrame);
        }
        private static void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            img = (Bitmap)eventArgs.Frame.Clone();
            img.Save(@filename);
            Device.SignalToStop();
        }

        private static void SendEmail()
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress("XXX");
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress("XXX"));
            //邮件标题。
            mailMessage.Subject = "XXX";
            //邮件内容。
            mailMessage.Body = "XXX";

            mailMessage.Attachments.Add(new Attachment(filename));
            //设置附件类型
            mailMessage.Attachments[0].ContentType.Name = "image/jpg";
            //设置附件 Id
            mailMessage.Attachments[0].ContentId = "XXX";
            //设置附件为 inline-内联
            mailMessage.Attachments[0].ContentDisposition.Inline = true;
            //设置附件的编码格式
            mailMessage.Attachments[0].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;


            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = "XXX";
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential("XXX@qq.com", "XXX");
            //发送
            client.Send(mailMessage);
        }
    }
}
