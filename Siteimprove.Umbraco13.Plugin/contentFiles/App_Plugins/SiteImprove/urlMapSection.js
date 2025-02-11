angular.module("umbraco").controller("Siteimprove.UrlMapController", [
    "$scope", "$http",
    function ($scope, $http) {
        $scope.urlMap = { id: null, newDomain: null };
        const backofficeApiUrl = "/umbraco/backoffice/api/SiteImproveApi";

        $scope.saveUrlMap = function() {
            $http.post(`${backofficeApiUrl}/SaveUrlMap`, {
                Id: $scope.urlMap.id,
                NewDomain: $scope.urlMap.newDomain
            })
            .then(function(resp) {
                if (resp.data.success === true) {
                    setTimeout(siteimprove.reloadPage, 1000);
                }
            });
        }

        $scope.loadData = function () {
            $http.get(`${backofficeApiUrl}/getUrlMap`)
                .then(function (resp) {                                   
                    $scope.urlMap = resp.data;
                });
        };
        siteimprove.reloadData = $scope.loadData;
        $scope.loadData();
    }
])