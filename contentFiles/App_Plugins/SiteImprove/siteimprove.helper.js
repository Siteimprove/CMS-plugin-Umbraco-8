var siteimprove = {
	log: false,
	currentPageId: 0,
	reloadData: function () {},
	reloadPage: function () {
		location.reload();
	},
};

siteimprove.helper = {
	backofficeApiUrl: "/umbraco/backoffice/api/Siteimprove",
	overlayUrl: "https://cdn.siteimprove.net/cms/overlay-latest.js",

	/**
	 * Will push method to _si
	 */
	pushSi: function (method, url) {
		const _si = window._si || [];

		if (siteimprove.log)
			console.log("Siteimprove pass: " + method + " - " + url);

		_si.push([method, url, ""]);
	},

	closeSi: function () {
		this.pushSi("clear", "");
	},

	/**
	 * Wrap simple ajax call getPageUrl
	 * @return {Promise}
	 */
	getPageUrl: function (pageId) {
		return $.get(`${this.backofficeApiUrl}/getPageUrl?pageid=${pageId}`)
			.then()
			.fail(function (error) {
				if (siteimprove.log) console.log("getPageUrl error:", error);
			});
	},

	/**
	 * Requests pageUrl from backoffice and send to SI
	 */
	handleFetchPushUrl: function (method, pageId, isFormPublish) {
		this.getPageUrl(pageId)
			.then(function (response) {
				if (response.success) {
					// When receive the url => send off to _si
					siteimprove.helper.pushSi(method, response.url);
				} else {
					if (isFormPublish) {
						siteimprove.helper.closeSi();
						return;
					}
					// If can't find page pass empty url
					siteimprove.helper.pushSi(method, "");
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
		if (next.params.id) {
			siteimprove.currentPageId = next.params.id;
		}
		// Only listen when user works on the content tree
		if (next.params.tree === "content") {
			if (
				!next.params.hasOwnProperty("create") &&
				(current === undefined ||
					!current.params.hasOwnProperty("create")) &&
				next.params.id
			) {
				siteimprove.helper.handleFetchPushUrl("input", next.params.id);
			}
		} else {
			// When not in content tree, we clear content assistant
			siteimprove.helper.closeSi();
		}
	},
};
