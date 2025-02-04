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
        var siHelper = window.siteimprove.helper;        			
        if ($scope) {
            $scope.$on("$routeChangeSuccess", siHelper.on$routeChangeSuccess.bind(siHelper));
        }
};

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