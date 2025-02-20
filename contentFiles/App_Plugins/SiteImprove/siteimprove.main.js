function InitializeSiteimprove($scope) {
	$.get(
		`${window.siteimprove.helper.backofficeApiUrl}/GetUmbracoVersion`
	).then(function (response) {
		initializeContentAssistant(response);
	});
	var siHelper = window.siteimprove.helper;
	if ($scope) {
		$scope.$on(
			"$routeChangeSuccess",
			siHelper.on$routeChangeSuccess.bind(siHelper)
		);
	}
}

function initializeContentAssistant(umbracoVersion) {
	const script = document.createElement("script");
	script.src = window.siteimprove.helper.overlayUrl;
	script.onload = function () {
		var si = window._si || [];
		si.push(["setSession", null, null, `umbraco-${umbracoVersion}`]);
		si.push(["clear"]);
	};
	document.body.appendChild(script);
}

app.config([
	"$provide",
	function ($provide) {
		$provide.decorator("$controller", [
			"$delegate",
			function ($delegate) {
				return function (constructor, locals) {
					if (typeof constructor == "string") {
						if (constructor == "Umbraco.MainController") {
							InitializeSiteimprove(locals.$scope);
						}
					}
					return $delegate.apply(this, [].slice.call(arguments));
				};
			},
		]);
	},
]);
