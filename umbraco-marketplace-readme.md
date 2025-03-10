# Siteimprove Content Assistant plugin for Umbraco

## Description

Seamless integration of Siteimprove's insights.

Transform your content creation process with the `Siteimprove Content Assistant`, which integrates Siteimprove’s industry-leading insights directly into Umbraco.

This integration helps optimize your content before publishing, enhancing productivity and quality while ensuring it meets the highest standards of compliance. It makes your website more inclusive and helps drive more traffic.

Key Features:

-   Effortless Integration: Instantly connect Siteimprove’s insights to Umbraco content.
-   Real-time Feedback: Receive immediate suggestions and improvements for your content as you work, helping you maintain high standards of quality and compliance even before you publish.
-   Enhanced Accessibility: Ensure your content meets all accessibility guidelines, making your website inclusive for all users.
-   SEO Optimization: Get actionable recommendations to improve your search engine rankings and increase website traffic.

## Installation

### Prerequisites

-   Umbraco CMS (version 13)
-   A valid Siteimprove account

### Installation Steps

1. **Download the nuget package**:
    ```sh
    dotnet add package Siteimprove.Umbraco13.Plugin
    ```
1. **Build your project and files will be copied to**:
    ```sh
    ~/App_Plugins/Siteimprove
    ```

## How the plugin works?

As soon as you login to Umbraco, you will be able to see the `Siteimprove Content Assistant` icon on the right (or left, depending on your settings).

If you click on it you will open the side panel, but will only be able to see it's default page. To see some data, you will need to select some content.

The URL of the selected content will be sent to the `Siteimprove Content Assistant`, and if the selected site/page is connected with Siteimprove environment, then you will be able to see some data related to that specific page on the `Live` tab.

You can also go the the preview of a specific page and then you will be able to run a prepublish check. In this case, insights will be provided even if the page/site is not configured inside Siteimprove yet.

## Configuration in Umbraco

There's a Siteimprove section in the main menu, added by this plugin, where you can define a public URL. To display this section you will need allow it, going to the `Users` tab, selecting the option to edit section, and then adding `Siteimprove`. After that, you will need to logout and login again, and then you will be able to see the new section.

The purpose of this section is to define a new domain, when necessary, to replace the Umbraco domain. Then the URL with the new domain will be the one that will be passed to the `Siteimprove Content Assistant`. It can also be used for testing purpose.

For example, if you defined the public URL value as https://yourdomain and the URL of content page you are accessing now is https://umbracodomain/about/, the final URL that will be passed to Siteimprove is https://yourdomain/about/.
