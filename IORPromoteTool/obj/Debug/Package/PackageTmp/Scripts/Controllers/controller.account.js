angular.module('controller.account', []).

controller('AccountCtrl', ['$scope', '$location', 'AccountAPI',
function ($scope, $location, AccountAPI) {
	
    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    /* Initialize User Account credentials */ 
    AccountAPI('AccountApi').get({},
        function (account) {
            $scope.isUserAdmin = account.UserAdmin;
            $scope.isRejectAdmin = account.RejectAdmin;
        });
}]);