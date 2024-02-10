# About KRB.Utilities Classes
In this section, there are a number of classes and application middleware, they are mentioned below

* [ExcelExport](#excelexport)
- [ForceWwwMiddleware](#the-forcewwwmiddleware-class)
+ [View Convertor](#viewconvertor-class)

## ExcelExport Class
This class, named ExcelExport, is responsible for converting data stored in a list to an Excel file. Specifically, this class is useful in scenarios within ***ASP.NET MVC***  where there's a need to export data from a model to an Excel file format.

Using this class, we can take a list of any model type as input and export them to an Excel file format. The class provides the ExcelExport method, which takes the data list and the file path for the Excel file as input, and generates the desired Excel file using the data available in the list.

The algorithm of this class can be summarized as follows:

Create a new Excel file using the OpenXML library.
Create a new workbook and a new sheet within the Excel file.
Create a header row for the sheet using the column names present in the model and append it to the sheet.
Populate the sheet with the data available in the list and append it to the Excel file.
Save the Excel file to the specified file path.
With this class, you can easily export your data to the Excel file format and use it for further analysis or presentation to your users.

**To use the ExcelExport class, follow these steps:**

> Instantiate an object of the ExcelExporter class, specifying the type of data you want to export (generic type T).
> Prepare your data as a list of objects of the specified type (List<T>).
> Call the ExcelExport method of the ExcelExporter object, passing the data list and the file path where you want to save the Excel file.
> Here's an example of how you can use the ExcelExport class:

```C#
using System.Collections.Generic;
using KRB.Utilities.Infrastructure; // Assuming the namespace where ExcelExport class is located
namespace YourNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example data list
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Department = "HR", Salary = 5000 },
                new Employee { Id = 2, Name = "Jane Smith", Department = "IT", Salary = 6000 },
                // Add more employee objects as needed
            };

            // Instantiate ExcelExport for Employee type
            var excelExport = new ExcelExport<Employee>();

            // Export data to Excel
            excelExport = ExportToExcel(employees);

            // Inform the user about the successful export
            Console.WriteLine("Data has been exported to Excel successfully.");
        }
    }

    // Example Employee class
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }
}
```
## The ForceWwwMiddleware Class
The **ForceWwwMiddleware** class looks correct for redirecting requests to include the **"www"** prefix in ASP.NET Core. However, you may need to include some error handling and additional checks for special cases, depending on your application's requirements.
This middleware now includes the scheme, path, and query parameters in the redirect URL to preserve the original request details. It also checks for the scheme (HTTP or HTTPS) to ensure that the redirect works correctly for both types of requests. Additionally, it maintains the query string parameters if present in the original request.

Make sure to register this middleware in your **Startup.cs** file's **Configure** method:
```C#
public void Configure(IApplicationBuilder app)
{
    app.UseMiddleware<ForceWwwMiddleware>();

    // Other middleware configurations
}
```
With these adjustments, the middleware should effectively redirect requests to include the **"www"** prefix when necessary.


## ViewConvertor Class

**ViewConvertor:** This class is responsible for converting a Razor page into a string. This class serves as a utility tool that can convert a Razor view into a string using its own functions.

A practical example for using ViewConvertor:
This code snippet demonstrates an email template in Razor that is used to send a password reset link to users. which is saved with the name **_ResetPasswordConfirmation.cshtml**
```C#
@model KRB.Web.Model.EmailModel.ResetPasswordModel

<div style="direction: rtl; -ms-word-wrap: break-word; word-wrap: break-word;">
    <p>Dear @Model.UserName,</p>
    <p>&nbsp;Greetings,</p>
    <p>It appears that you have requested to reset your password. If this request is yours, please click on the link below. Otherwise, you can disregard this message.</p>
    <p>@Model.ResetLink</p>
    <div style="font-size: 13px; color: #808080;">
        <p>Thank you,</p>
        <p>@Model.SiteTitle</p>
        <p>@Model.SiteDescription</p>
        <p><span style="direction: ltr !important; unicode-bidi: embed;">@DateTime.Now</span></p>
    </div>
</div>
```

And This method responsible for sending a confirmation email after a password reset request.
```C#
private readonly IHttpContextAccessor _httpContextAccessor;
private readonly EmailServiceConfiguration _emailServiceConfiguration;

public YourController(IHttpContextAccessor httpContextAccessor, EmailServiceConfiguration emailServiceConfiguration)
{
    _httpContextAccessor = httpContextAccessor;
    _emailServiceConfiguration = emailServiceConfiguration;
}
private SendingMailResult SendResetPasswordConfirmation(string userName, string email, string guid,
            string subject,string BlogDescription, string BlogName)
{
    string mailServerUrl = "mail.google.com";
    string adminEmail = "TestUserName";
    string mailServerPass = "";
    string mailServerLogin = "";
    int mailServerPort = 25;
    var model = new ResetPasswordModel
    {
        UserName = userName,
        ResetLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{Url.Action("ResetPassword","ForgottenPassword", new { key = guid })}",
        SiteDescription = blogDescription,
        SiteTitle = blogName
    };
    return EmailService.Send(new MailDocument
    {
        Body = new ViewConvertor().RenderRazorViewToString("_ResetPasswordConfirmation", ControllerContext, model),
        Subject = $"{_emailServiceConfiguration.SiteUrl} - {blogName} - {subject}",
        ToEmail = email,
    }, new MailConfiguration
    {
        EnableSsl = true,
        SmtpServer = mailServerUrl,
        From = adminEmail,
        Password = mailServerPass,
        UserName = mailServerLogin,
        SmtpPort = mailServerPort
    });
}
```

These two pieces of code are combined to send emails containing reset password to users. It's noteworthy that ViewConvertor is used for converting Razor views into strings, while EmailService is utilized for sending emails.


