angular.module("umbraco").controller("SiteImprove.DashboardController", [
    "$scope", "$http", "assetsService",
    function ($scope, $http, assetsService) {
        
        $scope.pathMap = { currentUrlPart: "", newUrlPart: ""};
        $scope.output = "";
        const backofficeApiUrl = "/umbraco/backoffice/api/SiteImproveApi";         

        $scope.savePathMap = function () {
            $http.post(`${backofficeApiUrl}/SaveUrlMap`,
                {
                    PageId: siteimprove.currentPageId,
                    CurrentUrlPart: $scope.pathMap.currentUrlPart,
                    NewUrlPart: $scope.pathMap.newUrlPart
                })
                .then(function(resp) {
                    $scope.output = resp.data.message;
                    if (resp.data.success === true) {
                        setTimeout(siteimprove.reloadPage, 1000);
                    }
                });
        };

        $scope.loadData = function () {
            $http.get(`${backofficeApiUrl}/getSettings`, { params: { pageId : siteimprove.currentPageId } })
                .then(function (resp) {                                   
                    $scope.pathMap.currentUrlPart = resp.data.currentUrlPart;
                    $scope.pathMap.newUrlPart = resp.data.newUrlPart;
                });
        };
        siteimprove.reloadData = $scope.loadData;
        $scope.loadData();
    }
]);