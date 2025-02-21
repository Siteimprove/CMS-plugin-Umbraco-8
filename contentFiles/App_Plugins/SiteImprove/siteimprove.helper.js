var siteimprove = {
	log: false,
	reloadData: function () {},
};

/**
 * Will push method to _si
 */
function pushSi(method, url) {
	const _si = window._si || [];

	if (siteimprove.log)
		console.log("Siteimprove pass: " + method + " - " + url);

	_si.push([method, url, ""]);
}

siteimprove.helper = {
	backofficeApiUrl: "/umbraco/backoffice/api/Siteimprove",
	overlayUrl: "https://cdn.siteimprove.net/cms/overlay-latest.js",

	closeSi: function () {
		pushSi("clear", "");
	},

	/**
	 * Wrap simple ajax call getPageUrl
	 * @return {Promise}
	 */
	getPageUrl: function (pageId) {
		return $.get(`${this.backofficeApiUrl}/PageUrl?pageid=${pageId}`)
			.then()
			.fail(function (error) {
				if (siteimprove.log) console.log("getPageUrl error:", error);
			});
	},

	/**
	 * Requests pageUrl from backoffice and send to SI
	 */
	handleFetchPushUrl: function (pageId) {
		this.getPageUrl(pageId)
			.then(function (response) {
				if (response.success) {
					// When receive the url => send off to _si
					pushSi("input", response.url);
				} else {
					siteimprove.helper.closeSi();
				}
			})
			.fail(function (error) {
				if (siteimprove.log)
					console.log("handleFetchPushUrl error:", error);
				siteimprove.helper.closeSi();
			});
	},

	/**
	 * Handles the RouteChangeSuccess event. Will take care of pushing to _si and listen on publish event for the new edit scope.
	 */
	on$routeChangeSuccess: function (e, next, current) {
		// Only listen when user works on the content tree
		if (next.params.tree === "content") {
			if (
				!next.params.hasOwnProperty("create") &&
				(current === undefined ||
					!current.params.hasOwnProperty("create")) &&
				next.params.id
			) {
				siteimprove.helper.handleFetchPushUrl(next.params.id);
			}
		} else {
			// When not in content tree, we clear content assistant
			siteimprove.helper.closeSi();
		}
	},
};
