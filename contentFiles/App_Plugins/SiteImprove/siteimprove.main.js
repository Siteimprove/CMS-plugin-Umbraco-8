function initializeSiteimprove($scope) {
	$.get(`${window.siteimprove.helper.backofficeApiUrl}/UmbracoVersion`).then(
		function (response) {
			initializeContentAssistant(response);
		}
	);
	var siHelper = window.siteimprove.helper;
	if ($scope) {
		$scope.$on(
			"$routeChangeSuccess",
			siHelper.on$routeChangeSuccess.bind(siHelper)
		);
	}
}

function initializeContentAssistant(umbracoVersion) {
	const helper = window.siteimprove.helper;
	const script = document.createElement("script");
	script.src = helper.overlayUrl;
	script.onload = function () {
		var si = window._si || [];
		si.push(["setSession", null, null, `umbraco-${umbracoVersion}`]);
		helper.clearPage();
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
							initializeSiteimprove(locals.$scope);
						}
					}
					return $delegate.apply(this, [].slice.call(arguments));
				};
			},
		]);
	},
]);
