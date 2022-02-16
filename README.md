# A Web-based Computer Laboratory Management System in PhilSCA Pasay Campus

The team proposed a project focused on developing a LAN website that aims to provide a solution that can help ensure effectiveness in the learning environment of computer laboratories in the Philippine State College of Aeronautics. 

The website has several abilities. It can monitor the output devices of units remotely to ensure students can participate in their classes. It can shut down units remotely and tell if they’re turned off or not. It automates attendances and designates students to their respective units to ensure that everyone has units during classes.

## To run in your environment
* Download or clone the main branch.
* Download & install [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet) for .NET 5.
* Download & install [MySQL 8+ Community](https://dev.mysql.com/downloads/mysql/).
* Download [PsTools](https://download.sysinternals.com/files/PSTools.zip), and set its directory to the environment variable.
    * To perform remote commands with PsTools, do the ff steps both to the server and client machines:
        * Disable User Account Control (UAC) remote restriction by setting up the reg key with the ff command line([source](https://stackoverflow.com/a/14103682)):
            * `reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system /v LocalAccountTokenFilterPolicy /t REG_DWORD /d 1 /f`
        * Enable File Sharing with the ff steps:
            * *Control Panel* > *Network and Internet* > *Network and Sharing Center* > *Change Advanced Sharing Settings* (tab) > select *Turn on file and printer sharing* (under your chosen service, usually the private network) 
        * If enabling File Sharing didn't work, ensure the ff:
            * The LanmanServer (to share files) and LanmanWorkstation (to request files) services must be running on the computer.
            * The SMB port (445 TCP) must be open on the firewalls between source and target computers.
* Configure the IIS to serve the site (for production)
    * To run it in dev, use the [iis-express](https://github.com/icflorescu/iisexpress-proxy) instead of IIS. This is much quicker in setting up.
    * Anyway, below lists the IIS settings you can try. We assume you have installed IIS.
        * Download & install the [hosting bundle](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/hosting-bundle?view=aspnetcore-5.0). 
        * If `HTTP Error 500.19` occurs, you might have a permission issue with the application pool. Try this solution from andy [HTTP Error 500.19 - Internal Server Error when publishing ASP.Net Core Web API to IIS Server](https://bytutorial.com/blogs/asp-net-core/http-error-50019---internal-server-error-when-publishing-aspnet-core-web-api-to-iis-server).
        * Set the inbound rules from your firewall for external requests.
* Configuring the DB server
    1. Import the [DB](Database/remote_desktop.sql)
    2. Insert directories in the columns that have *directory* in their names. You can find the columns in the ff tables, and also take note that the directories are relative to your machine:
        * admin_configs
        * desktops
    3. Insert values to all non-null columns. Check the imported DB.
* Reserve the computer IP addresses
    * The website needs its clients to have distinct IP addresses for certain features, such as designating a student to a particular unit. To ensure that, you can consider reserving the IP addresses, especially if you have a router with an automatic IP address system.
    * This mainly applies to a large number of computers, so let's say that if you have a single client, then you can skip this.

## Usage

This section must be done after doing the required configurations above in your environment.

### Development
1. Run the MySQL DB server, for example, `mysqld --console` command line.
2. Run the `dotnet run` command line.
3. Access the site from one of the links given by the `dotnet run` command line, or if you're using [iis-express](https://github.com/icflorescu/iisexpress-proxy), use what it gives.
