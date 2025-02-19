# Siteimprove Umbraco Plugin

## Install

Install the Nuget package in your Umbraco project. The Nuget package is automatically generated on the build of this project.

## Configuration in Umbraco

There's a Siteimprove section in the main menu, added by this plugin, where you can define a URL Map. To display this section you will need allow it, going to the `Users` tab, selecting the option to edit section, and then adding `Siteimprove`. After that, you will need to logout and login again, and then you will be able to see the new section.

The purpose of this section is to define a new domain, when necessary, to replace the Umbraco domain. Then the URL with the new domain will be the one that will be passed to the content assistant. It can also be used for testing purpose.

For example, if you defined the URL Map value as https://yourdomain and the URL of content page you are accessing now is https://umbracodomain/about/, the final URL that will be passed to the `input` function is https://yourdomain/about/.

## Run on local machine

Reference class library to Umbraco project, or install the Nuget package that is generated when building this project.

## How the plugin works?

As soon as you login to Umbraco, you will be able to see the content assistant icon on the right (or left, depending on your configs).

If you click on it you will open the side panel, but will only be able to see it's default page. To see some data, you will need to select a content.

The URL of the selected content will be sent to the content assistant, and if the selected site/page is connected with Siteimprove environment, then you will be able to see some data related to that specific page on the `Live` tab.

You can also go the the preview of a specific page and then you will also be able to run a prepublish check.
