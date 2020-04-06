using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Collections;
using PortalCyberOffice._CyberOffice._Global;
using System.Net;
using System.Data;

namespace PortalCyberOffice._CyberOffice._Global
{
/* Esta classe é usada por toda a aplicação para enviar e-mails. */
    public class EnviaEmail
    {
        public static bool EnviarEmail(int operador, int usuario, string destinatario, string nome, string destinatariosCopia, string assunto, string corpo)
        {
            bool enviado = false; DataRow drSmtp = configSMTP();
            MailMessage wMail = new MailMessage();
            wMail.From = new MailAddress("cyber01@inforshop.net.br", "Cyber Office");
            wMail.Subject = assunto;
            wMail.Body = corpo;
            wMail.IsBodyHtml = true;
            wMail.To.Add(new MailAddress(destinatario, nome));
            wMail.Bcc.Add(new MailAddress("suporte@inforshop.com.br", "Suporte CyberOffice"));
            wMail.Bcc.Add(new MailAddress("wolney.rodrigues@inforshop.com.br", "Wolney"));

            try
            {
                // Usuários que recebem cópia do e-mail (Vendedor, operador, etc.)
                if (destinatariosCopia.Length > 0 && destinatariosCopia != "")
                {
                    string[] aCC = destinatariosCopia.Split(';');
                    if (aCC.Length > 0)
                    {
                        for (int i = 0; i < aCC.Length; i++)
                        {
                            wMail.CC.Add(new MailAddress(aCC[i].ToString().Trim().ToLower().Replace(",",".")));
                        }
                    }
                }
            }
            catch(Exception ex) { GravaLog.Log(operador, ex.Message.ToString()); }

            try
            {
                if (drSmtp != null && drSmtp.Table.Rows.Count > 0)
                {
                    SmtpClient smEnvia = new SmtpClient();
                    smEnvia.Host = drSmtp["smtp_host"].ToString().Trim();
                    smEnvia.UseDefaultCredentials = Convert.ToBoolean(drSmtp["smtp_usarCredenciaisPadrao"]);
                    smEnvia.Port = Convert.ToInt32(drSmtp["smtp_porta"]);
                    smEnvia.Timeout = 10000;
                    smEnvia.Credentials = new System.Net.NetworkCredential(drSmtp["smtp_conta"].ToString().Trim(), drSmtp["smtp_senha"].ToString().Trim());
                    smEnvia.EnableSsl = Convert.ToBoolean(drSmtp["smtp_ssl"]);

                    try
                    {
                        smEnvia.Send(wMail);
                        enviado = true;
                        GravaLog.Log(operador, "Enviado e-mail para o usuário: #[" + usuario + "] no endereço: #[" + destinatario + "] com o assunto: #[" + assunto + "]");
                    }
                    catch (Exception ex) // Se der erro, grava no banco para enviar via serviço
                    {
                        GravaLog.Log(operador, "Erro ao enviar o e-mail em tempo real, gravando para enviar via serviço");
                        wMyEmail email = new wMyEmail();
                        email.corpo = corpo;
                        email.idPedido = 0;
                        email.Assunto = assunto;
                        email.Para = destinatario;
                        // Usuários que recebem cópia do e-mail (Vendedor, operador, etc.)
                        if (destinatariosCopia.Length > 0 && destinatariosCopia != "")
                        {
                            string[] aCC = destinatariosCopia.Split(';');
                            if (aCC.Length > 0)
                            {
                                for (int i = 0; i < aCC.Length; i++)
                                {
                                    email.CC += aCC[i].ToString().Trim().ToLower();
                                }
                            }
                        }
                        email.CCO = "suporte@inforshop.com.br;wolney.rodrigues@inforshop.com.br";
                        email.FormatoHTML = true;
                        email.EnviaEmailBanco();
                        email = null;
                        GravaLog.Log(operador, "Ocorreu o erro: (Sistema tentou enviar em 15 segundos)" + ex.Message.ToString() + " -> Enviado para o banco de dados.");
                    }
                    smEnvia.Dispose(); smEnvia = null; wMail.Dispose(); wMail = null;
                }
                else // Se não houver configuração na tabela, grava no banco para enviar via serviço
                {
                    wMyEmail email = new wMyEmail();
                    email.corpo = corpo;
                    email.idPedido = 0;
                    email.Assunto = assunto;
                    email.Para = destinatario;
                    // Usuários que recebem cópia do e-mail (Vendedor, operador, etc.)
                    if (destinatariosCopia.Length > 0 && destinatariosCopia != "")
                    {
                        string[] aCC = destinatariosCopia.Split(';');
                        if (aCC.Length > 0)
                        {
                            for (int i = 0; i < aCC.Length; i++)
                            {
                                email.CC += aCC[i].ToString().Trim().ToLower();
                            }
                        }
                    }
                    email.CCO = "suporte@inforshop.com.br;wolney.rodrigues@inforshop.com.br";
                    email.FormatoHTML = true;
                    email.EnviaEmailBanco();
                    email = null;
                    GravaLog.Log(operador, "Tabela cy_configSMTP vazia ou sem registro/configuração SMTP ativa.");
                }
            }
            catch (Exception ex) { GravaLog.Log(operador, ex.Message.ToString()); }
            return enviado;
        }
        protected static DataRow configSMTP()
        {
            return _Global.DataObjetcs.ExecutaSQL_DataTable("select top 1 * from cy_configSMTP with(nolock) where smtp_ativo = 1 ").Rows[0];
        }
    }
}