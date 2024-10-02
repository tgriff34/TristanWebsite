Hello! 👋

https://tristanwebsite-fka3ezcyduh4h7ck.canadacentral-01.azurewebsites.net/

I built this website to learn more about ASP.NET and full-stack development. It uses the ASP.NET Core framework utilizing the MVC design pattern and is written in C#, HTML, CSS, JavaScript, RESTful APIs, SQL, and more!

I hosted this website using Microsoft Azure, it allowed for easy secrets management using Azure Key Vault and fast way to deploy changes to a production environment.

On this page you can view or download my Resume, and if you navigate to 'My Strava' from the navigation bar you can view my most recent cycling rides! The Strava data is pulled using the Strava API and is deserialized into objects using the Newtonsoft.json Nuget package.

The maps drawn using the polyline received from the strava API which is then encoded and provided to the view.

**Please note** that website is fairly slow to load initially, so it may take awhile.  It is hosted in central Canada which is the only GEO I could use with a free Microsoft Azure account.  It's also run inside a Linux docker container instead of a Windows VM making it a lot cheaper to run but also a lot slower to load.

The home page contains the same little blurb as above, describing the stack used to build the website and why.

![image](https://github.com/user-attachments/assets/3d2a01a1-4fd2-497b-8c59-993123a6fb0e)

The strava page is a list of my recent cycling rides.

![image](https://github.com/user-attachments/assets/6f93fc36-010c-4e73-ad82-72baebbe818d)
