using Store.Model;

namespace App.Services
{
    public class EmailTemplateService
    {
        const string _unsubscribe = "To stop receiving further information from TrendyCampus, <a href='https://trendycampus.com/unsubscribe/'> Unsubscribe</a>";
        const string _alert = "If you require any assistance, please reach out to us via email, <a href='mailto:info@trendycampus.com'>info@trendycampus.com</a> or call <a href='tel:+2348054567999'>+234 805 456 7999</a> <br>";
        const string _address = "A product of iconnellysconcepts Inc\r\nPlot 721 pazzari District Deidei, Abuja. <br>";
        const string _website = "Visit TrendyCampus for more information <a href='https://trendycampus.com/'>www.trendycampus.com</a> <br>";

        //const string _footer1 = "<p style='margin-top: 3em;font-size: 13px; text-align: center;'>" + _alert+"<br>"+ _address + "<br>" + _website+ "</p>";
        const string _footer2 = "<p class='footer tint'>" + _alert + "<br>" + _address + "<br>" + _website + "<br>" + _unsubscribe + "</p>";
        const string _headerMeta = @"
<!DOCTYPE html>
<html>
<head>
	<meta charset='utf-8'>
	<meta name='viewport' content='width=device-width, initial-scale=1'>
	<style>
		.footer{
margin-top: 3em;
font-size: 13px; 
text-align: center;
			padding: 20px;
}
		.container {
			max-width: 600px;
			margin: 0 auto;
			font-family: Arial, sans-serif;
			font-size: 16px;
			line-height: 1.5;
			box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1);
color: #000000;
		}
.tint{
			color: #333333;
			background-color: #f5f5f5;
padding:5px;
}
		.header {
			margin-bottom: 20px;
		}
		.header img {
			max-width: 100%;
			height: auto;
			display: block;
			margin: 0 auto;
		}
		.greeting {
			margin-bottom: 20px;
			text-align: center;
			font-size: 24px;
			font-weight: bold;
			color: #0072c6;
		}
		.button {
			display: block;
			margin: 0 auto;
			padding: 10px 20px;
			font-size: 18px;
			font-weight: bold;
			color: #ffffff;
			background-color: #0072c6;
			border-radius: 5px;
			text-align: center;
			text-decoration: none;
		}
		.button:hover {
			background-color: #005d9a;
		}
		.link {
			display: block;
			margin-top: 20px;
			text-align: center;
			color: #0072c6;
			text-decoration: none;
		}
		.link:hover {
			text-decoration: underline;
		}
	</style>
</head>
<body>";
        const string _header = @"
<div class='header'>
			<img src='https://trendycampus.com/img/logo1.png' style ='width:120px' alt='Header Image'>
<hr>
		</div>
";
        public static int NewRegistration(User user)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
		" + _header + @"
		<div class='greeting'>"
            + $"Hi {user.Fname}," +
        @"</div>
		<p style= 'font-size: 13px'>
			Welcome to TrendyCampus, and thank you for registering with us!.
<br> <br>
		</p>" + _footer2 + @"
	</div>
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = "Welcome To TrendyCampus",
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "noreply@trendycampus.com", "ot!0C13#9");
        }
        public static int VerifyEmail(User user)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
		" + _header + @"
		<div class='greeting'>"
            + $"Hi {user.Username}," +
        @"</div>
<div class='text-center'><p class='tint' style= 'font-size: 13px'>
		Your email verification code is:
		</p>
<h2 class='tint'>" + $"{user.EmailVerCode}" + @"</h2>
</div>
	
		
<br> <br>
" + _footer2 + @"
	</div>
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = "Email Verification",
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "noreply@trendycampus.com", "ot!0C13#9");
        }
        public static int ResetPassword(User user)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
		" + _header + @"
        < div class='greeting'>"
            + $"Hi {user.Username}," +
        @"</div>
		<div class='text-center'><p style= 'font-size: 13px'>
		Your Password Reset Code is:
<h2 class='tint'>" + $"{user.PasswordVer}" + @"</h2>
		</p></div>
	
<br><br>
" + _footer2 + @"
	</div>
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = "Password Reset",
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "noreply@trendycampus.com", "ot!0C13#9");
        }
        public static int UserActivated(User user)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
		<div class='header'>
			<img src='https://trendycampus.com/img/logo1.png' style ='width:100px' alt='Header Image'>
		</div>
		<div class='greeting'>"
            + $"Hi {user.Username}," +
        @"</div>
		<p style= 'font-size: 13px'>
Your TrendyCampus account has been successfully activated.
<br><br>
To earn without limits, share your referral link now. Click on Profile > Referral to copy your link.
<br><br>
Together we can make the world a better place.
		</p>
	<br> <br>
" + _footer2 + @"
		</div>
	
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = "Account Activated",
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "noreply@trendycampus.com", "ot!0C13#9");
        }
        public static int BroadCast(List<string> mails, string message, string subject)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
" + _header + @"
	" + message + @"
	<br> <br>
" + _footer2 + @"
		</div>
	
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = subject,
                Recipients = mails
            }, "alert@trendycampus.com", "oip!Jqi#882");
        }
        public static int SingleMail(User user, string message, string subject)
        {
            return EmailService.SendMail(new Email()
            {
                Message = @"
	<div class='container'>
" + _header + @"
	" + message + @"
	<br> <br>
" + _footer2 + @"
		</div>
	
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = subject,
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "alert@trendycampus.com", "oip!Jqi#882");
        }
        public static int SupportMail(User user, string message, string subject)
        {
            return EmailService.SendMail(new Email()
            {
                Message = _headerMeta + @"
	<div class='container'>
" + _header + @"
	" + message + @"
	<br> <br>
" + _footer2 + @"
		</div>
	
</body>
</html>

",
                DisplayName = "TrendyCampus",
                Subject = subject,
                Recipients = new List<string>()
                {
                            user.Email
                }
            }, "support@trendycampus.com", "o!T0A19#1");
        }
    }

}