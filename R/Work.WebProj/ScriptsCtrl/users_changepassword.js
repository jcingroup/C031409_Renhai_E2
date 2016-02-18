var agApp = angular.module('angularApp', ['commfun', 'ui.bootstrap', 'toaster']);

agApp.controller('ctrl', ['$scope', '$http', 'toaster',
    function ($scope, $http, toaster) {
        $scope.submit = function () {
            $http.post(aj_MasterPasswordUpdate, $scope.fd)
            .success(function (data, status, headers, config) {
                console.log(data);
                if (data.result) {
                    $scope.fd = {}
                    //toaster.pop('success', js_Info_Toast_Return_Title, Info_ChangePassword_Success);
                    alert(Info_ChangePassword_Success);

                } else {
                    toaster.pop('error', msg_Err_System_Failure, data.message, 5000);
                }
            });
        }
    }
]);
