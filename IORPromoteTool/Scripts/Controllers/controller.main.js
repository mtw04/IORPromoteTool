angular.module('controller.main', []).

controller('MainCtrl', ['$scope', '$modal', function ($scope, $modal) {
	
    $scope.open = function (size) {

        var modalInstance = $modal.open({
            templateUrl: 'PartialViews/About/about.html',
            controller: ModalInstanceCtrl,
            size: size,
        });
    }

    var ModalInstanceCtrl = function ($scope, $modalInstance) {

        $scope.ok = function () {
            $modalInstance.dismiss('cancel');
        };
    };
}]);