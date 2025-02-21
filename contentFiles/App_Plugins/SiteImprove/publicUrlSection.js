angular.module("umbraco").controller("Siteimprove.PublicUrlController", [
	"$scope",
	"$http",
	"notificationsService",
	function ($scope, $http, notificationsService) {
		$scope.publicUrl = "";
		const backofficeApiUrl = window.siteimprove.helper.backofficeApiUrl;

		$scope.savePublicUrl = function () {
			$http
				.post(
					`${backofficeApiUrl}/PublicUrl`,
					JSON.stringify($scope.publicUrl)
				)
				.then(function (resp) {
					if (resp.data.success) {
						notificationsService.success(resp.data.message);
					} else {
						notificationsService.error(resp.data.message);
					}
				});
		};

		$scope.loadData = function () {
			$http.get(`${backofficeApiUrl}/PublicUrl`).then(function (resp) {
				if (resp.data.success) {
					$scope.publicUrl = resp.data.publicUrl;
				} else {
					notificationsService.error(resp.data.message);
				}
			});
		};
		siteimprove.reloadData = $scope.loadData;
		$scope.loadData();
	},
]);
