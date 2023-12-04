﻿namespace PulseStore.BLL.Models.Email;

public class EmailConfiguration
{
    public string FromName { get; set; }
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public bool UseSsl { get; set; }
}