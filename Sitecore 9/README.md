# AMGEN Parsabiv Helix Project #

Project uses Helix principles. Setup instructions coming soon.

Please note that the project assumes the following settings:

* Source location: D:\Projects\xBlog2\
* Website location: D:\Web\wwwroot\xBlog2\
* Website URL: http://xBlog2/

To change these settings see the "Configuring your settings" below

### To Install: ###

1. Clone this repository to your local file system.
2. (optional) Configure your settings if you are using settings other than the defaults:
* The default settings for this project are
     * URL: http://xBlog2/
     * Location: D:\Web\wwwroot\Amgen.Brands.Parsabiv\
* Copy the following files, remove the .example suffix, and update paths to match local envrionment to set the location of the source files, website files, and website URL:
     * Please be aware to include or omit trailing slashes - as per the default
     * [root]\src\Project\Parsabiv\code\App_Config\Include\Project\zzz.Amgen.Brands.Parsabiv.Helix.DevSettings.config.example
     * [root]\gulp-config.js.example
     * [root]\publishsettings.targets.example
3. Install Solr server locally by following [these instructions](https://sitecore-community.github.io/docs/search/solr/installing-solr-using-the-bitnami-apache-solr-stack/).
     *  After installation, remove the authentication by opening this file:  <-InstallationDirectory->\solr-6.2.1-2\apache-solr\conf\solr.conf
     *  Remove the entire < LocationMatch > node.
     *  Restart both Solr Services (solrJetty and solrApache)
     *  Browse to the Solr URL and make sure the site comes up (i.e. http://localhost:8983/solr)
     *  You DO NOT need to install the index cores for now.
4. Setup a clean Sitecore 8.2 update 2 with [Sitecore Instance Manager QA Version](http://dl.sitecore.net/updater/qa/sim/)
    *  Make sure you install the SIM **QA** version.
    *  When installing, change the hostname to "parsabiv.dev.local"
    *  When installing, under the Additional Configuration presets, choose the "Configuration - Sitecore 8.2 - ContentSearch - Solr.zip".  This will automatically update the Sitecore configuration to use Solr and install the Solr cores into your local Solr server.
5. Restore Node.js modules
    *  Make sure you have the latest version of node.js [Download here](https://nodejs.org/)
    *  In an elevated command window run 'npm install' in the root of repository.
6. Install T4 Toolbox for Visual Studio 2015 in Extensions and Updates
7. Download [Website.zip](https://1drv.ms/u/s!Aonns65Nlp64qTO7VmY6HlU_gKEI). Extract the contents into your IIS Sitecore Website folder.
8. Perform the steps listed under "Publishing Visual Studio Solution" below.
9. In the /lib/BasePackages of your source folder, install the following packages IN THE ORDER shown below:
    *  AmgenBaseSerialization.zip
    *  Amgen.Common.zip
    *  Brands Folder.zip
    *  CSS Asset Page.zip
    *  Parsabiv Folders.zip
10. Perform the steps under "Sync with Unicorn" below.

### Publishing Visual Studio Solution ###

* Visual Studio 2015 is required.
* Make sure the paths are setup correctly.
* Open up the solution
* Bring up the "Task Runner Explorer" by going to View -> Other Windows
* In the dropdown, you should see "Solution 'Amgen.Brands.Parsabiv'
* Double click the "default" task
* This should compile and copy all the files needed to your installed Sitecore instance.
* For more info about the gulp tasks, refer to the [Habitat Instructions](https://github.com/Sitecore/Habitat/wiki/01-Getting-Started)

### Sync with Unicorn ###

* After the publish is complete, go to http://parsabiv.dev.local/unicorn.aspx
* Select all and click "Sync"

### Questions? ###

* Project Manager:  
    * Thomas Knudsen
    * Skype: tknudsen86
* Project Architect:
    * Amir Setoudeh
    * Skype: amir.setoudeh
    * email: amir.setoudeh@xcentium.com
    * cell: 818.516.4311