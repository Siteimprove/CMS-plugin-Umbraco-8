angular.module("umbraco").controller("Siteimprove.UrlMapController", [
	"$scope",
	"$http",
	"notificationsService",
	function ($scope, $http, notificationsService) {
		$scope.urlMap = { id: null, newDomain: null };
		const backofficeApiUrl = window.siteimprove.helper.backofficeApiUrl;

		$scope.saveUrlMap = function () {
			$http
				.post(`${backofficeApiUrl}/SaveUrlMap`, {
					Id: $scope.urlMap.id,
					NewDomain: $scope.urlMap.newDomain,
				})
				.then(function (resp) {
					if (resp.data.success) {
						notificationsService.success(resp.data.message);
					} else {
						notificationsService.error(resp.data.message);
					}
				});
		};

		$scope.loadData = function () {
			$http.get(`${backofficeApiUrl}/getUrlMap`).then(function (resp) {
				$scope.urlMap = resp.data;
			});
		};
		siteimprove.reloadData = $scope.loadData;
		$scope.loadData();
	},
]);
