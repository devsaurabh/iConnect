var app = angular.module('account', []);
app.controller('LoginController', function($scope, $http) {
    $scope.person = {};

    $scope.sendFomr = function() {
        $http({
            method: 'POST',
            url: 'Account/Login',
            data: $scope.person
        }).success(function(data, status, headers, config) {
            $scope.message = '';
            if (data.success == false) {
                var str = '';
                for (var error in data.errors) {
                    str += data.errors[error] + '\n';
                }
                $scope.message = str;
            }
            else {
                $scope.message = 'Saved Successfully';
                $scope.person = {};
            }
        });
    };
});