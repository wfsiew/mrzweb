﻿// angular js controller for Index view
function IndexCtrl($scope, $http) {

    // a model to bind to the form
    $scope.model = {
        PassportNum: '',
        Nationality: '',
        DOB: {
            year: '',
            month: '',
            day: ''
        },
        Exp: {
            year: '',
            month: '',
            day: ''
        },
        Sex: '',
        Line: null
    };

    $scope.result = {};

    // function submit the request to the /Home/Validate action
    $scope.formsubmit = function () {
        var x = $scope.model;
        x.DOB.year = $scope.paddzero(x.DOB.year);
        x.DOB.month = $scope.paddzero(x.DOB.month);
        x.DOB.day = $scope.paddzero(x.DOB.day);

        x.Exp.year = $scope.paddzero(x.Exp.year);
        x.Exp.month = $scope.paddzero(x.Exp.month);
        x.Exp.day = $scope.paddzero(x.Exp.day);

        var o = {
            PassportNum: x.PassportNum,
            Nationality: x.Nationality,
            DOB: x.DOB.year + x.DOB.month + x.DOB.day + '',
            Sex: x.Sex,
            Expiry: x.Exp.year + x.Exp.month + x.Exp.day + ''
        };
        $http.post("/Home/Validate", o).success(function (data) {
            if (data.success == 1) {
                $scope.result = data.result;
                $scope.model.Line = data.result.Line;
            }

            else {
                alert(data.message);
            }
        });
    }

    // function to add padding to a given variable
    $scope.paddzero = function (a) {
        if (!$scope.isint(a))
            return '' + a;

        var i = parseInt(a, 10);
        var s = i < 10 ? '0' + i : '' + i;
        return s;
    }

    // function to check whether a given variable is integer
    $scope.isint = function (value) {
        return !isNaN(value) &&
               !isNaN(parseInt(value, 10));
    }

    // function to show the status of the validation result in terms of Pass/Fail
    $scope.status = function (a) {
        var s = a == true ? 'Pass' : 'Fail';
        return s;
    }
}

app.controller('IndexCtrl', ['$scope', '$http', IndexCtrl]);
