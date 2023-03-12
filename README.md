<h1 align="center">Work for you project</h1>

## Table of content
1. :information_source: [General info about project](#project-description)
2. :file_folder: [TODO list](#todolist)
3. :bookmark_tabs: [Stack](#stack)
4. :clipboard: [Install instruction](#install-instruction)
5. :speech_balloon: [Contact me](#contact-me)

## Project description
Project for job search and job candidates. The possibility of registering a candidate or employer account. The employer represents the company, provides detailed information about it and contact details. The candidate has the opportunity to respond to the vacancy created by the employer, who in turn can invite the candidate to the position. Basically, the general functionality is similar to djinni.co.

## TodoList
- [x] responsive for design
- [x] email list design
- [x] a page for unauthorized users only and a page for authorized users only
- [x] candidate or employer registration + authorization
- [x] delete, preview account
- [x] account confirmation
- [x] password recovery
- [x] password change
- [ ] authentication with google, linkedin, etc (add IdentityServer)
- [x] profile change for candidate or employer
- [x] CRUD operations for vacancies for the employer
- [x] for a candidate - a list of vacancies; for an employee - a list of workers
- [x] make filters for searching vacancies and candidates: country, work experience, employment, company type, salary from..., English level, technical / non-technical tags, simple search
- [x] possibility to save vacancies or candidates (saved list)
- [x] mini-rating of vacancies: views, number of reviews, number of saves, etc
- [ ] the possibility to subscribe to the company: when new vacancies appear - a letter to the mail or a message; or make them hidden
- [ ] list of recommendations for the candidate. When a candidate goes to the job page, they see a list of similarities with their profile
- [x] the ability to communicate via chat
- [ ] notification system when interacting with the chat and receiving feedback from the candidate
- [ ] changing the user's photo on the profile page
- [ ] redesign the profile change system

## Stack
- ASP.NET core MVC (+ Web API)
- HTML/CSS JavaScript (TypeScript, Bootstrap)
- T-SQL (MS SQL), EntityFrameworkCore
- SignalR

## Install instruction
1. Clone the repository. This can be done using one of the following steps:

<dl><dd><dl><dd><dl><dd>
  <ol>
    <li><strong>VisualStudio</strong>: when opening, select the item "Clone the repository" and insert there [link to the repository].git (in our case https://github.com/saibel203/workForYou.git).</li>
    <li><string>Git commands</strong>: <strong>git clone [repositoryLink] [directory]</strong>, where <i>directory</i> - folder where to clone. Here is the <a href="https://git-scm.com/docs/git-clone" target="_blank">documentation</a> for the command.</li>
  </ol>
</dl></dd></dl></dd></dl></dd>

2. The site uses the **SendGrid** mail service. For full functionality (account confirmation at registration, password recovery, password change, etc.), you need to register on their [site](https://sendgrid.com/) (keep in mind that the registration process is quite long). After registration, you need to do the following:

<dl><dd><dl><dd><dl><dd>
  <ol>
    <li>Go to your profile settings <i>(/settings/sender_auth)</i> and create a <strong>Server Identity</strong> - this is some information about the sender of all emails. Usually, they indicate their own e-mail address or the e-mail address of the company on whose behalf everything will be sent.</li>
    <li>Then you need to use their API. Go to <strong>Setup Guide - Send your first email</strong> <i>(/guide/integrate)</i> and select Web API. Next, you need to choose the language in which the SendGridClient client (in our case - C#). On the page that opens, only one thing is important to us the item - <strong>Create API Key </strong>. It must be created and somewhere copied, as it will not be available later. Next, you need to click on the checkbox and the "Next" button.</li>
  </ol>
</dl></dd></dl></dd></dl></dd>

3. If you have registered on the SendGrid site, you need to modify the configuration files of our project:

<dl><dd><dl><dd><dl><dd>
  <ol>
    <li>First, in the <strong>appsettings.json</strong> file of the WorkForYou.WebUI project, in the SendGridOptions section, you need to specify your Server Identity information (created in point 2.1). The e-mail must match, and the nickname can be anything.</li>
    <li>Our API key that we generated on the site is private. It cannot be thrown away or given to anyone, otherwise it will be automatically blocked. Theoretically, it can be specified in the following places:</li>
    <ol>
      <li>If you're only going to use the project on your local machine and not expose it over the network, you can put it in appsettings.json by adding a field called "ApiKey" to the SendGridOptions section and specifying your key.</li>
      <li>A more correct solution would be to use <strong>secrets.json</strong>. To use it, you need to go to the WebUI project folder in the command line <strong>cd [...]/src/WorkForYou.WebUI</strong>. Then you need to register <strong>dotnet user-secrets init</strong>. This will create our secrets.json in the AppData/Microsoft/UserSecrets/[id]/secrets.json folder. Also, this is to add to the csproj file of our project a unique identifier stored in the <strong>UserSecretsId</strong>. Then you need to use the command <strong>dotnet user-secrets set "SendGridOptions:ApiKey" "your api key"</strong>. After that everything will work. Also, if some error occurred, there are commands: <strong>dotent user-secrets clear</strong> - clear file secrets.json and <strong>dotnet user-secrets remove "key-name"</strong> - remove key from secrets.json.</li>
    </ol>
  </ol>
</dl></dd></dl></dd></dl></dd>

4. Next, in the **appsettings.json** file, you need to change the following:

<dl><dd><dl><dd><dl><dd>
  <ol>
    <li>In the section WebUIOptions, change the ApplicationUrl value to the appropriate domain <strong>(there must be a missing character at the end of the line /)</strong></li>
    <li>In the section ConnectionStrings, change DefaultConnectionString to your connection string. By default we use Local SqlExpress and database name work_for_you.</li>
  </ol>
</dl></dd></dl></dd></dl></dd>

## Contact me
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/Saibel203)
[![LinkedIn](https://img.shields.io/badge/linkedin-%230077B5.svg?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/%D0%BC%D0%B0%D0%BA%D1%81%D0%B8%D0%BC-%D0%BB%D0%BE%D0%B3%D0%B2%D1%96%D0%BD-335a03254/)
[![Instagram](https://img.shields.io/badge/Instagram-%23E4405F.svg?style=for-the-badge&logo=Instagram&logoColor=white)](https://www.instagram.com/saibel.og/)
