function HookPublishButton($scope) {
	// Hook on save and publish event
	$scope.$on("formSubmitted", function (form) {
		// Page has been submitted, but no id on the page. They just created a new page
		if (form.targetScope.content.id == 0) {
            // Published new page
			window.siteimprove.helper.isCreatingPage = true;
			return;
		}
        // Saved existing page
		window.siteimprove.helper.handleFetchPushUrl("recheck", form.targetScope.content.id, true);
	});
}

function InitializeSiteimprove($scope) {        
        $.get(`${window.siteimprove.helper.backofficeApiUrl}/GetUmbracoVersion`)
            .then(function(response) {
                initializeContentAssistant(response);
            });
        var siHelper = window.siteimprove.helper;        			
        if ($scope) {
            $scope.$on("$routeChangeSuccess", siHelper.on$routeChangeSuccess.bind(siHelper));
        }
};

function initializeContentAssistant(umbracoVersion) {
    const script = document.createElement("script");
    script.src = window.siteimprove.helper.overlayUrl;
    script.onload = function() {
        var si = window._si || [];
        si.push(["setSession", null, null, `umbraco-${umbracoVersion}`]);
        si.push(["clear"]);
    };
    document.body.appendChild(script);
}

app.config(["$provide", function ($provide) {
    $provide.decorator("$controller", [
        "$delegate",
        function ($delegate) {
            return function(constructor, locals) {
                if (typeof constructor == "string") {
					if(constructor == "Umbraco.MainController") {
						InitializeSiteimprove(locals.$scope);
					}
					if(constructor == "Umbraco.Editors.Content.EditController") {
                        HookPublishButton(locals.$scope);
                    }
                }
                return $delegate.apply(this, [].slice.call(arguments));
            }
        }]);
}]);