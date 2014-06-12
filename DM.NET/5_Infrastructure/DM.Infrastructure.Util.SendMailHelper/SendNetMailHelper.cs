using System;
using System.Net.Mail;


namespace DM.Infrastructure.Util.SendMailHelper
{
    public class SendMail
    {
        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="entity">邮件实体</param>
        /// <returns>是否发送成功:true为成功，false为失败</returns>
        public bool SendNetMail(MailEntity entity)
        {
            bool bl = false;
            string mailServer = "";
            //string mailServer = GetConfigData.GetConfigValue("MailServer");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            MailAddress From = new MailAddress(entity.FromMail, entity.SendName);
            MailMessage message = new MailMessage();

            smtp.Host = mailServer;

            //添加收件人邮箱
            if (!String.IsNullOrEmpty(entity.ToMail))
            {
                string[] strto = entity.ToMail.Split(new char[] { ',', ';' });
                for (int n = 0; n < strto.Length; n++)
                {
                    if (strto[n].ToString() != "")
                    {
                        MailAddress Mailto = new MailAddress(strto[n]);
                        message.To.Add(Mailto);
                    }
                }
            }

            //添加CC人邮箱
            if (!String.IsNullOrEmpty(entity.CCMail))
            {
                string[] strcc = entity.CCMail.Split(new char[] { ',', ';' });
                for (int m = 0; m < strcc.Length; m++)
                {
                    if (strcc[m].ToString() != "")
                    {
                        MailAddress CC = new MailAddress(strcc[m]);
                        message.CC.Add(CC);
                    }
                }
            }

            //添加BCC人邮箱
            if (!String.IsNullOrEmpty(entity.BCCMail))
            {
                string[] strbcc = entity.BCCMail.Split(new char[] { ',', ';' });
                for (int n = 0; n < strbcc.Length; n++)
                {
                    if (strbcc[n].ToString() != "")
                    {
                        MailAddress BCC = new MailAddress(strbcc[n]);
                        message.Bcc.Add(BCC);

                    }
                }
            }


            //添加附件
            if (!String.IsNullOrEmpty(entity.AttchmentFilePath))
            {
                Attachment att;
                string[] strpath = entity.AttchmentFilePath.Split(new char[] { ',', ';' });
                for (int i = 0; i < strpath.Length; i++)
                {
                    att = new Attachment(strpath[i]);
                    message.Attachments.Add(att);
                }
            }

            message.Subject = entity.Subject;
            message.Body = entity.Body;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            smtp.Timeout = 600000;

            try
            {
                smtp.Send(message);
                bl = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bl;
        }
    }

    /// <summary>
    /// 定义邮件实体
    /// </summary>
    public class MailEntity
    {
        private string m_SendName;
        private string m_ToMail;
        private string m_CCMail;
        private string m_BCCMail;
        private string m_FromMail;
        private string m_Subject;
        private string m_Body;
        private string m_mailFormat;
        private string m_AttchmentFilePath;

        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string SendName
        {
            get { return m_SendName; }
            set { m_SendName = value; }
        }

        /// <summary>
        /// 收件人邮箱，多个邮箱时用逗号或分号隔开
        /// </summary>
        public string ToMail
        {
            get { return m_ToMail; }
            set { m_ToMail = value; }
        }

        /// <summary>
        /// CC人邮箱，多个邮箱时用逗号或分号隔开
        /// </summary>
        public string CCMail
        {
            get { return m_CCMail; }
            set { m_CCMail = value; }
        }

        /// <summary>
        /// BCC人邮箱，多个邮箱时用逗号或分号隔开
        /// </summary>
        public string BCCMail
        {
            get { return m_BCCMail; }
            set { m_BCCMail = value; }
        }

        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string FromMail
        {
            get { return m_FromMail; }
            set { m_FromMail = value; }
        }

        /// <summary>
        /// 邮件主旨
        /// </summary>
        public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }

        /// <summary>
        /// 邮件格式
        /// </summary>
        public string MailFormat
        {
            get { return m_mailFormat; }
            set { m_mailFormat = value; }
        }

        /// <summary>
        /// 附件实际完整物理地址,多个附件用逗号或分号隔开
        /// </summary>
        public string AttchmentFilePath
        {
            get { return m_AttchmentFilePath; }
            set { m_AttchmentFilePath = value; }
        }
    }
}
