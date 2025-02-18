
# SiteImprove Umbraco Plugin 

## Install
Install either through the CMS or install the nuget both are located under /build/Package

## Configuration in Umbraco
The settings are located on the Siteimprove content tab
### Token
Required to authenticate communication between the add-on and Siteimprove.
### Recrawl id's
Specify all the id's that will trigger a recrawl. Comma separate the id's
### Modify the page URL by rewriting or removing a part of the Umbraco URL
Modify the page URL that is sent to Siteimprove, by replacing a part of the Umbraco URL. If the new part is empty, the current part will be removed.

Example:

```
Umbraco URL: https://www.website.com/about-us
Current URL part: about
New URL part: custom
Page URL that will be sent to Siteimprove: https://www.website.com/custom-us
```

The URL modification is inherited to all the descendant nodes without URL modifications.

The URL modification overrules any other modification from an ancestor node.


## Run on local machine
Reference class library to Umbraco project 

Add the following to Post-Build event command line:
``` shell
xcopy /I /E /Y "$(ProjectDir)bin\App_Plugins" "$(ProjectDir)App_Plugins"
```

## Check logs
Start console output after document ready
```js
siteimprove.log = true
```

## Build the Umbraco package
Run the build.bat

## TODO
 - Upload package to some nuget repository